using MYARCH.CORE.Entities;
using MYARCH.DATA.Context;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Services
{
    public class TopluBorclandirService : ITopluBorclandirService
    {
        private readonly IGenericRepository<toplu_borclandir> _topluBorclandirRepository;
        private readonly IGenericRepository<bagimsiz_bolum_kisiler> _bagimsizBolumlerKisiRepository;
        private readonly IGenericRepository<bagimsiz_bolumler> _bagimsizBolumlerRepository;
        private readonly IGenericRepository<hesap_hareket> _hesapHareketRepository;
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IGenericRepository<tipler> _tiplerRepository;
        private readonly IGenericRepository<yakit> _yakitRepository;

        private readonly IUnitofWork _uow;

        public TopluBorclandirService(UnitofWork uow)
        {
            _uow = uow;
            _topluBorclandirRepository = _uow.GetRepository<toplu_borclandir>();
            _bagimsizBolumlerKisiRepository = _uow.GetRepository<bagimsiz_bolum_kisiler>();
            _hesapHareketRepository = _uow.GetRepository<hesap_hareket>();
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _tiplerRepository = _uow.GetRepository<tipler>();
            _bagimsizBolumlerRepository = _uow.GetRepository<bagimsiz_bolumler>();
            _yakitRepository = _uow.GetRepository<yakit>();
        }

        public bool TopluBorclandirKaydet(toplu_borclandir model, hesap_hareket hesapModel, string tip)
        {
            //model.tarih = DateTime.Now;
            int ayTopluBorclandir = model.tarih.Month;
            int yilTopluBorclandir = hesapModel.tarih.Year;
            int gunTopluBorclandir = hesapModel.tarih.Day;


            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 999999);

            var refno = "TP" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;


            try
            {
                model.refno = refno;
                model.tip = tip;
                double borc = 0;
                model.gun = gunTopluBorclandir;
                model.ay = ayTopluBorclandir;
                model.yil = yilTopluBorclandir;
                model.tutar = hesapModel.borc;

                //toplu borclandır kaydet
                _topluBorclandirRepository.Insert(model);
                if (_uow.SaveChanges() > 0)
                {
                    if (tip == "Kiracı" && model.dagitim_sekli == "Bağımsız Bölümlere Eşit")
                    {
                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();

                        int vadeliBorcKisiId = 0;

                        //hesap harekette böyle bir kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.borclandirma_turu == hesapModel.borclandirma_turu).FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil).FirstOrDefault().kisi_id;
                        }

                        var aktifKiracilar = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kiracı" && x.aktif == "True" ).ToList();

                        hesap_hareket hesapHareketModel = new hesap_hareket();
                        kisiler kisi = new kisiler();

                        foreach (var item in aktifKiracilar)
                        {
                            
                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;
                            hesapHareketModel.borc = hesapModel.borc*item.daire_katsayisi;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = hesapModel.borc*item.daire_katsayisi - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;


                            hesapHareketModel.para_birimi = hesapModel.para_birimi;
                            //if (hesapModel.borclandirma_turu == "Akaryakıt")
                            //{
                            //    hesapHareketModel.para_birimi = "USD";
                            //}
                            //else
                            //{
                            //    hesapHareketModel.para_birimi = "TL";
                            //}


                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();

                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                            //euro ise
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }

                        }

                        return true;
                    }

                    if (tip == "Kat maliki" && model.dagitim_sekli == "Bağımsız Bölümlere Eşit")
                    {

                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();

                        int vadeliBorcKisiId = 0;
                        //hesap harekette kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault().kisi_id;
                        }

                        var aktifKatMaliki = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kat maliki" && x.aktif == "True" ).ToList();
                        hesap_hareket hesapHareketModel = new hesap_hareket();

                        foreach (var item in aktifKatMaliki)
                        {

                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;
                            hesapHareketModel.borc = hesapModel.borc*item.daire_katsayisi;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = hesapModel.borc*item.daire_katsayisi - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;
                            hesapHareketModel.para_birimi = hesapModel.para_birimi;


                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();

                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }

                            //euro ise
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                        }

                    }

                    if (tip == "Kiracı" && model.dagitim_sekli == "Bağımsız Bölümlere Göre")
                    {
                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();
                        int vadeliBorcKisiId = 0;
                        //hesap harekette kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault().kisi_id;
                        }

                        var aktifKiracilar = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kiracı" && x.aktif == "True" ).ToList();

                        hesap_hareket hesapHareketModel = new hesap_hareket();
                        kisiler kisi = new kisiler();

                        foreach (var item in aktifKiracilar)
                        {

                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;


                            //kisi birden fazla yerde kiracı olabilecegi icin bagimsiz_id ye gorede sorgulama yapıldı
                            //var gelenTip = _bagimsizBolumlerRepository.GetAll().Where(x =>
                            //x.kiraci_id == item.kisi_id && x.id == item.bagimsiz_id || (x.katmaliki_id != null && x.kiraci_id == null)
                            //).FirstOrDefault().tip;


                            var gelenTip = _bagimsizBolumlerRepository.GetAll().Where(x => x.id == item.bagimsiz_id).First().tip;

                            double tipFiyati = _tiplerRepository.GetAll().Where(x => x.bagimsiz_tip == gelenTip && x.yil == DateTime.Now.Year.ToString()).FirstOrDefault().aidat_tutar;

                            hesapHareketModel.borc = tipFiyati*item.daire_katsayisi;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = tipFiyati*item.daire_katsayisi - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;
                            hesapHareketModel.para_birimi = hesapModel.para_birimi;

                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();

                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                        }

                        return true;
                    }

                    if (tip == "Kat maliki" && model.dagitim_sekli == "Bağımsız Bölümlere Göre")
                    {
                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();
                        int vadeliBorcKisiId = 0;
                        //hesap harekette kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault().kisi_id;
                        }

                        var aktifKiracilar = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kat maliki" && x.aktif == "True" ).ToList();
                        hesap_hareket hesapHareketModel = new hesap_hareket();
                        kisiler kisi = new kisiler();

                        foreach (var item in aktifKiracilar)
                        {

                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;


                            //kisi birden fazla yerde kiracı olabilecegi icin  bagimsiz_id ye gorede sorgulama yapıldı
                            var gelenTip = _bagimsizBolumlerRepository.GetAll().Where(x => x.katmaliki_id == item.kisi_id && x.id == item.bagimsiz_id).FirstOrDefault().tip;
                            double tipFiyati = _tiplerRepository.GetAll().Where(x => x.bagimsiz_tip == gelenTip && x.yil == DateTime.Now.Year.ToString()).FirstOrDefault().aidat_tutar;

                            hesapHareketModel.borc = tipFiyati * item.daire_katsayisi;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = tipFiyati * item.daire_katsayisi - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;
                            hesapHareketModel.para_birimi = hesapModel.para_birimi;

                            if (hesapModel.borclandirma_turu == "Akaryakıt")
                            {
                                hesapHareketModel.para_birimi = "USD";
                            }
                            else
                            {
                                hesapHareketModel.para_birimi = "TL";
                            }

                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();

                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }

                        }

                        return true;
                    }



                    //akaryakıt borçlandırması
                    if (tip == "Kiracı" && model.borclandirma_turu == "Akaryakıt")
                    {
                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();
                        int vadeliBorcKisiId = 0;
                        //hesap harekette kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault().kisi_id;
                        }

                        var aktifKiracilar = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kiracı" && x.aktif == "True"
                    ).ToList();

                        hesap_hareket hesapHareketModel = new hesap_hareket();
                        kisiler kisi = new kisiler();

                        foreach (var item in aktifKiracilar)
                        {

                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;


                            //yakit borcununu hesapla
                            double toplamPetekBoyutu = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_petek_uzunluk;
                            double toplamTutar = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_tutar;
                            var petekBoyu = _bagimsizBolumlerRepository.GetAll().Where(x => x.id == item.bagimsiz_id).FirstOrDefault().petek_boyu;
                            double yakitBorc = (Convert.ToDouble(petekBoyu) / toplamPetekBoyutu) * toplamTutar;

                            double ikiBasamakYakitBorc = Math.Round(yakitBorc, 2);

                            hesapHareketModel.borc = ikiBasamakYakitBorc;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = ikiBasamakYakitBorc - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;
                            hesapHareketModel.para_birimi = hesapModel.para_birimi;
                            hesapHareketModel.donem = hesapModel.donem;

                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();


                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }

                        }

                        return true;
                    }

                    if (tip == "Kat maliki" && model.borclandirma_turu == "Akaryakıt")
                    {
                        string ay = hesapModel.tarih.Month.ToString();
                        string yil = hesapModel.tarih.Year.ToString();
                        int vadeliBorcKisiId = 0;
                        //hesap harekette kayıt varsa
                        if (_hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault() != null)
                        {
                            vadeliBorcKisiId = _hesapHareketRepository.GetAll().Where(x => x.ay == ay && x.yil == yil && x.pesin_tahsilat == "X").FirstOrDefault().kisi_id;
                        }

                        var aktifKiracilar = _bagimsizBolumlerKisiRepository.GetAll().Where(x => x.tip == "Kat maliki" && x.aktif == "True").ToList();

                        hesap_hareket hesapHareketModel = new hesap_hareket();
                        kisiler kisi = new kisiler();

                        foreach (var item in aktifKiracilar)
                        {


                            hesapHareketModel.aciklama = hesapModel.aciklama;
                            hesapHareketModel.saat = DateTime.Now.ToShortTimeString();
                            hesapHareketModel.tarih = hesapModel.tarih;
                            hesapHareketModel.gun = hesapModel.tarih.Day.ToString();
                            hesapHareketModel.ay = hesapModel.tarih.Month.ToString();
                            hesapHareketModel.yil = hesapModel.tarih.Year.ToString();
                            hesapHareketModel.sonodeme_tarihi = hesapModel.sonodeme_tarihi;


                            //yakit borcununu hesapla
                            double toplamPetekBoyutu = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_petek_uzunluk;
                            double toplamTutar = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_tutar;
                            var petekBoyu = _bagimsizBolumlerRepository.GetAll().Where(x => x.id == item.bagimsiz_id).FirstOrDefault().petek_boyu;
                            double yakitBorc = (Convert.ToDouble(petekBoyu) / toplamPetekBoyutu) * toplamTutar;
                            double ikiBasamakYakitBorc = Math.Round(yakitBorc, 2);


                            hesapHareketModel.borc = ikiBasamakYakitBorc;
                            hesapHareketModel.kisi_id = hesapModel.kisi_id;
                            hesapHareketModel.islem_turu = "borç dekontu";
                            hesapHareketModel.tahsilat_durumu = "ödenmedi";
                            hesapHareketModel.refno = refno;
                            hesapHareketModel.bagimsiz_id = hesapModel.bagimsiz_id;
                            hesapHareketModel.bakiye = ikiBasamakYakitBorc - hesapModel.alacak;
                            hesapHareketModel.borclandirma_turu = hesapModel.borclandirma_turu;
                            hesapHareketModel.bagimsiz_id = item.bagimsiz_id;
                            hesapHareketModel.kisi_id = item.kisi_id;
                            hesapHareketModel.islem_tarihi = DateTime.Now;
                            hesapHareketModel.hesap_adi = _kisilerRepository.GetAll().Where(x => x.id == item.kisi_id).FirstOrDefault().isim;
                            hesapHareketModel.para_birimi = hesapModel.para_birimi;
                            hesapHareketModel.donem = hesapModel.donem;

                            //hesap hareket kaydet
                            _hesapHareketRepository.Insert(hesapHareketModel);
                            _uow.SaveChanges();



                            //kisi borc guncelle
                            kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);
                            if (hesapModel.para_birimi == "TL")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);

                                    _uow.SaveChanges();

                                }
                            }

                            if (hesapModel.para_birimi == "USD")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);


                                    borc = context.Database.SqlQuery<double>("SELECT  COALESCE (SUM(borc),0)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                                    kisiIdModel.borc_tl = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }
                            if (hesapModel.para_birimi == "EURO")
                            {
                                using (var context = new MyArchContext())
                                {

                                    //borc = context.Database.SqlQuery<double>("SELECT borc FROM kisiler WHERE id='" + item.kisi_id + "' ").FirstOrDefault();
                                    borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + item.kisi_id + "' and para_birimi='EURO' ").SingleOrDefault();
                                    kisiIdModel.borc_dolar = borc;
                                    _kisilerRepository.Update(kisiIdModel);
                                    _uow.SaveChanges();

                                }
                            }

                        }

                        return true;
                    }

                    return true;
                }

                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public List<TopluBorclandirDTO> TopluBorclandirmalar()
        {
            using (var context = new MyArchContext())
            {

                var topluBorcListesi = context.Database.SqlQuery<TopluBorclandirDTO>("SELECT toplu_borclandir.id,toplu_borclandir.refno,toplu_borclandir.tarih,toplu_borclandir.sonodeme_tarihi,toplu_borclandir.dagitim_sekli,toplu_borclandir.tip,hesap_hareket.borclandirma_turu FROM toplu_borclandir " +
                    " LEFT JOIN hesap_hareket  ON toplu_borclandir.refno = hesap_hareket.refno GROUP BY toplu_borclandir.refno,toplu_borclandir.id,toplu_borclandir.tarih,toplu_borclandir.sonodeme_tarihi,toplu_borclandir.dagitim_sekli,toplu_borclandir.tip,hesap_hareket.borclandirma_turu order by  toplu_borclandir.id desc").ToList();

                return topluBorcListesi;

            }
        }


        public bool TopluBorcSil(string refno, string borclandirmaTuru)
        {

            try
            {
                if (refno != null)
                {
                    using (var context = new MyArchContext())
                    {
                        var kisiIdleri = context.Database.SqlQuery<int>("SELECT kisi_id FROM hesap_hareket WHERE refno = '" + refno + "' GROUP BY kisi_id ").ToList();

                        //hesap hareket ve borc tablo sil
                        context.Database.ExecuteSqlCommand("DELETE FROM hesap_hareket where refno = '" + refno + "'");
                        context.Database.ExecuteSqlCommand("DELETE FROM toplu_borclandir where refno = '" + refno + "'");

                        //var kisiIdleri = context.Database.SqlQuery<int>("SELECT kisi_id FROM hesap_hareket  GROUP BY kisi_id ").ToList();

                        //var kisiIdleri = context.Database.SqlQuery<int>("SELECT kisi_id FROM hesap_hareket WHERE refno != '" + refno + "' GROUP BY kisi_id ").ToList();



                        //var paraBirimi = context.Database.SqlQuery<string>("SELECT para_birimi FROM borc_tipleri WHERE borclandirma_turu = '" + borclandirmaTuru + "'  ").FirstOrDefault();
                        //var hesapHareketKontrol = context.Database.SqlQuery<int>("SELECT kisi_id FROM hesap_hareket WHERE refno = '" + refno + "' GROUP BY kisi_id ").ToList();

                        //etkillen kisileri kisiler tablosunda guncelle

                        if (kisiIdleri.Count != 0)
                        {
                            foreach (var item in kisiIdleri)
                            {
                                //dolar
                                double? kalanKisiBorcUsd = context.Database.SqlQuery<double?>("SELECT COALESCE(SUM(borc), 0) as borc FROM hesap_hareket WHERE kisi_id = '" + item + "' and para_birimi = 'USD' ").SingleOrDefault();

                                double? kalanKisiAlacakUsd = context.Database.SqlQuery<double?>("SELECT COALESCE(SUM(alacak), 0) as alacak FROM hesap_hareket WHERE kisi_id = '" + item + "' and para_birimi = 'USD' ").SingleOrDefault();

                                var sonDegerBorcUsd = kalanKisiBorcUsd.ToString().Replace(",", ".");
                                var sonDegerAlacakUsd = kalanKisiAlacakUsd.ToString().Replace(",", ".");

                                context.Database.ExecuteSqlCommand("UPDATE kisiler SET borc_dolar='" + sonDegerBorcUsd + "' where id = '" + item + "'");
                                context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_dolar='" + sonDegerAlacakUsd + "' where id = '" + item + "'");

                                //tl
                                double kalanKisiBorcTl = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(borc), 0) as borc FROM hesap_hareket WHERE kisi_id = '" + item + "' and para_birimi = 'TL' ").SingleOrDefault();

                                double kalanKisiAlacakTl = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(alacak), 0) as alacak FROM hesap_hareket WHERE kisi_id = '" + item + "' and para_birimi = 'TL' ").SingleOrDefault();

                                var sonDegerBorcTl = kalanKisiBorcTl.ToString().Replace(",", ".");
                                var sonDegerAlacakTl = kalanKisiAlacakTl.ToString().Replace(",", ".");

                                context.Database.ExecuteSqlCommand("UPDATE kisiler SET borc_tl='" + sonDegerBorcTl + "' where id = '" + item + "'");
                                context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_tl='" + sonDegerAlacakTl + "' where id = '" + item + "'");
                            }
                        }
                        else
                        {

                            context.Database.ExecuteSqlCommand("UPDATE kisiler SET borc_dolar='0' ");
                            context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_dolar='0' ");

                            context.Database.ExecuteSqlCommand("UPDATE kisiler SET borc_tl='0' ");
                            context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_tl='0' ");


                        }

                    }

                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }


        }

        public toplu_borclandir TopluBorclandirRefno(string refno)
        {
            var topluBorcRefno = _topluBorclandirRepository.GetAll().Where(x => x.refno == refno).FirstOrDefault();
            return topluBorcRefno;

        }

        public bool AkarYakitKontrol()
        {
            var akaryakitListesi = _yakitRepository.GetAll().FirstOrDefault();

            if (akaryakitListesi == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AkaryakitBorclanidrmaSayisi()
        {
            using (var context = new MyArchContext())
            {

       
               int sayi  = context.Database.SqlQuery<int>("SELECT COUNT(*) as sayi FROM toplu_borclandir WHERE yil = '"+DateTime.Now.Year+"' and borclandirma_turu = 'Akaryakıt' ").SingleOrDefault();

                if (sayi>=2)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
