using MYARCH.CORE.Entities;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using SahinSms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MYARCH.WEB.Controllers
{
    public class KisilerController : AdminController
    {

        private readonly IKisilerService _kisilerService;

        private readonly IUnitofWork _uow;

        public KisilerController(IUnitofWork uow, IKisilerService kisilerService)
  : base(uow)
        {
            _uow = uow;
            _kisilerService = kisilerService;


        }

        // GET: Kisiler
        public ActionResult Index()
        {

            var kisiler = _kisilerService.KisiGetir();
            return View(kisiler);
        }


        public ActionResult Pasif()
        {
            var pasifKisiler = _kisilerService.PasifKisiListesi();
            return View(pasifKisiler);
        }


        public  ActionResult Kaydet()
        {
            return View();
        }


        public ActionResult KaydetIslem(kisiler model)
        {


            //string mesaj = "Kullanıcı adı";
            //string gonMesaj = Fonksiyonlar.tr2en(mesaj);
            //string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, Numaralar);

            Random rastgele = new Random();
            int ascii = rastgele.Next(65, 91);
            char karakter = Convert.ToChar(ascii);
            int sayi = rastgele.Next(10000, 999999);

            model.sifre = sayi.ToString() + karakter;
            model.rutbe = "user";
            model.kullanici_adi = model.tel1.Substring(6);
            model.sifre = Fonksiyonlar.tr2en(model.sifre);

            var kaydet = _kisilerService.KisiKaydet(model);

            if (kaydet == true)
            {
                //kullanıcı kayıt oldugunda sms gönderimi.Daha sonra bagımsız bolumlere ilişkilendirme olarak değiştirildi
                
                //SahinHaberlesme sms = new SahinHaberlesme();
                //List<string> numaralar = new List<string>();
                //var telparIlk = model.tel1.Replace("(","");
                //var telParSon = telparIlk.Replace(")", "");
                //var telTire = telParSon.Replace("-", "");
              
                //var telefon = telTire;
                //numaralar.Add(telefon);
             
                //string mesaj = "Kullanıcı adınız :" + model.kullanici_adi + " " + "Şifreniz :" + model.sifre; 
                //string gonMesaj = Fonksiyonlar.tr2en(mesaj);
                //string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, numaralar);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            } 
        }

        [Route("~/KisiDuzenle/{id:int}")]
        public ActionResult KisiDuzenle(int id)
        {
            var gelenKisi = _kisilerService.KisiGetirId(id);
            return View(gelenKisi);
        }

        public ActionResult KisiGuncelleIslem(kisiler model)
        {
            var guncelleSonuc = _kisilerService.KisiGuncelle(model);
            if (guncelleSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Kisi(int id,string paraBirimi)
        {
            var kisilerTlBakiye = _kisilerService.kisiTlBakiye(id);
            var kisilerDolarBakiye = _kisilerService.kisiDolarBakiye(id);

            var kisiler = _kisilerService.KisiGetirId(id);
            dynamic mymodel = new ExpandoObject();
            mymodel.kisiler = kisiler;
            mymodel.kisilerTlBakiye = kisilerTlBakiye;
            mymodel.kisilerDolarBakiye = kisilerDolarBakiye;

            if (paraBirimi == "USD")
            {
                var kisiBorclariDolar = _kisilerService.KisiBorclariGetirDolar(id);
                if (kisiBorclariDolar != null)
                {
                    return Json(kisiBorclariDolar, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            else
            {
                var kisiBorclari = _kisilerService.KisiBorclariGetir(id);
                mymodel.kisiBorclari = kisiBorclari;
                return View(mymodel);
            }
        }


        public ActionResult KatmalikiDetay(int katmaliki_id)
        {
            var veri = _kisilerService.KatmalikiDetay(katmaliki_id);
            if (veri != null)
            {
                return Json(veri, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KiraciDetay(int kiraci_id)
        {
            var veri = _kisilerService.KiraciDetay(kiraci_id);
            if (veri != null)
            {
                return Json(veri, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KisiSil(int id)
        {
            var sil = _kisilerService.KisiSil(id);
            if (sil == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Kullanicilar(int id)
        {
            var sil = _kisilerService.KisiSil(id);
            if (sil == true)
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
