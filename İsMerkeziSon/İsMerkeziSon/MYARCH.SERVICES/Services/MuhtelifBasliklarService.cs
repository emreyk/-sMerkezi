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
    public class MuhtelifBasliklarService : IMuhtelifBasliklarService
    {

        private readonly IGenericRepository<muhtelif_baslikler> _muhtelifBasliklar;
        private readonly IUnitofWork _uow;

        public MuhtelifBasliklarService(UnitofWork uow)
        {
            _uow = uow;
            _muhtelifBasliklar = _uow.GetRepository<muhtelif_baslikler>();

        }

        public muhtelif_baslikler BaslikGetir(int id)
        {
            try
            {
                var baslik = _muhtelifBasliklar.GetAll().Where(x => x.id == id).FirstOrDefault();
                return baslik;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<muhtelif_baslikler> basliklar()
        {
            try
            {
                var basliklar = _muhtelifBasliklar.GetAll().ToList();
                return basliklar;
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        public bool baslikSil(muhtelif_baslikler model)
        {
            try
            {

                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM muhtelif_baslikler where id = '" + model.id + "'") > 0)
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

                throw ex;
            }
        }

        public bool MuhtelifIslemSil(muhtelif_islemler model)
        {
            try
            {
                double kasaAlacak = 0;
                double kasaBorc = 0;

                double bankaAlacak = 0;
                double bankaBorc = 0;

                using (var context = new MyArchContext())
                {
                    //muhtelif işlem sil
                    if (context.Database.ExecuteSqlCommand("DELETE FROM muhtelif_islemler where refno = '" + model.refno + "'") > 0)
                    {
                        //Nakit 
                        if (model.islem_turu == "Nakit")
                        {
                            //kasa hareketden sil
                            context.Database.ExecuteSqlCommand("DELETE FROM kasa_hareket where refno = '" + model.refno + "'");

                            //kasa update islemi
                            if (model.para_birimi == "TL")
                            {
                                kasaAlacak = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_alacak_tl),0) as alacak_tl FROM kasa_hareket ").SingleOrDefault();

                                kasaBorc = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_borc_tl),0) as borc_tl FROM kasa_hareket ").SingleOrDefault();

                                //kasa giren ve cikan tl update
                                int kasaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_tl=' " + kasaAlacak.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.kasa_id + "' ");

                                int kasaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_tl=' " + kasaBorc.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.kasa_id + "' ");

                                //kasa bakiye guncelle

                                var bakiyeSon = kasaAlacak - kasaBorc;

                                var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                                int kasaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_tl=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.kasa_id + "' ");
                            }
                            //kasa update islemi
                            if (model.para_birimi == "USD")
                            {
                                kasaAlacak = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_alacak_dolar),0) as alacak_dolar FROM kasa_hareket ").SingleOrDefault();

                                kasaBorc = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_borc_dolar).0) as borc_dolar FROM kasa_hareket ").SingleOrDefault();

                                //kasa giren ve cikan tl update
                                int kasaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_dolar=' " + kasaAlacak.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.kasa_id + "' ");

                                int kasaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_dolar=' " + kasaBorc.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.kasa_id + "' ");

                                //kasa bakiye guncelle

                                var bakiyeSon = kasaAlacak - kasaBorc;

                                var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                                int kasaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.kasa_id + "' ");
                            }
                            //if (model.para_birimi == "EURO")
                            //{
                            //    kasaAlacak = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_dolar) as alacak_dolar FROM kasa_hareket ").SingleOrDefault();

                            //    kasaBorc = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_dolar) as borc_dolar FROM kasa_hareket ").SingleOrDefault();

                            //    //kasa giren ve cikan tl update
                            //    int kasaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_dolar=' " + kasaAlacak + "  ' WHERE id = '" + model.kasa_id + "' ");

                            //    int kasaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_dolar=' " + kasaBorc + "  ' WHERE id = '" + model.kasa_id + "' ");

                            //    //kasa bakiye guncelle

                            //    var bakiyeSon = kasaAlacak - kasaBorc;

                            //    var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                            //    int kasaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.kasa_id + "' ");
                            //}
                        }
                        else
                        {

                            //banka hareketden sil
                            context.Database.ExecuteSqlCommand("DELETE FROM banka_hareket where refno = '" + model.refno + "'");

                            //banka update islemi
                            if (model.para_birimi == "TL")
                            {
                                bankaAlacak = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_tl),0) as alacak_tl FROM banka_hareket ").SingleOrDefault();

                                bankaBorc = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_borc_tl),0) as borc_tl FROM banka_hareket ").SingleOrDefault();

                                //banka giren ve cikan tl update
                                int bankaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET girentutar_tl=' " + bankaAlacak.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.banka_id + "' ");

                                int bankaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET cikantutar_tl=' " + bankaBorc.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.banka_id + "' ");

                                //banka bakiye guncelle

                                var bakiyeSon = bankaAlacak - bankaBorc;

                                var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                                int bankaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET bakiye_tl=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.banka_id + "' ");
                            }
                            //banka update islemi
                            if (model.para_birimi == "USD")
                            {
                                bankaAlacak = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_dolar),0) as alacak_dolar FROM banka_hareket ").SingleOrDefault();

                                bankaBorc = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_borc_dolar),0) as borc_dolar FROM banka_hareket ").SingleOrDefault();

                                //banka giren ve cikan tl update
                                int bankaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET girentutar_dolar=' " + bankaAlacak.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.banka_id + "' ");

                                int bankaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET cikantutar_dolar=' " + bankaBorc.ToString().Replace(",", ".") + "  ' WHERE id = '" + model.banka_id + "' ");

                                //banka bakiye guncelle

                                var bakiyeSon = bankaAlacak - bankaBorc;

                                var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                                int bankaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE banka SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.banka_id + "' ");
                            }
                            //if (model.para_birimi == "EURO")
                            //{
                            //    kasaAlacak = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_dolar) as alacak_dolar FROM kasa_hareket ").SingleOrDefault();

                            //    kasaBorc = context.Database.SqlQuery<double>("SELECT SUM(kasa_borc_dolar) as borc_dolar FROM kasa_hareket ").SingleOrDefault();

                            //    //kasa giren ve cikan tl update
                            //    int kasaGirenTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET girentutar_dolar=' " + kasaAlacak + "  ' WHERE id = '" + model.kasa_id + "' ");

                            //    int kasaCikanTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET cikantutar_dolar=' " + kasaBorc + "  ' WHERE id = '" + model.kasa_id + "' ");

                            //    //kasa bakiye guncelle

                            //    var bakiyeSon = kasaAlacak - kasaBorc;

                            //    var bakiyeSonDeger = bakiyeSon.ToString().Replace(",", ".");


                            //    int kasaBakiyeTlGuncelle = context.Database.ExecuteSqlCommand("UPDATE kasa SET bakiye_dolar=' " + bakiyeSonDeger + "  ' WHERE id = '" + model.kasa_id + "' ");
                            //}

                        }

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

                throw ex;
            }
        }

        public bool Kaydet(muhtelif_baslikler model)
        {
            try
            {
                _muhtelifBasliklar.Insert(model);
                if (_uow.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Guncelle(muhtelif_baslikler model)
        {
            try
            {
                var gelenbaslik = _muhtelifBasliklar.Find(model.id);
                gelenbaslik.baslik_adi = model.baslik_adi;
                gelenbaslik.para_birimi = model.para_birimi;
                gelenbaslik.toplama_katilim = model.toplama_katilim;


                _muhtelifBasliklar.Update(gelenbaslik);
                if (_uow.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ToplamaEkleSecim(string baslikKodu)
        {
            try
            {
                var secim = _muhtelifBasliklar.GetAll().Where(x => x.baslik_kodu == baslikKodu).FirstOrDefault().toplama_katilim;
                return secim;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
