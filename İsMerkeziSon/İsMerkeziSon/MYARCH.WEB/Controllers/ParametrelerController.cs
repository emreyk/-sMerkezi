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
    public class ParametrelerController : AdminController
    {
        private readonly IBorcTipleri _borcTipleri;
        private readonly IAidatGunService _aidatGunService;
        private readonly IVadeGunSayisiService _vadeGunSayisi;
        private readonly IUnitofWork _uow;

        public ParametrelerController(IUnitofWork uow, IAidatGunService aidatGunService, IVadeGunSayisiService vadeGunSayisi,IBorcTipleri borcTipleri)
  : base(uow)
        {
            _uow = uow;
            _aidatGunService = aidatGunService;
            _borcTipleri = borcTipleri;
            _vadeGunSayisi = vadeGunSayisi;

        }

        public ActionResult AidatGun()
        {
            var aidatGun = _aidatGunService.AidatGunGetir();
            return View(aidatGun);
        }


        //public ActionResult AidatGncelle(borc_tipleri model)
        //{
        //    var adatGunGuncelle = _aidatGunService.AidatGunGetir(model.para_birimi);
        //}

        public ActionResult VadeGunSayisi()
        {
            var vadeGunSayisi = _vadeGunSayisi.VadeGunSayisiGetir();
            return View(vadeGunSayisi);
        }
        
        public ActionResult AidatGunuGuncelle(string aidatGunu)
        {
            var guncelle = _aidatGunService.AidatGunuGuncelle(aidatGunu);
            if (guncelle == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VadeGuncelle(string vadeGunSayisi)
        {
            var guncelle = _vadeGunSayisi.VadeGunSayisiGuncelle(vadeGunSayisi);
            if (guncelle == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

       

        public ActionResult YakitTanimla()
        {
            ViewBag.akaryakitParaBirimi = _borcTipleri.ParaBirimiAkaryakit();
            var yakitBilgileri = _vadeGunSayisi.YakitBilgileri();
            return View(yakitBilgileri);
        }

        public ActionResult YakitTanimlaKaydet(yakit model)
        {
            var sonuc = _vadeGunSayisi.YakitKaydet(model);
            if (sonuc == true)
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