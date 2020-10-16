using MYARCH.CORE.Entities;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class MuhtelifController : AdminController
    {
        private readonly IUnitofWork _uow;
        private readonly IKasaService _kasaService;
        private readonly IBankaService _bankaService;
        private readonly IMuhtelifBasliklarService _muhtelifBasliklarService;
        private readonly IMuhtelifIslemlerService _muhtelifIslemlerService;

        private readonly IGenericRepository<muhtelif_baslikler> _muhtelifBasliklar;
        //private readonly IGenericRepository<muhtelif_islemler> _muhtelifIslemler;

        public MuhtelifController(IUnitofWork uow,IKasaService kasaService, IBankaService bankaService,IMuhtelifBasliklarService muhtelifBasliklarService,IMuhtelifIslemlerService muhtelifIslemlerService)
         : base(uow)
        {
            _uow = uow;
            _kasaService = kasaService;
            _bankaService = bankaService;
            _muhtelifBasliklarService = muhtelifBasliklarService;
            _muhtelifIslemlerService = muhtelifIslemlerService;
            _muhtelifBasliklar = _uow.GetRepository<muhtelif_baslikler>();
        }

        public ActionResult Basliklar()
        {
            var baslikler = _muhtelifBasliklarService.basliklar();
            return View(baslikler);
        }

        public ActionResult MuhtelifGuncelle(int id)
        {
            var baslik = _muhtelifBasliklarService.BaslikGetir(id);
            return View(baslik);
        }
       
        public ActionResult MuhtelifGuncelleIslem(muhtelif_baslikler model)
        {
            if (_muhtelifBasliklarService.Guncelle(model)==true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ToplamaEkleSecim(string baslikKodu)
        {
           string secim = _muhtelifBasliklarService.ToplamaEkleSecim(baslikKodu);
            if (secim != null || secim != "")
            {
                return Json(secim, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(secim, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Ekle()
        {
          
            return View();
        }


        public  ActionResult KaydetIslem(muhtelif_baslikler model)
        {
            Random random = new Random();
            int sayi = random.Next(10, 99);

            model.baslik_kodu = model.baslik_adi.Substring(0, 1) + sayi;

            if (_muhtelifBasliklarService.Kaydet(model)==true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
         
        }

        public ActionResult MuhtelifBanka()
        {
            ViewBag.Bankalar = _bankaService.BankaGetir();
            ViewBag.Basliklar = _muhtelifBasliklarService.basliklar();
            return View();
        }


        public ActionResult MuhtelifKasa()
        {
            ViewBag.Kasalar = _kasaService.KasaGetir();
            ViewBag.Basliklar = _muhtelifBasliklarService.basliklar();
            return View();
        }

        public ActionResult BaslikSil(muhtelif_baslikler model)
        {
            var silSonuc = _muhtelifBasliklarService.baslikSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MuhtelifIslemSil(muhtelif_islemler model)
        {
            var silSonuc = _muhtelifBasliklarService.MuhtelifIslemSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //banka
        public ActionResult MuhtelifKaydetIslem(muhtelif_islemler model,int banka_id)
        {
            var paraBirimi = _muhtelifBasliklar.GetAll().Where(x => x.baslik_kodu == model.islem_no).FirstOrDefault().para_birimi;
            model.para_birimi = paraBirimi;

            if (_muhtelifIslemlerService.Kaydet(model,banka_id)==true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        
        }

        //kasa kaydet
        public ActionResult MuhtelifKasaKaydetIslem(muhtelif_islemler model, int kasa_id)
        {
            var paraBirimi = _muhtelifBasliklar.GetAll().Where(x => x.baslik_kodu == model.islem_no).FirstOrDefault().para_birimi;
            model.para_birimi = paraBirimi;

            if (_muhtelifIslemlerService.KaydetKasa(model, kasa_id) == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        // muhtelif işlem listesi
        public ActionResult Liste()
        {
            var liste = _muhtelifIslemlerService.Liste();
            return View(liste);
        }


        //muhtelif kasa guncelle
        public ActionResult KasaGuncelle(int id)
        {

            ViewBag.IslemAdlari = _muhtelifBasliklar.GetAll().ToList();
            ViewBag.kasalar = _kasaService.KasaGetir();

            var muhIslemGelen = _muhtelifIslemlerService.MuhtelifGetir(id);
            return View(muhIslemGelen);
        }

        //muhtelif banka guncelle
        public ActionResult BankaGuncelle(int id)
        {
            ViewBag.IslemAdlari = _muhtelifBasliklar.GetAll().ToList();

            var muhIslemGelen = _muhtelifIslemlerService.MuhtelifGetir(id);
            return View(muhIslemGelen);
        }

        public ActionResult KasaGuncelleIslem(muhtelif_islemler model)
        {
            if (_muhtelifIslemlerService.KasaGuncelle(model) == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BankaGuncelleIslem(muhtelif_islemler model)
        {
            //if (_muhtelifIslemlerService.KasaGuncelle(model,model)==true)
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{

            //}

            return null;
        }
    }
}