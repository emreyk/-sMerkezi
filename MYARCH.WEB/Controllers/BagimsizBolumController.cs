using MYARCH.CORE.Entities;
using MYARCH.DATA.Context;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
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
    public class BagimsizBolumController : AdminController
    {
        private readonly IBorclandirmaService _borclandirService;
        private readonly IDosyaService _dosyaServis;
        private readonly IBagimsizBolumlerService _bagimsizBolumService;
        private readonly IKisilerService _kisilerService;
        private readonly ITipService _tipService;
        private readonly IBlokService _blokService;
        private readonly IBorcTipleri _borcTipleri;
        private readonly IUnitofWork _uow;

        public BagimsizBolumController(IUnitofWork uow, IBagimsizBolumlerService bagimsizBolumService, IBlokService blokService, IKisilerService kisilerService, ITipService tipService, IBorcTipleri borcTipleri, IDosyaService dosyaServis, IBorclandirmaService borclandirService)
  : base(uow)
        {
            _uow = uow;
            _bagimsizBolumService = bagimsizBolumService;
            _blokService = blokService;
            _kisilerService = kisilerService;
            _tipService = tipService;
            _borcTipleri = borcTipleri;
            _dosyaServis = dosyaServis;
            _borclandirService = borclandirService;

        }

        public ActionResult Index()
        {
            var bagimsizBolumler = _bagimsizBolumService.BagimsizBolumler();
            return View(bagimsizBolumler);
        }


        public ActionResult Detay(int id, string paraBirimi)
        {
            
            ViewBag.aidatParaBirimi = _borcTipleri.ParaBirimiAidat();

            var kisiler = _kisilerService.KisiListesiModal();
            dynamic mymodel = new ExpandoObject();
            var detay = _bagimsizBolumService.BagimsizBolumBilgileriGetir(id);
            mymodel.detay = detay;
            mymodel.kisiler = kisiler;

            
      
            EBakiyeDTO katMalikiBakiye;
            using (var context = new MyArchContext())
            {

                katMalikiBakiye = context.Database.SqlQuery<EBakiyeDTO>("SELECT SUM(borc-alacak) as bakiye FROM hesap_hareket WHERE bagimsiz_id = '" + id + "' ").SingleOrDefault();

            }

            if (katMalikiBakiye == null)
            {
                ViewBag.Bakiye = "";
            }

            else
            {
                ViewBag.Bakiye = katMalikiBakiye.bakiye;
            }


            if (paraBirimi == "TL" || paraBirimi == null)
            {
                var detayKisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id, paraBirimi);
                mymodel.detayKisiler = detayKisiler;
                return View(mymodel);

            }
            else
            {
                var detayKisiler = _bagimsizBolumService.BagimsizBolumlerKisiler(id, paraBirimi);
                mymodel.detayKisiler = detayKisiler;
                return Json(mymodel.detayKisiler, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult CikisTarihiGuncelle(bagimsiz_bolum_kisiler model)
        {

            //çıkarılıcak kişinin borcu varsa
            if (_bagimsizBolumService.BorcKontrol(model.kisi_id, model.bagimsiz_id) == false)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            else
            {
                bool durum = _bagimsizBolumService.CikisTarihiGuncelle(model);

                if (durum == true)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

        }


        public ActionResult BagimsizAdresNoKontrol(string adresDaireNo, int id)
        {
            bool kontrol = _bagimsizBolumService.BagimsizAdresNoKontrol(adresDaireNo, id);

            if (kontrol == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DetayDuzenle(int id)
        {
            var detay = _bagimsizBolumService.BagimsizBolumBilgileriGetir(id);
            ViewBag.aidatParaBirimi = _borcTipleri.ParaBirimiAidat();


            if (detay.daire_adres_no != null)
            {
                ViewBag.DaireDetay = true;
            }
            else
            {
                ViewBag.DaireDetay = false;
            }

            ViewBag.Tipler = _tipService.TipGetirYıl();
            ViewBag.TipAdi = detay.tip;
            ViewBag.Bloklar = _blokService.BlokGetir();
            return View(detay);
        }


        public ActionResult Guncelle(bagimsiz_bolumler model)
        {
            bool durum = _bagimsizBolumService.BagimsizBolumGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DaireEkle()
        {
            ViewBag.Tipler = _tipService.TipGetirYıl();
            ViewBag.aidatParaBirimi = _borcTipleri.ParaBirimiAidat();
            return View();
        }

        public ActionResult DaireKaydet(bagimsiz_bolumler model)
        {
            bool durum = _bagimsizBolumService.BagimsizBolumKaydet(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    
        public ActionResult OtoparkTanimla()
        {
            int otoparkId;
            using (var context = new MyArchContext())
            {
                //bu sorgu otopark id sini getirir
                 otoparkId = context.Database.SqlQuery<int>("SELECT id FROM bagimsiz_bolumler WHERE daire_numarasi IS NULL and elektrik_abone_no IS NULL and petek_boyu IS NULL").FirstOrDefault();
            }
            
            //otoparka özel
            var detay = _bagimsizBolumService.BagimsizBolumBilgileriGetir(otoparkId);
            ViewBag.Tipler = _tipService.TipGetirYıl();
            return View(detay);
        }

        public ActionResult OtoparakTanimlaIslem(bagimsiz_bolumler model)
        {
            int otoparkId;
            var katsayisi = Convert.ToString(model.daire_katsayisi);
            var sonKatsayisi = katsayisi.Replace(',', '.');
            using (var context = new MyArchContext())
            {
                //bu sorgu otopark id sini getirir
                otoparkId = context.Database.SqlQuery<int>("SELECT id FROM bagimsiz_bolumler WHERE daire_numarasi IS NULL and elektrik_abone_no IS NULL and petek_boyu IS NULL").FirstOrDefault();
            }

            model.id = otoparkId;

            using (var context = new MyArchContext())
            {
                //bagimsiz_bolum_kisilerde daire_katsayisini guncelle
                otoparkId = context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolum_kisiler SET daire_katsayisi = '"+sonKatsayisi+"' WHERE bagimsiz_id = '"+model.id+"'");
            }

          

            bool durum = _bagimsizBolumService.BagimsizBolumGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KisiKaydet(bagimsiz_bolum_kisiler model, string isim, int bagimsiz_id, int? katmaliki_id, int? kiraci_id)
        {
            bool durum = _bagimsizBolumService.BagimsizBolumKisilerKaydet(model, isim, bagimsiz_id, katmaliki_id, kiraci_id);

            var kisiBilgileri = _kisilerService.KisiGetirId(model.kisi_id);

            //if (_kisilerService.KisiKayitlimi(kisiBilgileri.id) == true)
            //{
            //    //sms gonder
            //    SahinHaberlesme sms = new SahinHaberlesme();
            //    List<string> numaralar = new List<string>();
            //    var telparIlk = kisiBilgileri.tel1.Replace("(", "");
            //    var telParSon = telparIlk.Replace(")", "");
            //    var telTire = telParSon.Replace("-", "");

            //    var telefon = telTire;
            //    numaralar.Add(telefon);
        
            //    string mesaj = " BİYÖNSİS sistemine giriş bilgileriniz  Kullanıcı adınız :" + kisiBilgileri.kullanici_adi + " " + "Şifreniz :" + kisiBilgileri.sifre;
            //    string gonMesaj = Fonksiyonlar.tr2en(mesaj);
            //    string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, numaralar);


            //}

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult KatMalikiKontrol(int bagimsiz_id, string tip)
        {
            bool katMalikiVarmi = _bagimsizBolumService.KatMalikiKontrol(bagimsiz_id, tip);

            if (katMalikiVarmi == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult KiraciKontrol(int bagimsiz_id, string tip)
        {
            bool kiraciVarmi = _bagimsizBolumService.KiraciKontrol(bagimsiz_id, tip);
            bool katMalikiKontrol = _bagimsizBolumService.KatMalikiKontrol(bagimsiz_id, tip);

            //katmaliki kendi yerinde ise kiracı eklenemez

            if (katMalikiKontrol == true)
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            if (kiraciVarmi == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BagimsizDetayKontrol(int bagimsiz_id)
        {
            bool kontrol = _bagimsizBolumService.BagimsızBolumDetayKontrol(bagimsiz_id);

            if (kontrol == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult TipeGoreAidat(int id)
        {

            var tipModel = _tipService.TipGetirId(id);

            if (tipModel != null)
            {
                return Json(tipModel.aidat_tutar, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult DosyaKaydet(dosyalar model)
        {


            var durum = true;
            if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase httpPostedFile = Request.Files[file];
                    string dosyaYolu = "/Uploads/" + httpPostedFile.FileName;

                    string contentTipi = httpPostedFile.ContentType;
                    string tip = contentTipi.Split('/')[1];
                    model.dosya_tipi = tip;
                    model.dosya_adi = httpPostedFile.FileName;
                    try
                    {
                        Request.Files[file].SaveAs(Server.MapPath(dosyaYolu));
                        model.dosya_yolu = dosyaYolu;


                        if (_dosyaServis.DosyaKaydet(model) == true)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return HttpNotFound();

                        }

                    }
                    catch (Exception)
                    {
                        durum = false;
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");

        }


        public ActionResult BagimsizKisiSil(hesap_hareket model, string tip)
        {
            // tahsilat ve borc varsa uyarı ver.bagımsız bolum kisilerdeki silme işlemi için yapıldı
            if (_borclandirService.TahsilatKontrolIdKisi(model.kisi_id) == true)
            {
                return Json("tahsilatvar", JsonRequestBehavior.AllowGet);
            }

            if (_borclandirService.BorcKontrolIdKisi(model.kisi_id) == true)
            {
                return Json("borcvar", JsonRequestBehavior.AllowGet);
            }


            var silSonuc = _bagimsizBolumService.BagimsizKisiSil(model, tip);
            if (silSonuc == true)
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
