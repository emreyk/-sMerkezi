using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Services;
using System.Dynamic;
using MYARCH.DATA.Context;

namespace MYARCH.WEB.Controllers
{
    public class TanimlarController : AdminController
    {
        private readonly IBorcTipleri _borcTipleri;
        private readonly IBlokService _blokService;
        private readonly ITipService _tipService;
        private readonly IPersonelService _personelService;
        private readonly IAnasayacService _anaSayacService;
        private readonly ISayacTipleriService _sayacTipleriService;
        private readonly IAnaSayacOrtakDagitimService _sayacOrtakAlanService;
        private readonly IBagimsizSayacService _bagimsizSayacService;
        private readonly IBagimsizBolumlerService _bagimsizSBolumlercService;
        private readonly IKasaService _kasaService;
        private readonly IBankaService _bankaService;
        private readonly IFirmaService _firmaService;
        private readonly IUnitofWork _uow;

        public TanimlarController(IUnitofWork uow, IBlokService blokService, ITipService tipService, IPersonelService personelService, IAnasayacService anaSayacService, ISayacTipleriService sayacTipleriService,
           IAnaSayacOrtakDagitimService sayacOrtakAlanService, IBagimsizSayacService bagimsizSayacService, IBagimsizBolumlerService bagimsizSBolumlercService, IKasaService kasaService, IBankaService bankaService,
           IFirmaService firmaService, IBorcTipleri borcTipleri)
        : base(uow)
        {
            _uow = uow;
            _blokService = blokService;
            _tipService = tipService;
            _personelService = personelService;
            _anaSayacService = anaSayacService;
            _sayacTipleriService = sayacTipleriService;
            _sayacOrtakAlanService = sayacOrtakAlanService;
            _bagimsizSayacService = bagimsizSayacService;
            _bagimsizSBolumlercService = bagimsizSBolumlercService;
            _kasaService = kasaService;
            _bankaService = bankaService;
            _firmaService = firmaService;
            _borcTipleri = borcTipleri;
        }

        // GET: Manager
        public ActionResult Index()
        {

            return View();
        }

   
        public ActionResult Bloklar()
        {
            var bloklar = _blokService.BlokGetir();
            return View(bloklar);
        }

        public ActionResult Firma()
        {
            var firmalar = _firmaService.FirmaListesi();
            return View(firmalar);
        }

        public ActionResult FirmaKaydet()
        {
            return View();
        }

