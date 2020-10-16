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
    public class BorclandirmaService : IBorclandirmaService
    {

        private readonly IGenericRepository<hesap_hareket> _borclandirmaRepository;
        private readonly IGenericRepository<kasa> _kasaRepository;
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IGenericRepository<banka> _bankaRepository;
        private readonly IGenericRepository<kasa_hareket> _kasaHareketRepository;
        private readonly IGenericRepository<banka_hareket> _bankaHareketRepository;
        private readonly IGenericRepository<hesap_hareket> _hesapHareketRepository;
        private readonly IGenericRepository<toplu_borclandir> _topluBorclandirRepository;
        private readonly IGenericRepository<yakit> _yakitRepository;
        private readonly IGenericRepository<bagimsiz_bolumler> _bagimsizBolumlerRepository;
        private readonly IUnitofWork _uow;

        public BorclandirmaService(UnitofWork uow)
        {
            _uow = uow;
            _borclandirmaRepository = _uow.GetRepository<hesap_hareket>();
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _kasaRepository = _uow.GetRepository<kasa>();
            _bankaRepository = _uow.GetRepository<banka>();
            _kasaHareketRepository = _uow.GetRepository<kasa_hareket>();
            _bankaHareketRepository = _uow.GetRepository<banka_hareket>();
            _hesapHareketRepository = _uow.GetRepository<hesap_hareket>();
            _topluBorclandirRepository = _uow.GetRepository<toplu_borclandir>();
            _yakitRepository = _uow.GetRepository<yakit>();
            _bagimsizBolumlerRepository = _uow.GetRepository<bagimsiz_bolumler>();

        }


        //tahsilat ve tediye islemleri
        public bool BorclandirmaKaydet(hesap_hareket model)
        {
            kisiler kisi = new kisiler();
            kisiler kisiIdModel = _kisilerRepository.Find(model.kisi_id);

            kasa kasa = new kasa();
            kasa kasaModel = _kasaRepository.Find(model.kasa_id);

            banka banka = new banka();
            banka bankaModel = _bankaRepository.Find(model.banka_id);

            kasa_hareket kasa_hareket = new kasa_hareket();
            banka_hareket banka_hareket = new banka_hareket();

            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 999999);

            var refno = "TH" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;
            var refnoBorc = "TD" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;


            try
            {

                double borc = 0;
                double alacak = 0;
                double kasaGiren = 0;
                double bankaGiren = 0;
                string hesapAdi;

                double hhAlacak = 0;
                double tahsilatToplam = 0;

                if (model.islem_turu == "borç dekontu")
                {
                    using (var context = new MyArchContext())
                    {
                        hesapAdi = context.Database.SqlQuery<string>("SELECT  isim FROM kisiler WHERE id = '" + model.kisi_id + "' ").SingleOrDefault();
                        model.hesap_adi = hesapAdi;
                        model.refno = refnoBorc;
                        model.bakiye = model.borc - model.alacak;
                        model.tahsilat_durumu = "ödenmedi";

                        model.gun = model.tarih.Day.ToString();
                        model.ay = model.tarih.Month.ToString();
                        model.yil = model.tarih.Year.ToString();
                        model.saat = DateTime.Now.ToShortTimeString();
                        model.para_birimi = model.para_birimi;

                        if (model.borclandirma_turu == "Akaryakıt")
                        {
                            double toplamPetekBoyutu = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_petek_uzunluk;
                            double toplamTutar = _yakitRepository.GetAll().OrderByDescending(x => x.id).FirstOrDefault().toplam_tutar;
                            var petekBoyu = _bagimsizBolumlerRepository.GetAll().Where(x => x.id == model.bagimsiz_id).FirstOrDefault().petek_boyu;
                            double yakitBorc = (Convert.ToDouble(petekBoyu) / toplamPetekBoyutu) * toplamTutar;

                            double ikiBasamakYakitBorc = Math.Round(yakitBorc, 2);

                            model.borc = ikiBasamakYakitBorc;
                            model.bakiye = ikiBasamakYakitBorc - model.alacak;
                        }

                        _borclandirmaRepository.Insert(model);
                        _uow.SaveChanges();


                        if (model.para_birimi == "USD")
                        {

                            borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + model.kisi_id + "' and para_birimi = 'USD' ").SingleOrDefault();
                            kisiIdModel.borc_dolar = borc;
                        }
                        else
                        {

                            borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + model.kisi_id + "' and para_birimi = 'TL' ").SingleOrDefault();
                            kisiIdModel.borc_tl = borc;
                        }

                    }
                }
                else
                {

                    using (var context = new MyArchContext())
                    {

                        hesapAdi = context.Database.SqlQuery<string>("SELECT  isim FROM kisiler WHERE id = '" + model.kisi_id + "' ").SingleOrDefault();
                        model.hesap_adi = hesapAdi;
                        model.refno = refno;
                        model.bakiye = model.borc - model.alacak;

                        model.gun = model.tarih.Day.ToString();
                        model.ay = model.tarih.Month.ToString();
                        model.yil = model.tarih.Year.ToString();
                        model.saat = DateTime.Now.ToShortTimeString();
                        model.para_birimi = model.para_birimi;
                        


                        _borclandirmaRepository.Insert(model);
                        _uow.SaveChanges();

                        //alacak = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE kisi_id = '" + model.kisi_id + "' ").SingleOrDefault();
                        //kisiIdModel.alacak = alacak;

                        //kisiler alacak gunceleme islemi
                        if (model.para_birimi == "USD")
                        {

                            alacak = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE kisi_id = '" + model.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                            kisiIdModel.alacak_dolar = alacak;
                        }
                        else
                        {
                            alacak = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE kisi_id = '" + model.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                            kisiIdModel.alacak_tl = alacak;
                        }

                        hhAlacak = context.Database.SqlQuery<double>("SELECT borc FROM hesap_hareket WHERE id = '" + model.tahsilat_id + "' ").SingleOrDefault();
                        tahsilatToplam = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE tahsilat_id = '" + model.tahsilat_id + "' ").SingleOrDefault();

                        if (hhAlacak == tahsilatToplam)
                        {
                            int guncelle = context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET tahsilat_durumu='ödendi' WHERE id = '" + model.tahsilat_id + "' ");
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET aciklama='" + model.aciklama + "' WHERE id = '" + model.tahsilat_id + "' ");
                        }
                        if (hhAlacak > tahsilatToplam)
                        {
                            int guncelle = context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET tahsilat_durumu='eksik tahsilat' WHERE id = '" + model.tahsilat_id + "' ");

                            var sonAciklama = model.aciklama + " [ " + " kalan " + (hhAlacak - tahsilatToplam).ToString() + " ] ";
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET aciklama=' " + sonAciklama + "  ' WHERE id = '" + model.tahsilat_id + "' ");
                        }


                        //kasa ve banka hareketleri

                        if (model.islem == "Nakit")
                        {
                            kasa_hareket.tarih = DateTime.Now.ToString("yyyy-MM-dd");
                            if (model.para_birimi == "USD")
                            {

                                //kasa haraket insert
                                kasa_hareket.hesap_adi = hesapAdi;
                                kasa_hareket.islem = model.islem;
                                kasa_hareket.islem_turu = model.islem_turu;
                                kasa_hareket.kasa_alacak_dolar = model.alacak;
                                kasa_hareket.aciklama = model.aciklama;
                                kasa_hareket.bakiye = model.alacak - model.borc;
                                kasa_hareket.refno = refno;
                                kasa_hareket.kisi_id = model.kisi_id;
                                kasa_hareket.kasa_id = model.kasa_id;
                                
                                _kasaHareketRepository.Insert(kasa_hareket);
                                _uow.SaveChanges();

                                // kasa  tablosu update islemleri
                                List<kasa> kasaIdler = context.Database.SqlQuery<kasa>("SELECT * FROM kasa ").ToList();

                                foreach (var item in kasaIdler)
                                {
                                    kasaGiren = context.Database.SqlQuery<double>("SELECT COALESCE (SUM(kasa_alacak_dolar),0) as toplam_alacak FROM kasa_hareket WHERE kasa_id = '" + item.id + "' ").SingleOrDefault();

                                    kasa kasaSonModel = _kasaRepository.Find(item.id);

                                    kasaSonModel.girentutar_dolar = kasaGiren;
                                    kasaSonModel.bakiye_dolar = kasaGiren - kasa.cikantutar_dolar;
                                    _kasaRepository.Update(kasaSonModel);
                                    _uow.SaveChanges();
                                }


                                //banka tahsilatı kasa olarak guncellenmek istenirse banka tablosu yeniden update için yapıldı
                                List<banka> bankaIdler = context.Database.SqlQuery<banka>("SELECT * FROM banka ").ToList();

                                foreach (var item in bankaIdler)
                                {
                                    bankaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_dolar),0) as toplam_alacak FROM banka_hareket WHERE banka_id = '" + item.id + "' ").SingleOrDefault();


                                    banka bankaSonModel = _bankaRepository.Find(item.id);

                                    bankaSonModel.girentutar_dolar = bankaGiren;
                                    bankaSonModel.bakiye_dolar = bankaGiren - banka.cikantutar_dolar;
                                    _bankaRepository.Update(bankaSonModel);
                                    _uow.SaveChanges();
                                }

                            }
                            if (model.para_birimi == "TL")
                            {

                                //kasa haraket insert
                                kasa_hareket.hesap_adi = hesapAdi;
                                kasa_hareket.islem = model.islem;
                                kasa_hareket.islem_turu = model.islem_turu;
                                kasa_hareket.kasa_alacak_tl = model.alacak;
                                kasa_hareket.aciklama = model.aciklama;
                                kasa_hareket.bakiye = model.alacak - model.borc;
                                kasa_hareket.refno = refno;
                                kasa_hareket.kisi_id = model.kisi_id;
                                kasa_hareket.kasa_id = model.kasa_id;
                                _kasaHareketRepository.Insert(kasa_hareket);
                                _uow.SaveChanges();

                                // kasa  tablosu update islemleri
                                List<kasa> kasaIdler = context.Database.SqlQuery<kasa>("SELECT * FROM kasa ").ToList();

                                foreach (var item in kasaIdler)
                                {
                                    kasaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_alacak_tl),0) as toplam_alacak FROM kasa_hareket WHERE kasa_id = '" + item.id + "' ").SingleOrDefault();

                                    kasa kasaSonModel = _kasaRepository.Find(item.id);
                                    kasaSonModel.girentutar_tl = kasaGiren;
                                    kasaSonModel.bakiye_tl = kasaGiren - kasa.cikantutar_tl;
                                    _kasaRepository.Update(kasaSonModel);
                                    _uow.SaveChanges();
                                }

                                //banka tahsilatı kasa olarak guncellenmek istenirse banka tablosu yeniden update için yapıldı
                                List<banka> bankaIdler = context.Database.SqlQuery<banka>("SELECT * FROM banka ").ToList();

                                foreach (var item in bankaIdler)
                                {
                                    bankaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_tl),0) as toplam_alacak FROM banka_hareket WHERE banka_id = '" + item.id + "' ").SingleOrDefault();

                                    banka bankaSonModel = _bankaRepository.Find(item.id);

                                    bankaSonModel.girentutar_tl = bankaGiren;
                                    bankaSonModel.bakiye_tl = bankaGiren - banka.cikantutar_tl;
                                    _bankaRepository.Update(bankaSonModel);
                                    _uow.SaveChanges();
                                }

                            }

                            //else euro yapılacak
                        }

                        else
                        {

                            banka_hareket.tarih = DateTime.Now.ToString("yyyy-MM-dd");
                            if (model.para_birimi == "USD")
                            {

                                //banka haraket insert
                                banka_hareket.hesap_adi = hesapAdi;
                                banka_hareket.islem = model.islem;
                                banka_hareket.islem_turu = model.islem_turu;
                                banka_hareket.banka_alacak_dolar = model.alacak;
                                banka_hareket.aciklama = model.aciklama;
                                banka_hareket.bakiye = model.alacak - model.borc;
                                banka_hareket.refno = refno;
                                banka_hareket.kisi_id = model.kisi_id;
                                banka_hareket.banka_id = model.banka_id;
                                _bankaHareketRepository.Insert(banka_hareket);
                                _uow.SaveChanges();

                                List<banka> bankaIdler = context.Database.SqlQuery<banka>("SELECT * FROM banka ").ToList();

                                foreach (var item in bankaIdler)
                                {
                                    bankaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_dolar),0) as toplam_alacak FROM banka_hareket WHERE banka_id = '" + item.id + "' ").SingleOrDefault();

                              
                                    banka bankaSonModel = _bankaRepository.Find(item.id);

                                    bankaSonModel.girentutar_dolar = bankaGiren;
                                    bankaSonModel.bakiye_dolar = bankaGiren - banka.cikantutar_dolar;
                                    _bankaRepository.Update(bankaSonModel);
                                    _uow.SaveChanges();
                                }

                                // banka tablosu tahsilatı kasa olarak değiştirilirse kasa yeniden upload işemi yapıldı
                                List<kasa> kasaIdler = context.Database.SqlQuery<kasa>("SELECT * FROM kasa ").ToList();

                                foreach (var item in kasaIdler)
                                {
                                    kasaGiren = context.Database.SqlQuery<double>("SELECT COALESCE (SUM(kasa_alacak_dolar),0) as toplam_alacak FROM kasa_hareket WHERE kasa_id = '" + item.id + "' ").SingleOrDefault();

                                    kasa kasaSonModel = _kasaRepository.Find(item.id);

                                    kasaSonModel.girentutar_dolar = kasaGiren;
                                    kasaSonModel.bakiye_dolar = kasaGiren - kasa.cikantutar_dolar;
                                    _kasaRepository.Update(kasaSonModel);
                                    _uow.SaveChanges();
                                }
                            }
                            else
                            {
                                //banka haraket insert
                                banka_hareket.hesap_adi = hesapAdi;
                                banka_hareket.islem = model.islem;
                                banka_hareket.islem_turu = model.islem_turu;
                                banka_hareket.banka_alacak_tl = model.alacak;
                                banka_hareket.aciklama = model.aciklama;
                                banka_hareket.bakiye = model.alacak - model.borc;
                                banka_hareket.refno = refno;
                                banka_hareket.kisi_id = model.kisi_id;
                                banka_hareket.banka_id = model.banka_id;
                                _bankaHareketRepository.Insert(banka_hareket);
                                _uow.SaveChanges();

                                List<banka> bankaIdler = context.Database.SqlQuery<banka>("SELECT * FROM banka ").ToList();

                                foreach (var item in bankaIdler)
                                {
                                    bankaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(banka_alacak_tl),0) as toplam_alacak FROM banka_hareket WHERE banka_id = '" + item.id + "' ").SingleOrDefault();

                                    banka bankaSonModel = _bankaRepository.Find(item.id);

                                    bankaSonModel.girentutar_tl = bankaGiren;
                                    bankaSonModel.bakiye_tl = bankaGiren - banka.cikantutar_tl;
                                    _bankaRepository.Update(bankaSonModel);
                                    _uow.SaveChanges();
                                }

                                // banka tablosu tahsilatı kasa olarak değiştirilirse kasa yeniden upload işemi yapıldı
                                List<kasa> kasaIdler = context.Database.SqlQuery<kasa>("SELECT * FROM kasa ").ToList();

                                foreach (var item in kasaIdler)
                                {
                                    kasaGiren = context.Database.SqlQuery<double>("SELECT COALESCE(SUM(kasa_alacak_tl),0) as toplam_alacak FROM kasa_hareket WHERE kasa_id = '" + item.id + "' ").SingleOrDefault();

                                    kasa kasaSonModel = _kasaRepository.Find(item.id);
                                    kasaSonModel.girentutar_tl = kasaGiren;
                                    kasaSonModel.bakiye_tl = kasaGiren - kasa.cikantutar_tl;
                                    _kasaRepository.Update(kasaSonModel);
                                    _uow.SaveChanges();
                                }

                            }
                        }
                    }
                }

                _kisilerRepository.Update(kisiIdModel);

                //model.hesap_adi = hesapAdi;
                //model.refno = refno;
                //_borclandirmaRepository.Insert(model);
                if (_uow.SaveChanges() > 0)
                {
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

        public List<EHesapHaraketDTO> BorclandirmaListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListe = context.Database.SqlQuery<EHesapHaraketDTO>("select borc.id,aciklama,borc.para_birimi,tarih,sonodeme_tarihi,isim,blok_adi,daire_numarasi,borclandirma_turu,borc.borc,kisi_id,tahsilat_durumu from hesap_hareket as borc left join kisiler as ks on ks.id=borc.kisi_id left join bagimsiz_bolumler as bs on bs.id=borc.bagimsiz_id WHERE islem_turu = 'borç dekontu'  ORDER BY id DESC").ToList();
                    return borcListe;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //finansal menusu borclandırmadaki guncelleme islemi
        public bool FinansalBorcGuncelle(EFinansalBorcDuzenle model)
        {
            hesap_hareket hesap_hareket = new hesap_hareket();
            hesap_hareket hesapHareketIdModel = _hesapHareketRepository.Find(model.id);

            kisiler kisi = new kisiler();
            kisiler kisiIdModel = _kisilerRepository.Find(model.kisi_id);

            hesapHareketIdModel.tarih = model.tarih;
            hesapHareketIdModel.sonodeme_tarihi = model.sonodeme_tarihi;
            hesapHareketIdModel.borclandirma_turu = model.borclandirma_turu;
            hesapHareketIdModel.borc = model.borc;
            hesapHareketIdModel.bakiye = model.borc;
            hesapHareketIdModel.aciklama = model.aciklama;
            hesapHareketIdModel.para_birimi = model.para_birimi;

            _hesapHareketRepository.Update(hesapHareketIdModel);
            _uow.SaveChanges();


            var borcDolar = (from u in _hesapHareketRepository.GetAll()
                             where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "USD"
                             select new EKisiDTO
                             {
                                 borc = u.borc,
                             }).ToList();

            double toplamBorcDolar = borcDolar.AsEnumerable().Sum(o => o.borc);
            kisiIdModel.borc_dolar = toplamBorcDolar;



            var borcTl = (from u in _hesapHareketRepository.GetAll()
                          where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "TL"
                          select new EKisiDTO
                          {
                              borc = u.borc,
                          }).ToList();

            double toplamBorcTl = borcTl.AsEnumerable().Sum(o => o.borc);
            kisiIdModel.borc_tl = toplamBorcTl;


            //euro guncellenecek
            //if (model.para_birimi == "EURO")
            //{
            //    var borc = (from u in _hesapHareketRepository.GetAll()
            //                where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "EURO"
            //                select new EKisiDTO
            //                {
            //                    borc = u.borc,
            //                }).ToList();

            //    double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
            //    kisiIdModel.borc_tl = toplamBorc;
            //}

            try
            {

                _kisilerRepository.Update(kisiIdModel);
                if (_uow.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }



        public bool FinansalTahsilatGuncelle(EFinansalTahsilatDuzenle model, hesap_hareket hhModel)
        {

            try
            {
                //hhModel.id = model.tahsilat_id;

                bool silSonuc = TahsilatSil(hhModel);
                hhModel.islem_turu = "alacak dekontu";
                bool kaydetSonuc = BorclandirmaKaydet(hhModel);

                if (silSonuc == true && kaydetSonuc == true)
                {
                    return true;
                }
            }
            catch (Exception msg)
            {
                return false;
                throw msg;
            }
            //hesap_hareket hesap_hareket = new hesap_hareket();
            //hesap_hareket hesapHareketIdModel;
            //using (var context = new MyArchContext())
            //{

            //    hesapHareketIdModel = context.Database.SqlQuery<hesap_hareket>("select * from hesap_hareket WHERE refno='" + model.refno + "' ").FirstOrDefault();

            //}

            //var tahsilatYapilanId = hesapHareketIdModel.tahsilat_id;
            //var borc = _hesapHareketRepository.GetAll().Where(x => x.id == tahsilatYapilanId).SingleOrDefault().borc;
            ////var tahsilatDurumu = _hesapHareketRepository.GetAll().Where(x => x.id == tahsilatYapilanId).SingleOrDefault().tahsilat_durumu;
            ////var aciklama = _hesapHareketRepository.GetAll().Where(x => x.id == tahsilatYapilanId).SingleOrDefault().aciklama;

            //kisiler kisi = new kisiler();
            //kisiler kisiIdModel = _kisilerRepository.Find(model.kisi_id);

            ////hesap hareket güncelleme
            //hesapHareketIdModel.tarih = model.tarih;
            //hesapHareketIdModel.alacak = model.alacak;
            //hesapHareketIdModel.aciklama = model.aciklama;
            //hesapHareketIdModel.bakiye = hesapHareketIdModel.borc - model.alacak;
            //if (model.alacak == borc)
            //{
            //    using (var context = new MyArchContext())
            //    {

            //        context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET  aciklama='" + model.aciklama + "',tahsilat_durumu ='ödendi'  WHERE id='" + tahsilatYapilanId + "' ");
            //    }

            //}
            //else
            //{
            //    var kalan = borc - model.alacak;
            //    using (var context = new MyArchContext())
            //    {
            //        var sonAciklama = model.aciklama + " [ " + " kalan " + kalan.ToString() + " ] ";
            //        context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET  aciklama='" + sonAciklama + "',tahsilat_durumu ='eksik tahsilat'  WHERE id='" + tahsilatYapilanId + "' ");
            //    }
            //}
            //_hesapHareketRepository.Update(hesapHareketIdModel);

            //_uow.SaveChanges();

            ////kisi güncelle
            //if (model.para_birimi == "TL")
            //{
            //    var alacak = (from u in _hesapHareketRepository.GetAll()
            //                  where u.kisi_id == model.kisi_id && u.islem_turu == "Tahsilat" && model.para_birimi == "TL"
            //                  select new EKisiDTO
            //                  {
            //                      alacak = u.alacak,
            //                  }).ToList();

            //    double toplamAlacak = alacak.AsEnumerable().Sum(o => o.alacak);
            //    kisiIdModel.alacak_tl = toplamAlacak;
            //}

            //if (model.para_birimi == "USD")
            //{
            //    var alacak = (from u in _hesapHareketRepository.GetAll()
            //                  where u.kisi_id == model.kisi_id && u.islem_turu == "Tahsilat" && model.para_birimi == "USD"
            //                  select new EKisiDTO
            //                  {
            //                      alacak = u.alacak,
            //                  }).ToList();

            //    double toplamAlacak = alacak.AsEnumerable().Sum(o => o.alacak);
            //    kisiIdModel.alacak_dolar = toplamAlacak;
            //}

            //////euro yapılacak
            ////if (model.para_birimi == "TL")
            ////{
            ////    var alacak = (from u in _hesapHareketRepository.GetAll()
            ////                  where u.kisi_id == model.kisi_id && u.islem_turu == "Tahsilat" && model.para_birimi == "TL"
            ////                  select new EKisiDTO
            ////                  {
            ////                      alacak = u.alacak,
            ////                  }).ToList();

            ////    double toplamAlacak = alacak.AsEnumerable().Sum(o => o.alacak);
            ////    kisiIdModel.alacak_tl = toplamAlacak;
            ////}

            //try
            //{
            //    _kisilerRepository.Update(kisiIdModel);
            //    _uow.SaveChanges();
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}


            //if (hesapHareketIdModel.islem == "Kredi kartı")
            //{

            //    //banka hareket guncelle
            //    banka_hareket banka_hareket = new banka_hareket();
            //    banka_hareket bankaHareketIdModel;
            //    using (var context = new MyArchContext())
            //    {
            //        bankaHareketIdModel = context.Database.SqlQuery<banka_hareket>("select * from banka_hareket WHERE refno='" + model.refno + "' ").FirstOrDefault();
            //    }

            //    if (model.para_birimi == "TL")
            //    {
            //        bankaHareketIdModel.aciklama = model.aciklama;
            //        bankaHareketIdModel.banka_alacak_tl = model.alacak;
            //        bankaHareketIdModel.bakiye = bankaHareketIdModel.banka_alacak_tl - bankaHareketIdModel.banka_borc_tl;
            //        _uow.SaveChanges();
            //    }
            //    if (model.para_birimi == "USD")
            //    {
            //        bankaHareketIdModel.aciklama = model.aciklama;
            //        bankaHareketIdModel.banka_alacak_dolar = model.alacak;
            //        bankaHareketIdModel.bakiye = bankaHareketIdModel.banka_alacak_dolar - bankaHareketIdModel.banka_borc_dolar;
            //        _uow.SaveChanges();
            //    }
            //    //euro yapılacak
            //    //if (model.para_birimi == "TL")
            //    //{
            //    //    bankaHareketIdModel.aciklama = model.aciklama;
            //    //    bankaHareketIdModel.banka_borc_tl = model.alacak;
            //    //    bankaHareketIdModel.bakiye = bankaHareketIdModel.banka_borc_tl - bankaHareketIdModel.banka_alacak_tl;
            //    //}
            //    try
            //    {
            //        _bankaHareketRepository.Update(bankaHareketIdModel);

            //    }
            //    catch (Exception ex)
            //    {

            //        throw ex;
            //    }

            //    //banka  guncelle
            //    if (model.para_birimi == "TL")
            //    {
            //        var bankaAlacak = (from u in _bankaHareketRepository.GetAll()
            //                           where u.banka_id == model.banka_id
            //                           select new EKisiDTO
            //                           {
            //                               alacak = u.banka_alacak_tl,
            //                           }).ToList();

            //        double bankatoplamAlacak = bankaAlacak.AsEnumerable().Sum(o => o.alacak);

            //        banka idBanka = _bankaRepository.Find(hesapHareketIdModel.banka_id);
            //        idBanka.girentutar_tl = bankatoplamAlacak;
            //        idBanka.bakiye_tl = idBanka.girentutar_tl - idBanka.cikantutar_tl;

            //        try
            //        {
            //            _bankaRepository.Update(idBanka);

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }

            //    }
            //    if (model.para_birimi == "USD")
            //    {
            //        var bankaAlacak = (from u in _bankaHareketRepository.GetAll()
            //                           where u.banka_id == model.banka_id
            //                           select new EKisiDTO
            //                           {
            //                               alacak = u.banka_alacak_dolar,
            //                           }).ToList();

            //        double bankatoplamAlacak = bankaAlacak.AsEnumerable().Sum(o => o.alacak);

            //        banka idBanka = _bankaRepository.Find(hesapHareketIdModel.banka_id);
            //        idBanka.girentutar_dolar = bankatoplamAlacak;
            //        idBanka.bakiye_dolar = idBanka.girentutar_dolar - idBanka.cikantutar_dolar;

            //        try
            //        {
            //            _bankaRepository.Update(idBanka);

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }

            //    }


            //    //euro yapılacak
            //    //if (model.para_birimi == "USD")
            //    //{
            //    //    var bankaAlacak = (from u in _hesapHareketRepository.GetAll()
            //    //                       where u.banka_id == model.banka_id && model.para_birimi == "USD"
            //    //                       select new EKisiDTO
            //    //                       {
            //    //                           alacak = u.alacak,
            //    //                       }).ToList();

            //    //    double bankatoplamAlacak = bankaAlacak.AsEnumerable().Sum(o => o.alacak);

            //    //    banka idBanka = _bankaRepository.Find(hesapHareketIdModel.banka_id);
            //    //    idBanka.girentutar_dolar = bankatoplamAlacak;
            //    //    idBanka.bakiye_dolar = idBanka.girentutar_dolar - idBanka.cikantutar_dolar;

            //    //    try
            //    //    {
            //    //        _bankaRepository.Update(idBanka);

            //    //    }
            //    //    catch (Exception ex)
            //    //    {

            //    //        throw ex;
            //    //    }

            //    //}
            //    _uow.SaveChanges();

            //}
            //else
            //{
            //    kasa_hareket kasa_hareket = new kasa_hareket();
            //    kasa_hareket kasaHareketIdModel;

            //    //kasa hareket guncelle
            //    if (model.para_birimi == "TL")
            //    {
            //        using (var context = new MyArchContext())
            //        {

            //            kasaHareketIdModel = context.Database.SqlQuery<kasa_hareket>("select * from kasa_hareket WHERE refno='" + model.refno + "' ").FirstOrDefault();

            //        }

            //        kasaHareketIdModel.aciklama = model.aciklama;
            //        kasaHareketIdModel.kasa_alacak_tl = model.alacak;
            //        kasaHareketIdModel.bakiye = kasaHareketIdModel.kasa_alacak_tl - kasaHareketIdModel.kasa_borc_tl;
            //        try
            //        {
            //            _kasaHareketRepository.Update(kasaHareketIdModel);
            //            _uow.SaveChanges();

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }

            //    }

            //    if (model.para_birimi == "USD")
            //    {
            //        using (var context = new MyArchContext())
            //        {

            //            kasaHareketIdModel = context.Database.SqlQuery<kasa_hareket>("select * from kasa_hareket WHERE refno='" + model.refno + "' ").FirstOrDefault();

            //        }

            //        kasaHareketIdModel.aciklama = model.aciklama;
            //        kasaHareketIdModel.kasa_alacak_dolar = model.alacak;
            //        kasaHareketIdModel.bakiye = kasaHareketIdModel.kasa_alacak_dolar - kasaHareketIdModel.kasa_borc_dolar;
            //        try
            //        {
            //            _kasaHareketRepository.Update(kasaHareketIdModel);
            //            _uow.SaveChanges();

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }

            //    }
            //    //euro yapılacak
            //    //if (model.para_birimi == "USD")
            //    //{
            //    //    using (var context = new MyArchContext())
            //    //    {

            //    //        kasaHareketIdModel = context.Database.SqlQuery<kasa_hareket>("select * from kasa_hareket WHERE refno='" + model.refno + "' ").FirstOrDefault();

            //    //    }

            //    //    kasaHareketIdModel.aciklama = model.aciklama;
            //    //    kasaHareketIdModel.kasa_borc_dolar = model.alacak;
            //    //    kasaHareketIdModel.bakiye = kasaHareketIdModel.kasa_borc_dolar - kasaHareketIdModel.kasa_alacak_dolar;
            //    //    try
            //    //    {
            //    //        _kasaHareketRepository.Update(kasaHareketIdModel);


            //    //    }
            //    //    catch (Exception ex)
            //    //    {

            //    //        throw ex;
            //    //    }

            //    //}
            //    //kasa  guncelle
            //    if (model.para_birimi == "TL")
            //    {
            //        var kasaAlacak = (from u in _kasaHareketRepository.GetAll()
            //                          where u.kasa_id == model.kasa_id
            //                          select new EKisiDTO
            //                          {
            //                              alacak = u.kasa_alacak_tl,
            //                          }).ToList();

            //        double kasatoplamAlacak = kasaAlacak.AsEnumerable().Sum(o => o.alacak);

            //        kasa idKasa = _kasaRepository.Find(hesapHareketIdModel.kasa_id);
            //        idKasa.girentutar_tl = kasatoplamAlacak;
            //        idKasa.bakiye_tl = idKasa.girentutar_tl - idKasa.cikantutar_tl;
            //        try
            //        {
            //            _kasaRepository.Update(idKasa);

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }
            //    }
            //    if (model.para_birimi == "USD")
            //    {
            //        var kasaAlacak = (from u in _kasaHareketRepository.GetAll()
            //                          where u.kasa_id == model.kasa_id
            //                          select new EKisiDTO
            //                          {
            //                              alacak = u.kasa_alacak_dolar,
            //                          }).ToList();

            //        double kasatoplamAlacak = kasaAlacak.AsEnumerable().Sum(o => o.alacak);

            //        kasa idKasa = _kasaRepository.Find(hesapHareketIdModel.kasa_id);
            //        idKasa.girentutar_dolar = kasatoplamAlacak;
            //        idKasa.bakiye_dolar = idKasa.girentutar_dolar - idKasa.cikantutar_dolar;
            //        try
            //        {
            //            _kasaRepository.Update(idKasa);

            //        }
            //        catch (Exception ex)
            //        {

            //            throw ex;
            //        }
            //    }
            //    //euro yapılacak
            //    //if (model.para_birimi == "TL")
            //    //{
            //    //    var kasaAlacak = (from u in _hesapHareketRepository.GetAll()
            //    //                      where u.banka_id == model.banka_id && u.para_birimi == "TL"
            //    //                      select new EKisiDTO
            //    //                      {
            //    //                          alacak = u.alacak,
            //    //                      }).ToList();

            //    //    double kasatoplamAlacak = kasaAlacak.AsEnumerable().Sum(o => o.alacak);

            //    //    kasa idKasa = _kasaRepository.Find(hesapHareketIdModel.kasa_id);
            //    //    idKasa.girentutar_tl = kasatoplamAlacak;
            //    //    idKasa.bakiye_tl = idKasa.girentutar_tl - idKasa.cikantutar_tl;
            //    //    try
            //    //    {
            //    //        _kasaRepository.Update(idKasa);

            //    //    }
            //    //    catch (Exception ex)
            //    //    {

            //    //        throw ex;
            //    //    }
            //    //}
            //    _uow.SaveChanges();

            //}

            return true;
        }

        public EFinansalBorcDuzenle FinansBorcDuzenle(int id)
        {
            using (var context = new MyArchContext())
            {

                var finansBorcıd = context.Database.SqlQuery<EFinansalBorcDuzenle>("select borc.id,borc.para_birimi,aciklama,tarih,sonodeme_tarihi,isim,blok_adi,daire_numarasi,borclandirma_turu,borc.borc,kisi_id from " +
                    "hesap_hareket as borc left join kisiler as ks on ks.id=borc.kisi_id left join bagimsiz_bolumler as bs on bs.id=borc.bagimsiz_id WHERE islem_turu = 'borç dekontu' and borc.id='" + id + "' ORDER BY id DESC").SingleOrDefault();
                return finansBorcıd;
            }
        }



        public bool HesapHaraketSil(hesap_hareket model)
        {
            kisiler kisi = new kisiler();
            kisiler kisiIdModel = _kisilerRepository.Find(model.kisi_id);

            var hesapHaraketId = _hesapHareketRepository.Find(model.id);
            _hesapHareketRepository.Delete(hesapHaraketId);
            _uow.SaveChanges();



            var borcDolar = (from u in _hesapHareketRepository.GetAll()
                             where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "USD"
                             select new EKisiDTO
                             {
                                 borc = u.borc,
                             }).ToList();

            double toplamBorcDolar = borcDolar.AsEnumerable().Sum(o => o.borc);
            kisiIdModel.borc_dolar = toplamBorcDolar;
            _kisilerRepository.Update(kisiIdModel);



            var borcTl = (from u in _hesapHareketRepository.GetAll()
                          where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "TL"
                          select new EKisiDTO
                          {
                              borc = u.borc,
                          }).ToList();

            double toplamBorcTl = borcTl.AsEnumerable().Sum(o => o.borc);
            kisiIdModel.borc_tl = toplamBorcTl;
            _kisilerRepository.Update(kisiIdModel);

            //euro yapılacak
            //if (model.para_birimi == "TL")
            //{
            //    var borc = (from u in _hesapHareketRepository.GetAll()
            //                where u.kisi_id == model.kisi_id && u.islem_turu == "borç dekontu" && u.para_birimi == "TL"
            //                select new EKisiDTO
            //                {
            //                    borc = u.borc,
            //                }).ToList();

            //    double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
            //    kisiIdModel.borc_tl = toplamBorc;
            //    _kisilerRepository.Update(kisiIdModel);
            //}

            if (_uow.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EHesapHaraketDTO> TahsilatListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var alacakListe = context.Database.SqlQuery<EHesapHaraketDTO>("select borc.id,borc.para_birimi,borclandirma_turu,aciklama,tarih,sonodeme_tarihi,isim,blok_adi,daire_numarasi,borclandirma_turu,borc.alacak,islem,kisi_id,refno,kasa_id,banka_id from hesap_hareket as borc left join kisiler as ks on ks.id=borc.kisi_id left join bagimsiz_bolumler as bs on bs.id=borc.bagimsiz_id WHERE islem_turu = 'alacak dekontu'  ORDER BY id DESC").ToList();
                    return alacakListe;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public EFinansalTahsilatDuzenle FinansTahsilatDuzenle(string refno)
        {
            using (var context = new MyArchContext())
            {

                var finansTahsilcıd = context.Database.SqlQuery<EFinansalTahsilatDuzenle>("select borc.id,borc.para_birimi,bagimsiz_id,aciklama,islem,tarih,sonodeme_tarihi,isim,blok_adi,daire_numarasi,borclandirma_turu,borc.alacak,kisi_id,refno,kasa_id,banka_id,tahsilat_id from " +
                    "hesap_hareket as borc left join kisiler as ks on ks.id=borc.kisi_id left join bagimsiz_bolumler as bs on bs.id=borc.bagimsiz_id WHERE islem_turu = 'alacak dekontu' and borc.refno='" + refno + "' ORDER BY id DESC").SingleOrDefault();
                return finansTahsilcıd;
            }
        }

        public hesap_hareket HesapHareketId(int hhId)
        {
            var hesapHareket = _hesapHareketRepository.Find(hhId);
            return hesapHareket;
        }

        public double KalanBakiyeKontrol(hesap_hareket model)
        {
            double hhAlacak = 0;
            double tahsilatToplam = 0;
            double kalanBakiye = 0;
            using (var context = new MyArchContext())
            {
                //var tahsilatıdvarmi = _hesapHareketRepository.GetAll().Where(x => x.id == model.tahsilat_id).FirstOrDefault();

                var tahsilatıdvarmi = _hesapHareketRepository.GetAll().Where(x => x.tahsilat_id == model.id).FirstOrDefault();

                hhAlacak = context.Database.SqlQuery<double>("SELECT borc FROM hesap_hareket WHERE id = '" + model.id + "' ").SingleOrDefault();

                if (tahsilatıdvarmi == null)
                {
                    kalanBakiye = hhAlacak;
                }
                else
                {
                    tahsilatToplam = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE tahsilat_id = '" + model.id + "' ").SingleOrDefault();
                    kalanBakiye = hhAlacak - tahsilatToplam;
                }
            }


            return kalanBakiye;
        }

        public bool PesinTahsilatKaydet(hesap_hareket hhHareket)
        {
            kisiler kisi = new kisiler();
            kisiler kisiIdModel = _kisilerRepository.Find(hhHareket.kisi_id);

            kasa kasa = new kasa();
            kasa kasaModel = _kasaRepository.Find(hhHareket.kasa_id);

            banka banka = new banka();
            banka bankaModel = _bankaRepository.Find(hhHareket.banka_id);

            kasa_hareket kasa_hareket = new kasa_hareket();
            banka_hareket banka_hareket = new banka_hareket();

            Random rastgele = new Random();
            int sayi = rastgele.Next(100000, 999999);

            var refno = "TH" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;
            var refnoBorc = "TD" + DateTime.Now.ToString("ddMMyyyy") + "-" + sayi;



            double borc = 0;
            double alacak = 0;
            double kasaGiren = 0;
            double bankaGiren = 0;
            string hesapAdi;

            double hhAlacak = 0;
            double tahsilatToplam = 0;

            try
            {
                using (var context = new MyArchContext())
                {


                    //tediye yap
                    hhHareket.islem_tarihi = DateTime.Now;
                    hhHareket.saat = DateTime.Now.ToShortTimeString();
                    hesapAdi = context.Database.SqlQuery<string>("SELECT  isim FROM kisiler WHERE id = '" + hhHareket.kisi_id + "' ").SingleOrDefault();
                    hhHareket.hesap_adi = hesapAdi;
                    hhHareket.refno = refnoBorc;

                    hhHareket.tahsilat_durumu = "ödendi";
                    hhHareket.islem_turu = "borç dekontu";
                    hhHareket.alacak = 0;
                    hhHareket.bakiye = hhHareket.borc - hhHareket.alacak;
                    hhHareket.gun = hhHareket.gun;
                    hhHareket.ay = hhHareket.ay;
                    hhHareket.yil = hhHareket.yil;
                    var test = hhHareket.yil + "-" + hhHareket.ay + "-" + hhHareket.gun;
                    hhHareket.tarih = Convert.ToDateTime((hhHareket.yil + "-" + hhHareket.ay + "-" + hhHareket.gun));
                    hhHareket.pesin_tahsilat = "X";
                    //model.saat = model.tarih.Date;
                    _borclandirmaRepository.Insert(hhHareket);
                    _uow.SaveChanges();


                    if (hhHareket.borclandirma_turu == "Akaryakıt")
                    {

                        borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + hhHareket.kisi_id + "' and para_birimi = 'USD' ").SingleOrDefault();
                        kisiIdModel.borc_dolar = borc;
                        _kisilerRepository.Update(kisiIdModel);
                        _uow.SaveChanges();
                    }
                    else
                    {
                        borc = context.Database.SqlQuery<double>("SELECT SUM(borc)  FROM hesap_hareket WHERE kisi_id = '" + hhHareket.kisi_id + "' and para_birimi = 'TL' ").SingleOrDefault();
                        kisiIdModel.borc_tl = borc;
                        _kisilerRepository.Update(kisiIdModel);
                        _uow.SaveChanges();

                    }



                    //tahsilat yap
                    hhHareket.islem_tarihi = DateTime.Now;
                    hhHareket.saat = DateTime.Now.ToShortTimeString();
                    hhHareket.hesap_adi = hesapAdi;
                    hhHareket.refno = refno;
                    hhHareket.bakiye = hhHareket.alacak;
                    hhHareket.tahsilat_durumu = "";
                    hhHareket.islem_turu = "alacak dekontu";
                    hhHareket.alacak = hhHareket.borc;
                    hhHareket.borc = 0;
                    hhHareket.bakiye = hhHareket.borc - hhHareket.alacak;
                    hhHareket.ay = hhHareket.ay;
                    hhHareket.yil = hhHareket.yil;

                    _borclandirmaRepository.Insert(hhHareket);
                    _uow.SaveChanges();


                    if (hhHareket.borclandirma_turu == "Akaryakıt")
                    {

                        alacak = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE kisi_id = '" + hhHareket.kisi_id + "' and para_birimi='USD' ").SingleOrDefault();
                        kisiIdModel.alacak_dolar = alacak;
                        _kisilerRepository.Update(kisiIdModel);
                    }
                    else
                    {
                        alacak = context.Database.SqlQuery<double>("SELECT SUM(alacak)  FROM hesap_hareket WHERE kisi_id = '" + hhHareket.kisi_id + "' and para_birimi='TL' ").SingleOrDefault();
                        kisiIdModel.alacak_tl = alacak;
                        _kisilerRepository.Update(kisiIdModel);

                    }


                    if (hhHareket.islem == "Nakit")
                    {
                        if (hhHareket.para_birimi == "USD")
                        {
                            // kasa  tablosu update islemleri
                            kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(alacak) as toplam_alacak FROM hesap_hareket WHERE islem_turu = 'alacak dekontu' and islem = 'Nakit' and para_birimi='USD' ").SingleOrDefault();

                            kasaModel.girentutar_dolar = kasaGiren;
                            kasaModel.bakiye_dolar = kasaGiren - kasa.cikantutar_dolar;
                            _kasaRepository.Update(kasaModel);

                            //kasa haraket insert
                            kasa_hareket.hesap_adi = hesapAdi;
                            kasa_hareket.islem = hhHareket.islem;
                            kasa_hareket.islem_turu = hhHareket.islem_turu;
                            kasa_hareket.kasa_borc_dolar = hhHareket.alacak;
                            kasa_hareket.aciklama = hhHareket.aciklama;
                            kasa_hareket.bakiye = hhHareket.alacak - hhHareket.borc;
                            kasa_hareket.refno = refno;

                            _kasaHareketRepository.Insert(kasa_hareket);
                        }
                        else
                        {
                            // kasa  tablosu update islemleri
                            kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(alacak) as toplam_alacak FROM hesap_hareket WHERE islem_turu = 'alacak dekontu' and islem = 'Nakit' and para_birimi='TL' ").SingleOrDefault();

                            kasaModel.girentutar_tl = kasaGiren;
                            kasaModel.bakiye_tl = kasaGiren - kasa.cikantutar_tl;
                            _kasaRepository.Update(kasaModel);

                            //kasa haraket insert
                            kasa_hareket.hesap_adi = hesapAdi;
                            kasa_hareket.islem = hhHareket.islem;
                            kasa_hareket.islem_turu = hhHareket.islem_turu;
                            kasa_hareket.kasa_borc_tl = hhHareket.alacak;
                            kasa_hareket.aciklama = hhHareket.aciklama;
                            kasa_hareket.bakiye = hhHareket.alacak - hhHareket.borc;
                            kasa_hareket.refno = refno;

                            _kasaHareketRepository.Insert(kasa_hareket);
                        }
                    }
                    else
                    {
                        if (hhHareket.para_birimi == "USD")
                        {
                            bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(alacak) as toplam_alacak FROM hesap_hareket WHERE islem_turu = 'alacak dekontu' and islem = 'Kredi kartı' and para_birimi = 'USD' ").SingleOrDefault();

                            bankaModel.girentutar_dolar = bankaGiren;
                            bankaModel.bakiye_dolar = bankaGiren - banka.cikantutar_dolar;
                            _bankaRepository.Update(bankaModel);

                            //banka haraket insert
                            banka_hareket.hesap_adi = hesapAdi;
                            banka_hareket.islem = hhHareket.islem;
                            banka_hareket.islem_turu = hhHareket.islem_turu;
                            banka_hareket.banka_borc_dolar = hhHareket.alacak;
                            banka_hareket.aciklama = hhHareket.aciklama;
                            banka_hareket.bakiye = hhHareket.alacak - hhHareket.borc;
                            banka_hareket.refno = refno;

                            _bankaHareketRepository.Insert(banka_hareket);
                        }

                        else
                        {
                            bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(alacak) as toplam_alacak FROM hesap_hareket WHERE islem_turu = 'alacak dekontu' and islem = 'Kredi kartı' and para_birimi = 'TL' ").SingleOrDefault();

                            bankaModel.girentutar_tl = bankaGiren;
                            bankaModel.bakiye_tl = bankaGiren - banka.cikantutar_tl;
                            _bankaRepository.Update(bankaModel);

                            //banka haraket insert
                            banka_hareket.hesap_adi = hesapAdi;
                            banka_hareket.islem = hhHareket.islem;
                            banka_hareket.islem_turu = hhHareket.islem_turu;
                            banka_hareket.banka_alacak_tl = hhHareket.alacak;
                            banka_hareket.aciklama = hhHareket.aciklama;
                            banka_hareket.bakiye = hhHareket.alacak - hhHareket.borc;
                            banka_hareket.refno = refno;

                            _bankaHareketRepository.Insert(banka_hareket);
                        }
                    }

                    if (_uow.SaveChanges() > 0)
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

        public int GunSayisiGetir(string ay)
        {
            var gunSayisi = DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(ay));
            return gunSayisi;
        }

        public bool borclandirmaKontrol(toplu_borclandir model)
        {
            var borclandirmaKontrol = _topluBorclandirRepository.GetAll().Where(x => x.ay == model.ay && x.yil == model.yil && x.borclandirma_turu == model.borclandirma_turu && x.tip == model.tip).FirstOrDefault();
            if (borclandirmaKontrol != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string BorclandirmaTuru(int? id)
        {
            var tur = _hesapHareketRepository.GetAll().Where(x => x.id == id).FirstOrDefault().borclandirma_turu;
            return tur;
        }

        public bool TahsilatSil(hesap_hareket model)
        {
            double kasaGiren = 0;
            double bankaGiren = 0;



            kisiler kisi = new kisiler();

            try
            {
                //hesap haraket tahsilat ve kasa-banka hareket silme

                kisiler kisiIdModel = _kisilerRepository.Find(model.kisi_id);

                var hesapHaraketId = _hesapHareketRepository.Find(model.id);
                var tahsilatId = _hesapHareketRepository.GetAll().Where(x => x.id == model.id).SingleOrDefault().tahsilat_id;
                using (var context = new MyArchContext())
                {
                    int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE hesap_hareket SET tahsilat_durumu='ödenmedi' WHERE id = '" + tahsilatId + "' ");
                }

                _hesapHareketRepository.Delete(hesapHaraketId);
                _uow.SaveChanges();


                //kasa islemleri
                if (model.islem == "Nakit")
                {
                    kasa kasa = new kasa();
                    kasa kasaModel = _kasaRepository.Find(model.kasa_id);
                    using (var context = new MyArchContext())
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM kasa_hareket where refno = '" + model.refno + "'");

                    }

                    if (model.para_birimi == "USD")
                    {
                        var borc = (from u in _hesapHareketRepository.GetAll()
                                    where u.kisi_id == model.kisi_id && u.islem_turu == "alacak dekontu" && u.para_birimi == "USD"
                                    select new EKisiDTO
                                    {
                                        borc = u.alacak,
                                    }).ToList();

                        double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
                        //kisiIdModel.alacak_dolar = toplamBorc;
                        //_kisilerRepository.Update(kisiIdModel);
                        using (var context = new MyArchContext())
                        {
                            var toplamBorcSon = toplamBorc.ToString().Replace(",", ".");
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_dolar=' " + toplamBorcSon + "  ' WHERE id = '" + model.kisi_id + "' ");
                        }


                        // kasa  tablosu update islemleri
                        using (var context = new MyArchContext())
                        {
                            var liste = _kasaHareketRepository.GetAll().Where(x => x.kasa_id == model.kasa_id).ToList();
                            if (liste.Count > 0)
                            {
                                kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_dolar) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + model.kasa_id + "' ").SingleOrDefault();

                                kasaModel.girentutar_dolar = kasaGiren;
                                kasaModel.bakiye_dolar = kasaGiren - kasa.cikantutar_dolar;

                            }


                            else
                            {
                                kasaModel.girentutar_dolar = 0;
                                kasaModel.bakiye_dolar = 0;
                            }
                            _kasaRepository.Update(kasaModel);
                        }

                  
                    }

                    if (model.para_birimi == "TL")
                    {
                        var borc = (from u in _hesapHareketRepository.GetAll()
                                    where u.kisi_id == model.kisi_id && u.islem_turu == "alacak dekontu" && u.para_birimi == "TL"
                                    select new EKisiDTO
                                    {
                                        borc = u.alacak,
                                    }).ToList();

                        double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
                        //kisiIdModel.alacak_tl = toplamBorc;
                        //_kisilerRepository.Update(kisiIdModel);

                        using (var context = new MyArchContext())
                        {
                            var toplamBorcSon = toplamBorc.ToString().Replace(",", ".");
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_tl=' " + toplamBorcSon + "  ' WHERE id = '" + model.kisi_id + "' ");
                        }


                        using (var context = new MyArchContext())
                        {

                            var liste = _kasaHareketRepository.GetAll().Where(x => x.kasa_id == model.kasa_id).ToList();

                            if (liste.Count > 0)
                            {
                                kasaGiren = context.Database.SqlQuery<double>("SELECT SUM(kasa_alacak_tl) as toplam_alacak FROM kasa_hareket WHERE kasa_id='" + model.kasa_id + "' ").SingleOrDefault();

                                kasaModel.girentutar_tl = kasaGiren;
                                kasaModel.bakiye_tl = kasaGiren - kasa.cikantutar_tl;
                            }


                            else
                            {
                                kasaModel.girentutar_tl = 0;
                                kasaModel.bakiye_tl = 0;
                            }
                            _kasaRepository.Update(kasaModel);
                        }

                    }
                    //euro yapılacak
                    //if (model.para_birimi == "TL")
                    //{
                    //    var borc = (from u in _hesapHareketRepository.GetAll()
                    //                where u.kisi_id == model.kisi_id && u.islem_turu == "Tahsilat" && u.para_birimi == "TL"
                    //                select new EKisiDTO
                    //                {
                    //                    borc = u.borc,
                    //                }).ToList();

                    //    double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
                    //    kisiIdModel.borc_tl = toplamBorc;
                    //    _kisilerRepository.Update(kisiIdModel);
                    //}
                }

                //banka islemleri
                else
                {
                    banka banka = new banka();
                    banka bankaModel = _bankaRepository.Find(model.banka_id);

                    using (var context = new MyArchContext())
                    {
                        context.Database.ExecuteSqlCommand("DELETE FROM banka_hareket where refno = '" + model.refno + "'");

                    }

                    if (model.para_birimi == "USD")
                    {
                        var borc = (from u in _hesapHareketRepository.GetAll()
                                    where u.kisi_id == model.kisi_id && u.islem_turu == "alacak dekontu" && u.para_birimi == "USD"
                                    select new EKisiDTO
                                    {
                                        borc = u.alacak,
                                    }).ToList();

                        double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
                        //kisiIdModel.alacak_dolar = toplamBorc;
                        //_kisilerRepository.Update(kisiIdModel);

                        using (var context = new MyArchContext())
                        {
                            var toplamBorcSon = toplamBorc.ToString().Replace(",", ".");
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_dolar=' " + toplamBorcSon + "  ' WHERE id = '" + model.kisi_id + "' ");
                        }


                        using (var context = new MyArchContext())
                        {
                            //para birimi usd olan kayıt varsa
                            var liste = _bankaHareketRepository.GetAll().Where(x => x.banka_id == model.banka_id).ToList();

                            if (liste.Count > 0)
                            {
                                bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_dolar) as toplam_alacak FROM banka_hareket WHERE banka_id='" + model.banka_id + "' ").SingleOrDefault();

                                bankaModel.girentutar_dolar = bankaGiren;
                                bankaModel.bakiye_dolar = bankaGiren - banka.cikantutar_dolar;
                            }


                            else
                            {
                                bankaModel.girentutar_dolar = 0;
                                bankaModel.bakiye_dolar = 0;
                            }

                            _bankaRepository.Update(bankaModel);

                        }
                    }

                    if (model.para_birimi == "TL")
                    {
                        var borc = (from u in _hesapHareketRepository.GetAll()
                                    where u.kisi_id == model.kisi_id && u.islem_turu == "alacak dekontu" && u.para_birimi == "TL"
                                    select new EKisiDTO
                                    {
                                        borc = u.alacak,
                                    }).ToList();

                        double toplamBorc = borc.AsEnumerable().Sum(o => o.borc);
                        //kisiIdModel.alacak_tl = toplamBorc;
                        //_kisilerRepository.Update(kisiIdModel);

                        using (var context = new MyArchContext())
                        {
                            var toplamBorcSon = toplamBorc.ToString().Replace(",", ".");
                            int aciklamaGuncelle = context.Database.ExecuteSqlCommand("UPDATE kisiler SET alacak_tl=' " + toplamBorcSon + "  ' WHERE id = '" + model.kisi_id + "' ");
                        }


                        using (var context = new MyArchContext())
                        {
                            var liste = _bankaHareketRepository.GetAll().Where(x => x.banka_id == model.banka_id).ToList();

                            if (liste.Count > 0)
                            {
                                bankaGiren = context.Database.SqlQuery<double>("SELECT SUM(banka_alacak_tl) as toplam_alacak FROM banka_hareket WHERE banka_id='" + model.banka_id + "' ").SingleOrDefault();

                                bankaModel.girentutar_tl = bankaGiren;
                                bankaModel.bakiye_tl = bankaGiren - banka.cikantutar_tl;
                            }

                            else
                            {
                                bankaModel.girentutar_tl = 0;
                                bankaModel.bakiye_tl = 0;
                            }
                            _bankaRepository.Update(bankaModel);
                        }

                    }


                }

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

        public List<hesap_hareket> TumBorcListesi()
        {


            var liste = _hesapHareketRepository.GetAll().Where(x => x.tahsilat_durumu == "eksik tahsilat" || x.tahsilat_durumu == "ödenmedi" && DateTime.Now > x.sonodeme_tarihi && x.yil == DateTime.Now.Year.ToString()).ToList();

            return liste;
        }


        //gönderilen refnoya ait tahsilat kontrolu yapar
        public bool TahsilatKontrolRefno(string refno)
        {
            var tahsilatlar = _hesapHareketRepository.GetAll().Where(x => x.refno == refno && x.tahsilat_durumu != "ödenmedi").FirstOrDefault();
            if (tahsilatlar == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool TahsilatKontrolId(int id)
        {
            var tahsilatlar = _hesapHareketRepository.GetAll().Where(x => x.id == id && x.tahsilat_durumu != "ödenmedi").FirstOrDefault();
            if (tahsilatlar == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool BorcKontrolIdKisi(int kisiId)
        {
            var borclar = _hesapHareketRepository.GetAll().Where(x => x.kisi_id == kisiId && x.tahsilat_durumu == "ödenmedi" && x.islem_turu == "borç dekontu").FirstOrDefault();

            if (borclar == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool TahsilatKontrolIdKisi(int kisiId)
        {
            var tahsilatlar = _hesapHareketRepository.GetAll().Where(x => x.kisi_id == kisiId && x.tahsilat_durumu != "ödenmedi" && x.islem_turu == "alacak dekontu").FirstOrDefault();

            if (tahsilatlar == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AkaryakitDonemKontrol(int kisi_id)
        {
            var kontrol = _hesapHareketRepository.GetAll().Where(x=>x.yil ==DateTime.Now.Year.ToString() && x.kisi_id == kisi_id && x.donem == "1" && x.borclandirma_turu == "Akaryakıt" && x.islem_turu == "alacak dekontu").FirstOrDefault();

            if (kontrol != null )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<EHesapHaraketDTO> Yillar()
        {
            List<EHesapHaraketDTO> yillar = new List<EHesapHaraketDTO>();
            using (var context = new MYARCH.DATA.Context.MyArchContext())
            {
                yillar = context.Database.SqlQuery<EHesapHaraketDTO>("SELECT yil FROM hesap_hareket WHERE tahsilat_durumu = 'ödendi'  GROUP BY yil ").ToList();
            }
            return yillar;
        }
    }
}
