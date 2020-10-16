using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class TopluBorclandirController : AdminController
    {
        private readonly IBorcTipleri _borcTipleri;
        private readonly IBorclandirmaService _borclandirService;
        private readonly ITopluBorclandirService _topluBorclandirService;
        private readonly IAidatGunService _aidatGunService;
        private readonly IVadeGunSayisiService _vadeGunSayisiService;
      
        private readonly IUnitofWork _uow;

        public TopluBorclandirController(IUnitofWork uow, ITopluBorclandirService topluBorclandirService, IAidatGunService aidatGunService, IVadeGunSayisiService vadeGunSayisiService, IBorclandirmaService borclandirService, IBorcTipleri borcTipleri)
        : base(uow)
        {
            _uow = uow;
            _topluBorclandirService = topluBorclandirService;
            _aidatGunService = aidatGunService;
            _vadeGunSayisiService = vadeGunSayisiService;
            _borclandirService = borclandirService;
            _borcTipleri = borcTipleri;
        }

        public ActionResult Index()
        {

            var topluBorcListesi = _topluBorclandirService.TopluBorclandirmalar();
            return View(topluBorcListesi);
        }

        public ActionResult Kaydet()
        {
            var borclandirmaTurleri = _borcTipleri.BorcTipleriGetir();
            ViewBag.borcTiperi = borclandirmaTurleri;

            ViewBag.AidatGun = _aidatGunService.AidatGunGetir().aidat_gunu;
            ViewBag.VadeGunSayisi = _vadeGunSayisiService.VadeGunSayisiGetir().vade_gun_sayisi;

            return View();
        }


        public ActionResult KaydetIslem(toplu_borclandir model, hesap_hareket hesapModel, string tip)
        {


            model.ay = hesapModel.tarih.Month;
            model.yil = hesapModel.tarih.Year;
            model.gun = hesapModel.tarih.Day;

            var borcKontrol = _borclandirService.borclandirmaKontrol(model);
            if (borcKontrol == false)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            else
            {
                var kaydetSonuc = _topluBorclandirService.TopluBorclandirKaydet(model, hesapModel, tip);


                if (kaydetSonuc == true)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
        }


        public ActionResult Sil(string refno, string borclandirmaTuru)
        {
            //bu toplu boçlandırmaya ait tahsilat varsa retun -1

            if (_borclandirService.TahsilatKontrolRefno(refno) == true)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            var silSonuc = _topluBorclandirService.TopluBorcSil(refno, borclandirmaTuru);
            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Duzenle(string refno)
        {
            var topluBorclandirma = _topluBorclandirService.TopluBorclandirRefno(refno);

            var borclandirmaTurleri = _borcTipleri.BorcTipleriGetir();
            ViewBag.borcTiperi = borclandirmaTurleri;

            return View(topluBorclandirma);
        }


        public ActionResult DuzenleIslem(toplu_borclandir model, hesap_hareket hesapModel, string tip)
        {

            hesapModel.para_birimi = _borcTipleri.paraBirimiGetir(hesapModel.borclandirma_turu);

            model.ay = hesapModel.tarih.Month;
            model.yil = hesapModel.tarih.Year;
            model.gun = hesapModel.tarih.Day;

            try
            {
                var topluBorcandirListe = _topluBorclandirService.TopluBorclandirRefno(model.refno);
                if (topluBorcandirListe.tutar == 0)
                {
                    topluBorcandirListe.tutar = null;
                }

                if (topluBorcandirListe.ay == model.ay && topluBorcandirListe.yil == model.yil && topluBorcandirListe.yil == model.yil && topluBorcandirListe.tarih == model.tarih && topluBorcandirListe.sonodeme_tarihi == model.sonodeme_tarihi && topluBorcandirListe.tip == model.tip && topluBorcandirListe.aciklama == model.aciklama && topluBorcandirListe.dagitim_sekli == model.dagitim_sekli && topluBorcandirListe.borclandirma_turu == model.borclandirma_turu && topluBorcandirListe.tutar == model.tutar)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                //bu toplu boçlandırmaya ait tahsilat varsa retun -1
                if (_borclandirService.TahsilatKontrolRefno(model.refno) == true)
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _topluBorclandirService.TopluBorcSil(model.refno, model.borclandirma_turu);
                    var kaydetSonuc = _topluBorclandirService.TopluBorclandirKaydet(model, hesapModel, tip);

                    if (kaydetSonuc == true)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }


        public ActionResult AkaryakitKontrol()
        {
            var akaryakitDeger = _topluBorclandirService.AkarYakitKontrol();
            if (akaryakitDeger == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AkaryakitBorclanidrmaSayisi()
        {
            var sayi = _topluBorclandirService.AkaryakitBorclanidrmaSayisi();
            if (sayi == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}