        public ActionResult FirmaKaydetIslem(firmalar model)
        {
            var kayitSonuc = _firmaService.FirmaKaydet(model);

            if (kayitSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FirmaDuzenle(firmalar model)
        {
            var firmaGelen = _firmaService.FirmaGetir(model);
            return View(firmaGelen);
        }

        public ActionResult FirmaGuncelleIslem(firmalar model)
        {
            var guncellemeSonucu = _firmaService.FirmaGuncelle(model);

            if (guncellemeSonucu == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FirmaSil(int id)
        {
            var firmaSilmeSonucu = _firmaService.FirmaSil(id);

            if (firmaSilmeSonucu == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DaireEkle()
        {
            return View();
        }

        public ActionResult BorclandirmaTuru()
        {
            var firmalar = _borcTipleri.BorcTipleriGetir();
            return View(firmalar);
        }

        public ActionResult BorclandirmaTuruKaydet(borc_tipleri model)
        {
            var tipKaydetSonuc = _borcTipleri.BorcTipleriKaydet(model);

            if (tipKaydetSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BorclandirmaTuruSil(borc_tipleri model)
        {
            var silSonuc = _borcTipleri.borcTipiSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BlokKaydet(blok model)
        {
            MyArchContext context = new MyArchContext();

           
            var blokKaydetSonuc = _blokService.BlokKaydet(model);

            if (blokKaydetSonuc == true)
            {
                var sonBlokId = context.blok.OrderByDescending(x => x.id).FirstOrDefault();


                bagimsiz_bolumler bagimsizModel = new bagimsiz_bolumler();
                bagimsizModel.blok_id = sonBlokId.id;
                bagimsizModel.blok_adi = model.blok_adi;
                bagimsizModel.daire_numarasi = model.daire_sayisi;
                var bagimsizBolumBlokKaydet = _bagimsizSBolumlercService.BagimsizBolumBlokKaydet(bagimsizModel);
            }

            if (blokKaydetSonuc == true )
            {

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BlokSil(blok model)
        {
            var silSonuc = _blokService.BlokSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult BlokGuncelle(EBlokDTO model)
        {
            bool durum = _blokService.BlokGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BlokDaireleri(int id)
        {
            EBlokDTO daireSayisi = _blokService.BlokDaireleri(id);

            if (daireSayisi.daire_sayisi != null)
            {
                return Json(daireSayisi.daire_sayisi, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Tipler()
        {
            ViewBag.aidatParaBirimi = _borcTipleri.ParaBirimiAidat();
            var tipler = _tipService.TipGetir();
            return View(tipler);
        }


        public ActionResult TipDuzenle(int id)
        {
            var tipId = _tipService.TipGetirId(id);
            return View();
        }

        public ActionResult TipKaydet(tipler model)
        {
            var tipKaydetSonuc = _tipService.TipKaydet(model);



            if (tipKaydetSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AidatTahsilatKontrol()
        {
            bool durum = _tipService.AidatTahsilatKontrol();

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TipGuncelle(ETiplerDTO model)
        {
            bool durum = _tipService.TipGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TipSil(tipler model)
        {
            var silSonuc = _tipService.TipSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Personel()
        {
            var tipler = _personelService.PersonelGetir();
            return View(tipler);
        }


        public ActionResult PersonelKaydet()
        {
            return View();
        }

        public ActionResult PersonelKaydetIslem(personel model)
        {
            var tipKaydetSonuc = _personelService.PersonelKaydet(model);

            if (tipKaydetSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult PersonelGuncelle(int id)
        {
            var personel = _personelService.PersonelBilgileriGetir(id);
            return View(personel);
        }


        public ActionResult Sayac()
        {
            var sayaclar = _anaSayacService.AnaSayacGetir();
            var bagimsizSayaclar = _bagimsizSayacService.BagimsizSayacGetir();
            dynamic mymodel = new ExpandoObject();
            mymodel.sayaclar = sayaclar;
            mymodel.bagimsizSayac = bagimsizSayaclar;
            return View(mymodel);
        }

        public ActionResult SayacKaydet()
        {
            var sayacTipleri = _anaSayacService.SayacTipi();
            var sayacOrtakAlan = _anaSayacService.OrtakAlan();
            ViewBag.SayacBilgiler = sayacTipleri;
            ViewBag.sayacOrtakAlan = sayacOrtakAlan;
            return View();
        }

        public ActionResult SayacKaydetIslem(anasayac model)
        {
            var sayacKaydetSonuc = _anaSayacService.SayacKaydet(model);

            if (sayacKaydetSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SayacGuncelle(int id)
        {
            var sayacTipleri = _anaSayacService.SayacTipi();
            var sayacOrtakAlan = _anaSayacService.OrtakAlan();
            ViewBag.SayacBilgiler = sayacTipleri;
            ViewBag.sayacOrtakAlan = sayacOrtakAlan;

            var sayacBilgileri = _anaSayacService.SayacBilgileriGetir(id);
            return View(sayacBilgileri);
        }



        public ActionResult SayacSilKontrol(int id)
        {
            var silSonuc = _anaSayacService.SayacSilKontrol(id);

            if (silSonuc == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult SayacSil(anasayac model)
        {
            var silSonuc = _anaSayacService.AnaSayacSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SayacGuncelleIslem(EAnasayacDTO model)
        {
            bool durum = _anaSayacService.AnaSayacGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SayacTipleri()
        {
            var sayacTipleri = _sayacTipleriService.SayacTipleri();
            return View(sayacTipleri);
        }

        public ActionResult SayacTipleriKaydet(sayac_tipleri model)
        {
            var sayacTipiKaydetSonuc = _sayacTipleriService.SayacTipiKaydet(model);

            if (sayacTipiKaydetSonuc == true)
            {

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SayacTipSil(sayac_tipleri model)
        {
            var silSonuc = _sayacTipleriService.SayacTipiSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SayacTipGuncelle(sayac_tipleri model)
        {
            bool durum = _sayacTipleriService.SayacTipiGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OrtakAlanDagitim()
        {
            var ortakAlan = _sayacOrtakAlanService.OrtakAlanGetir();
            return View(ortakAlan);
        }

        public ActionResult OrtakAlanKaydet(anasayac_ortak_dagitim model)
        {
            var sonuc = _sayacOrtakAlanService.OrtakAlanKaydet(model);

            if (sonuc == true)
            {

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult OrtakAlanSil(anasayac_ortak_dagitim model)
        {
            var silSonuc = _sayacOrtakAlanService.OrtakAlanSil(model);

            if (silSonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult OrtakAlanGuncelle(anasayac_ortak_dagitim model)
        {
            bool durum = _sayacOrtakAlanService.OrtakAlanGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult BagimsizSayacEkle()
        {
            ViewBag.Bloklar = _blokService.BlokGetir();
            ViewBag.AnaSayaclar = _anaSayacService.AnaSayacGetir();
            return View();
        }

        public ActionResult BagimsizSayacKaydetIslem(bagimsiz_bolum_sayaclari model)
        {
            var sonuc = _bagimsizSayacService.BagimsizSayacKaydet(model);

            if (sonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Kasa()
        {

            var kasalar = _kasaService.KasaGetir();
            return View(kasalar);
        }

        public ActionResult KasaKaydet()
        {

            return View();
        }

        public ActionResult KasaHareket()
        {

            var kasaHareket = _kasaService.KasaHareket();
            return View(kasaHareket);
        }

        public ActionResult KasaKaydetIslem(kasa model)
        {
            Random rastgele = new Random();
            int sayi = rastgele.Next(10, 99);

            model.kasa_no = "KS" + "-" + sayi;
            model.durum = "True";
            bool sonuc = _kasaService.KasaKaydet(model);


            if (sonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult KasaGuncelle(int id)
        {
            var kasaBilgileri = _kasaService.KasaBilgileri(id);
            return View(kasaBilgileri);
        }

        public ActionResult KasaGuncelleIslem(kasa model)
        {
            bool durum = _kasaService.KasaGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Banka()
        {

            var kasalar = _bankaService.BankaGetir();
            return View(kasalar);
        }

        public ActionResult BankaKaydet()
        {

            return View();
        }

        public ActionResult BankaHareket()
        {

            var bankaHareket = _bankaService.BankaHareket();
            return View(bankaHareket);
        }

        public ActionResult BankaKaydetIslem(banka model)
        {
            Random rastgele = new Random();
            int sayi = rastgele.Next(10, 99);

            model.banka_no = "BK" + "-" + sayi;
            model.durum = "True";
            bool sonuc = _bankaService.BankaKaydet(model);


            if (sonuc == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BankaGuncelle(int id)
        {
            var bankaBilgileri = _bankaService.BankaBilgileri(id);
            return View(bankaBilgileri);
        }


        public ActionResult BankaGuncelleIslem(banka model)
        {
            bool durum = _bankaService.BankaGuncelle(model);

            if (durum == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PesonelSil(personel model)
        {
            var silSonuc = _personelService.PersonelSil(model);

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