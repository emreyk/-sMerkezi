using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class HesapHaraketController : AdminController
    {

        private readonly IBorclandirmaService _borclandirmaService;
        private readonly IBorcTipleri _borcTipleri;
        private readonly IBagimsizBolumlerService _bagimsizBolumService;
        private readonly IKisilerService _kisilerService;
        private readonly IKasaService _kasaService;
        private readonly IBankaService _bankaService;
        private readonly IUnitofWork _uow;


        public HesapHaraketController(IUnitofWork uow, IBorclandirmaService borclandirmaService,IKisilerService kisilerService,IBagimsizBolumlerService bagimsizBolumlerService,IKasaService kasaService,IBankaService bankaService,IBorcTipleri borcTipleri)
       : base(uow)
        {
            _uow = uow;
            _borclandirmaService = borclandirmaService;
            _kisilerService = kisilerService;
            _bagimsizBolumService = bagimsizBolumlerService;
            _kasaService = kasaService;
            _bankaService = bankaService;
            _borcTipleri = borcTipleri;
        }



        public ActionResult Index()
        {
            var liste = _borclandirmaService.BorclandirmaListesi();
            return View(liste);
        }

        public ActionResult VadeliBorclandir()
        {
            return View();
        }


        public ActionResult TahsilatListesi()
        {
            var tahsilatListesi = _borclandirmaService.TahsilatListesi();
            return View(tahsilatListesi);
        }

        public ActionResult KisiBagimsizBolumleri(int id)
        {
            var sonuc = _kisilerService.KisiBagimsizBolumleri(id);

            if (sonuc != null)
            {
                return Json(sonuc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Kaydet()
        {
            ViewBag.Kisiler = _kisilerService.KisiGetir();
            return View();
        }

        public ActionResult KaydetIslem(hesap_hareket model)
        {
            model.islem_tarihi = DateTime.Now;

         

            if (model.islem_turu == "borç dekontu")
            {
                model.alacak = 0;
            }
            else
            {
                model.borclandirma_turu = _borclandirmaService.BorclandirmaTuru(model.tahsilat_id);
                model.borc = 0;
            }

            var sonuc = _borclandirmaService.BorclandirmaKaydet(model);
            if (sonuc==true)
            {
                return Json(sonuc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BorcGirisi(int id)
        {
            var borclandirmaTurleri = _borcTipleri.BorcTipleriGetir();
            ViewBag.borcTiperi = borclandirmaTurleri;

            var kisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id,"");
            var bagimsizBilgiler = _bagimsizBolumService.BagimsizBolumBilgileriGetir(id);
            ViewBag.kisiler = kisiler;

            return View(bagimsizBilgiler);
        }

        //public ActionResult TahsilEt(int id)
        //{
        //    var kisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id);
        //    var bagimsizBilgiler = _bagimsizBolumService.BagimsizBolumBilgileriGetir(id);

        //    var kasalar = _kasaService.KasaGetir();
        //    var bankalar = _bankaService.BankaGetir();

        //    ViewBag.kisiler = kisiler;
        //    ViewBag.kasalar = kasalar;
        //    ViewBag.bankalar = bankalar;


        //    return View(bagimsizBilgiler);
        //}

        public ActionResult TahsilEtId(int id)
        {
            //var kisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id);
            var kasalar = _kasaService.KasaGetir();
            var bankalar = _bankaService.BankaGetir();
            //ViewBag.kisiler = kisiler;
            hesap_hareket modelHh = new hesap_hareket();
            modelHh.id = id;
            double kisiKalanBakiye = _borclandirmaService.KalanBakiyeKontrol(modelHh);

            ViewBag.kasalar = kasalar;
            ViewBag.bankalar = bankalar;
            ViewBag.KalanBakiye = kisiKalanBakiye;


            var model = _borclandirmaService.HesapHareketId(id);
            return View(model);
        }

        public ActionResult FinansalTahsilEt()
        {
          
            var kasalar = _kasaService.KasaGetir();
            var bankalar = _bankaService.BankaGetir();

            ViewBag.Kisiler = _kisilerService.KisiGetir();

            ViewBag.kasalar = kasalar;
            ViewBag.bankalar = bankalar;
            return View();
        }


        public ActionResult FinansalBorcDuzenle(int id)
        {

            var borclandirmaTurleri = _borcTipleri.BorcTipleriGetir();
            ViewBag.borcTiperi = borclandirmaTurleri;

            var finansBorcBilgisi = _borclandirmaService.FinansBorcDuzenle(id);
            return View(finansBorcBilgisi);
        }

        public ActionResult FinansalBorcGuncelle(EFinansalBorcDuzenle model)
        {
            //bu borca  ait tahsilat varsa retun -1

            if (_borclandirmaService.TahsilatKontrolId(model.id) == true)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            var sonuc = _borclandirmaService.FinansalBorcGuncelle(model);
            if (sonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BorcSil(hesap_hareket model)
        {
            //bu borca  ait tahsilat varsa retun -1

            if (_borclandirmaService.TahsilatKontrolId(model.id) == true)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

            var silSonuc = _borclandirmaService.HesapHaraketSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult TahsilatSil(hesap_hareket model)
        {
            var silSonuc = _borclandirmaService.TahsilatSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinansalTahsilatDuzenle(string refno)
        {
            var finansTahsilatBilgisi = _borclandirmaService.FinansTahsilatDuzenle(refno);

            var kasalar = _kasaService.KasaGetir();
            var bankalar = _bankaService.BankaGetir();

       
            ViewBag.kasalar = kasalar;
            ViewBag.bankalar = bankalar;

            if (finansTahsilatBilgisi.kasa_id !=0)
            {
                ViewBag.KasaAdi = _kasaService.KasaBilgileri(finansTahsilatBilgisi.kasa_id).kasa_adi;
            }
            else if(finansTahsilatBilgisi.banka_id != 0)
            {
                ViewBag.BankaAdi = _bankaService.BankaBilgileri(finansTahsilatBilgisi.banka_id).banka_adi;
            }


            return View(finansTahsilatBilgisi);
        }


        public ActionResult FinansalTahsilatGuncelle(EFinansalTahsilatDuzenle model,hesap_hareket hhModel)
        {
            //double kisiKalanBakiye = _borclandirmaService.KalanBakiyeKontrol(hhModel);

            //if (hhModel.alacak>kisiKalanBakiye)
            //{
            //    return Json("fazlatuta", JsonRequestBehavior.AllowGet);
            //}

            var sonuc = _borclandirmaService.FinansalTahsilatGuncelle(model,hhModel);
            if (sonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BakiyeKontrol(hesap_hareket model)
        {
           var  kalanBakiye = _borclandirmaService.KalanBakiyeKontrol(model);

       

            if (model.alacak>kalanBakiye)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
          
        }


        //public ActionResult PesinTahsilat(int id)
        //{
        //    var kisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id);
        //    var bagimsizBilgiler = _bagimsizBolumService.BagimsizBolumBilgileriGetir(id);
        //    ViewBag.kisiler = kisiler;


        //    var kasalar = _kasaService.KasaGetir();
        //    var bankalar = _bankaService.BankaGetir();


        //    ViewBag.kasalar = kasalar;
        //    ViewBag.bankalar = bankalar;

        //    return View(bagimsizBilgiler);
          
        //}

        public ActionResult PesinTahsilatKaydet(hesap_hareket model)
        {

            var pesintahsilatModel = _borclandirmaService.PesinTahsilatKaydet(model);

            if (pesintahsilatModel==true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult GunSayisiGetir(string ay)
        {
            int gunSayisi = _borclandirmaService.GunSayisiGetir(ay);

            if (gunSayisi != 0)
            {
                return Json(gunSayisi, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AkaryakitDonemKontrol(int kisi_id)
        {
            bool kontrol = _borclandirmaService.AkaryakitDonemKontrol(kisi_id);

            if (kontrol == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TahsilatTakip()
        {
            var yillar = _borclandirmaService.Yillar();
            return View(yillar);
        }
    }
}