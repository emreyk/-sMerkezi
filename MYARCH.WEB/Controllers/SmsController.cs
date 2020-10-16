using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using SahinSms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MYARCH.WEB.Controllers
{
    public class SmsController : AdminController
    {

        private readonly ISmsService _smsService;
        private readonly IKisilerService _kisilerService;
        private readonly IBorclandirmaService _borclandirmaService;
        private readonly IUnitofWork _uow;

        public SmsController(IUnitofWork uow, ISmsService smsService, IKisilerService kisilerService,IBorclandirmaService borclandirmaService)
: base(uow)
        {
            _uow = uow;
            _smsService = smsService;
            _kisilerService = kisilerService;
            _borclandirmaService = borclandirmaService;
        }


        SahinHaberlesme sms = new SahinHaberlesme();
        List<string> numaralar = new List<string>();

        public ActionResult Index()
        {
            var smsListesi = _smsService.SmsListesi();
            return View(smsListesi);
        }

        public ActionResult Kaydet()
        {
            var kisiListesi = _kisilerService.KisiListesiModal();

            return View(kisiListesi);
        }

        public ActionResult KaydetIslem(sms model, string tip)
        {
            model.tarih = DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
               
                sms.gettoken("5332563356", "7304707");
                string mesaj = model.icerik;
                string gonMesaj = Fonksiyonlar.tr2en(mesaj);
                string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (tip == "kisi")
                {
                   
                    var telparIlk = model.tel.Replace("(", "");
                    var telParSon = telparIlk.Replace(")", "");
                    var telTire = telParSon.Replace("-", "");
                   // _smsService.SmsKaydet(model);

                    var telefon = telTire;
                    numaralar.Add(telefon);
                }

                if (tip == "katmaliki")
                {
                    var katMalikiListe = _kisilerService.KatmalikiListesi();
                    foreach (var item in katMalikiListe)
                    {
                        var tel = _kisilerService.KisiGetirId(item.kisi_id).tel1;
                        model.kisi_id = item.kisi_id;
                        model.tel = tel;
                        //_smsService.SmsKaydet(model);

                        var telparIlk = tel.Replace("(", "");
                        var telParSon = telparIlk.Replace(")", "");
                        var telTire = telParSon.Replace("-", "");

                        var telefon = telTire;
                        numaralar.Add(telefon);
                    }


                }

                if (tip == "kiraci")
                {
                    var kiraciListe = _kisilerService.KiraciListesi();
                    foreach (var item in kiraciListe)
                    {
                        var tel = _kisilerService.KisiGetirId(item.kisi_id).tel1;
                        model.kisi_id = item.kisi_id;
                        model.tel = tel;
                        //_smsService.SmsKaydet(model);

                        var telparIlk = tel.Replace("(", "");
                        var telParSon = telparIlk.Replace(")", "");
                        var telTire = telParSon.Replace("-", "");

                        var telefon = telTire;
                        numaralar.Add(telefon);
                    }

                  
                }

                sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, numaralar);
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
          
            return Json(true, JsonRequestBehavior.AllowGet);
        }   

        public ActionResult BorcSms()
        {
            return View();
        }

        public ActionResult BorcSmsKaydet(sms model)
        {
            model.tarih = DateTime.Now.ToString("yyyy-MM-dd");
            sms.gettoken("5332563356", "7304707");
            string mesaj = model.icerik;
            string gonMesaj = Fonksiyonlar.tr2en(mesaj);
            string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            try
            {
                var borcluListesi = _borclandirmaService.TumBorcListesi();

                foreach (var item in borcluListesi)
                {
                    var tel = _kisilerService.KisiGetirId(item.kisi_id).tel1;
                    model.kisi_id = item.kisi_id;
                    model.tel = tel;
                   // _smsService.SmsKaydet(model);

                    var telparIlk = tel.Replace("(", "");
                    var telParSon = telparIlk.Replace(")", "");
                    var telTire = telParSon.Replace("-", "");

                    var telefon = telTire;
                    numaralar.Add(telefon);
                }

                sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, numaralar);
            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}