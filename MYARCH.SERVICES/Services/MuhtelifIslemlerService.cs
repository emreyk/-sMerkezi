using MYARCH.CORE.Entities;
using MYARCH.DATA.Context;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Services
{
    public class MuhtelifIslemlerService : IMuhtelifIslemlerService
    {

        private readonly IGenericRepository<muhtelif_islemler> _muhtelifIslemler;
        private readonly IGenericRepository<banka_hareket> _bankaHareketRepository;
        private readonly IGenericRepository<kasa_hareket> _kasaHareketRepository;
        private readonly IUnitofWork _uow;

        public MuhtelifIslemlerService(UnitofWork uow)
        {
            _uow = uow;
            _muhtelifIslemler = _uow.GetRepository<muhtelif_islemler>();
            _bankaHareketRepository = _uow.GetRepository<banka_hareket>();
            _kasaHareketRepository = _uow.GetRepository<kasa_hareket>();

        }


        //banka kaydet
        public bool Kaydet(muhtelif_islemler model, int banka_id)
        {

            double bankaGiren = 0;
            double bankaCikan = 0;

            double bankaToplamGiren = 0;
            double bankaToplamCikan = 0;

            banka_hareket bankaHareket = new banka_hareket();

            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 999999);

            var refno = "MH" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;
            model.refno = refno;
            model.tarih = DateTime.Now.ToString("yyyy-MM-dd");
            model.banka_id = banka_id;
            try
            {
                bankaHareket.bakiye = model.tutar;
                bankaHareket.islem = model.islem_turu;
                bankaHareket.islem_turu = "alacak dekontu";
                bankaHareket.aciklama = model.aciklama;
                bankaHareket.refno = model.refno;
                bankaHareket.banka_id = banka_id;
                bankaHareket.tarih = DateTime.Now.ToString("yyyy-MM-dd");
                bankaHareket.toplama_katilim = model.toplama_katilim;


                //Gelir islemleri
                if (model.para_birimi == "TL")
                {
                    if (model.muhtelif_tipi == "gelir")
                    {
                        bankaHareket.banka_alacak_tl = model.tutar;
                    }
                    else
                    {
                        bankaHareket.banka_borc_tl = model.tutar;
                    }

                    _muhtelifIslemler.Insert(model);

                    _bankaHareketRepository.Insert(bankaHareket);
                    _uow.SaveChanges();

                  

                    using (var context = new MyArchContext())
                    {
                        bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_tl) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        int girenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET girentutar_tl=' " + bankaGiren + "  ' WHERE id = '" + banka_id + "' ");

                        //banka guncellemesindeki gelir/ gider olarak değiştirilerse gelir güncellmesi icin yapıldı

                        bankaCikan = context.Database.SqlQuery<double>("SELECT SUM(banka_borc_tl) as banka_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        var bankaCikanDeger = bankaCikan.ToString().Replace(",", ".");

                        int cikanGuncellegelen = context.Database.ExecuteSqlCommand("UPDATE banka SET cikantutar_tl=' " + bankaCikanDeger + "  ' WHERE id = '" + banka_id + "' ");

                    }


                    //bakiye update tl
                    using (var context = new MyArchContext())
                    {
                        bankaToplamGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_tl) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        bankaToplamCikan = context.Database.SqlQuery<double>("SELECT SUM(banka_borc_tl) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        var bakiyeSon = bankaToplamGiren - bankaToplamCikan;

                        var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");

                        int cikanDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET bakiye_tl=' " + bakiyeSonDeger + "  ' WHERE id = '" + banka_id + "' ");
                    }

                }
                if (model.para_birimi == "USD")
                {
                    if (model.muhtelif_tipi == "gelir")
                    {
                        bankaHareket.banka_alacak_dolar = model.tutar;
                    }
                    else
                    {
                        bankaHareket.banka_borc_dolar = model.tutar;
                    }
                    _muhtelifIslemler.Insert(model);

                    _bankaHareketRepository.Insert(bankaHareket);
                    _uow.SaveChanges();

                    using (var context = new MyArchContext())
                    {
                        bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_dolar) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        int girenDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET girentutar_dolar=' " + bankaGiren + "  ' WHERE id = '" + banka_id + "' ");

                        //banka guncellemesindeki gelir/ gider olarak değiştirilerse gelir güncellmesi icin yapıldı
                        bankaCikan = context.Database.SqlQuery<double>("SELECT SUM(banka_borc_dolar) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        var bankaCikanDeger = bankaCikan.ToString().Replace(",", ".");

                        int cikanGuncellegelen = context.Database.ExecuteSqlCommand("UPDATE banka SET cikantutar_dolar=' " + bankaCikanDeger + "  ' WHERE id = '" + banka_id + "' ");
                    }

                    //bakiye update dolar
                    using (var context = new MyArchContext())
                    {
                        bankaToplamGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_dolar) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        bankaToplamCikan = context.Database.SqlQuery<double>("SELECT SUM(banka_borc_dolar) as toplam_alacak FROM banka_hareket WHERE banka_id='" + banka_id + "' ").SingleOrDefault();

                        var bakiyeSon = bankaToplamGiren - bankaToplamCikan;

                        var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");

                        int cikanDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + banka_id + "' ");
                    }

                }
                //euro yapıalcak
                //if (model.para_birimi == "USD")
                //{
                //    bankaHareket.banka_alacak_dolar = model.tutar;
                //    _muhtelifIslemler.Insert(model);

                //}

                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool KaydetKasa(muhtelif_islemler model, int kasa_id)
        {
            double kasaGiren = 0;
            double kasaCikan = 0;

            double kasaToplamGiren = 0;
            double kasaToplamCikan = 0;

            kasa_hareket kasaHareket = new kasa_hareket();

            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 999999);

            var refno = "MH" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;
            model.refno = refno;
            model.tarih = DateTime.Now.ToString("yyyy-MM-dd");
            model.kasa_id = kasa_id;
            try
            {
                kasaHareket.bakiye = model.tutar;
                kasaHareket.islem = model.islem_turu;
                kasaHareket.islem_turu = "alacak dekontu";
                kasaHareket.aciklama = model.aciklama;
                kasaHareket.refno = model.refno;
                kasaHareket.kasa_id = kasa_id;
                kasaHareket.toplama_katilim = model.toplama_katilim;
                kasaHareket.tarih = DateTime.Now.ToString("yyyy-MM-dd");

                if (model.para_birimi == "TL")
                {
                    if (model.muhtelif_tipi == "gelir")
                    {
                        kasaHareket.kasa_alacak_tl = model.tutar;
                    }
                    else
                    {
                        kasaHareket.kasa_borc_tl = model.tutar;
                    }


                    _muhtelifIslemler.Insert(model);

                    _kasaHareketRepository.Insert(kasaHareket);
                    _uow.SaveChanges();

                    using (var context = new MyArchContext())
                    {
                        kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_tl) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var kasaGirenDeger = kasaGiren.ToString().Replace(",", ".");

                        int girenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_tl=' " + kasaGirenDeger + "  ' WHERE id = '" + kasa_id + "' ");

                        //kasa guncellemesindeki gelir/ gider olarak değiştirilerse gelir güncellmesi icin yapıldı

                        kasaCikan = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_tl) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var kasaCikanDeger = kasaCikan.ToString().Replace(",", ".");

                        int cikanGuncellegelen = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_tl=' " + kasaCikanDeger + "  ' WHERE id = '" + kasa_id + "' ");
                    }


                    //bakiye update tl
                    using (var context = new MyArchContext())
                    {
                        kasaToplamGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_tl) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        kasaToplamCikan = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_tl) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var bakiyeSon = kasaToplamGiren - kasaToplamCikan;

                        var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");

                        int cikanDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_tl=' " + bakiyeSonDeger + "  ' WHERE id = '" + kasa_id + "' ");
                    }

                }
                if (model.para_birimi == "USD")
                {
                    if (model.muhtelif_tipi == "gelir")
                    {
                        kasaHareket.kasa_alacak_dolar = model.tutar;
                    }
                    else
                    {
                        kasaHareket.kasa_borc_dolar = model.tutar;
                    }


                    _muhtelifIslemler.Insert(model);
                    _kasaHareketRepository.Insert(kasaHareket);
                    _uow.SaveChanges();

                    using (var context = new MyArchContext())
                    {
                        kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_dolar) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var kasaGirenDeger = kasaGiren.ToString().Replace(",", ".");

                        int girenDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_dolar=' " + kasaGirenDeger + "  ' WHERE id = '" + kasa_id + "' ");


                        //kasa guncellemesindeki gelir/ gider olarak değiştirilerse gelir güncellmesi icin yapıldı

                        kasaCikan = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_dolar) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var kasaCikanDeger = kasaCikan.ToString().Replace(",", ".");

                        int cikanGuncellegelen = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_dolar=' " + kasaCikanDeger + "  ' WHERE id = '" + kasa_id + "' ");
                    }

                    //bakiye update dolar
                    using (var context = new MyArchContext())
                    {
                        kasaToplamGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_dolar) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        kasaToplamCikan = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_dolar) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + kasa_id + "' ").SingleOrDefault();

                        var bakiyeSon = kasaToplamGiren - kasaToplamCikan;

                        var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");

                        int cikanDolarGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + kasa_id + "' ");
                    }

                }
                //euro yapıalcak
                //if (model.para_birimi == "USD")
                //{
                //    bankaHareket.banka_alacak_dolar = model.tutar;
                //    _muhtelifIslemler.Insert(model);

                //}

                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public List<muhtelif_islemler> Liste()
        {
            try
            {
                var liste = _muhtelifIslemler.GetAll().ToList();
                return liste;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public muhtelif_islemler MuhtelifGetir(int id)
        {
            var muhtelifListeId = _muhtelifIslemler.GetAll().Where(x => x.id == id).FirstOrDefault();
            return muhtelifListeId;
        }

        public bool KasaGuncelle(muhtelif_islemler model)
        {
            //muhtelif islem ve kasa_harekette silme işlemi yapıldıktan sonra kaydet calıstıralacak
            try
            {
                using (var context = new MyArchContext())
                {
                    int muhtelifSil = context.Database.ExecuteSqlCommand("DELETE FROM muhtelif_islemler where refno = '" + model.refno + "'");
                    int kasaHareketSil = context.Database.ExecuteSqlCommand("DELETE FROM kasa_hareket where refno = '" + model.refno + "'");

                    //yeni muhtelif islemi kaydet
                    //_muhtelifIslemler.Insert(model);
                    //_uow.SaveChanges();

                    if (muhtelifSil > 0 && kasaHareketSil > 0 && KaydetKasa(model, model.kasa_id) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }

        }

        public double BagimsizAlanHesabı(string paraBirimi, string bastarih, string bitTarih)
        {
            double kasaBorc = 0;
            int haneSayisi;
            double aidatTutari = 0;
            try
            {
                using (var context = new MyArchContext())
                {

                    if (paraBirimi == "TL")
                    {
                        kasaBorc = context.Database.SqlQuery<double>("SELECT (SELECT COALESCE (SUM(kasa_borc_tl-kasa_alacak_tl),0) FROM kasa_hareket where toplama_katilim = 'true' and tarih BETWEEN '" + bastarih + "' AND '" + bitTarih + "' ) + (SELECT  COALESCE(SUM(banka_borc_tl-banka_alacak_tl),0) FROM banka_hareket where toplama_katilim = 'true' and tarih BETWEEN '" + bastarih + "' AND '" + bitTarih + "' ) as sonuc ").SingleOrDefault();

                        haneSayisi = context.Database.SqlQuery<int>("select COUNT(id) as hane_sayisi from bagimsiz_bolumler").SingleOrDefault();

                        aidatTutari = kasaBorc / haneSayisi;
                    }

                    if (paraBirimi == "USD")
                    {
                        kasaBorc = context.Database.SqlQuery<double>("SELECT (SELECT COALESCE (SUM(kasa_borc_dolar-kasa_alacak_dolar),0) FROM kasa_hareket where toplama_katilim = 'true' and tarih BETWEEN '" + bastarih + "' AND '" + bitTarih + "' ) + (SELECT  COALESCE(SUM(banka_borc_dolar-banka_alacak_dolar),0) FROM banka_hareket where toplama_katilim = 'true' and tarih BETWEEN '" + bastarih + "' AND '" + bitTarih + "' ) as sonuc ").SingleOrDefault();

                        haneSayisi = context.Database.SqlQuery<int>("select COUNT(id) as hane_sayisi from bagimsiz_bolumler").SingleOrDefault();

                        aidatTutari = kasaBorc / haneSayisi;
                    }
                    //else
                    //{
                    //    //euro işlemleri
                    //    kasaBorc = context.Database.SqlQuery<double>("select SUM(kasa_borc_euro) as toplam_borc from kasa_hareket where toplama_katilim = 'true' ").SingleOrDefault();

                    //    haneSayisi = context.Database.SqlQuery<int>("select COUNT(id) as hane_sayisi from bagimsiz_bolumler").SingleOrDefault();

                    //    aidatTutari = kasaBorc / haneSayisi;
                    //}



                }

                return aidatTutari;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
