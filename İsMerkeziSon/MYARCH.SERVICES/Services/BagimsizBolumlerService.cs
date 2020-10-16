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
    public class BagimsizBolumlerService : IBagimsizBolumlerService
    {

        private readonly IGenericRepository<bagimsiz_bolumler> _bagimsizBolumlerRepository;
        private readonly IGenericRepository<bagimsiz_bolum_kisiler> _bagimsizBolumKisilerRepository;
        private readonly IGenericRepository<hesap_hareket> _hesapHareketRepository;
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IUnitofWork _uow;
        private EBagimsizBolumlerDTO _bagimsizBolumlerDTO;
        public BagimsizBolumlerService(UnitofWork uow)
        {
            _uow = uow;
            _bagimsizBolumlerRepository = _uow.GetRepository<bagimsiz_bolumler>();
            _bagimsizBolumKisilerRepository = _uow.GetRepository<bagimsiz_bolum_kisiler>();
            _hesapHareketRepository = _uow.GetRepository<hesap_hareket>();
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _bagimsizBolumlerDTO = new EBagimsizBolumlerDTO();
        }

        public bool BagimsizAdresNoKontrol(string adresNo, int id)
        {
            var daireAdresNo = _bagimsizBolumlerRepository.GetAll().Where(x => x.daire_adres_no == adresNo && x.id != id).FirstOrDefault();

            if (daireAdresNo != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public EBagimsizBolumlerDTO BagimsizBolumBilgileriGetir(int id)
        {
            var control = (from u in _bagimsizBolumlerRepository.GetAll()
                           where u.id == id
                           select new EBagimsizBolumlerDTO
                           {
                               id = u.id,
                               borc_toplam = u.borc_toplam,
                               aidat_tutari = u.aidat_tutari,
                               kat = u.kat,
                               tip = u.tip,
                               brut_alan = u.brut_alan,
                               net_alan = u.brut_alan,
                               arsa_payı = u.arsa_payı,
                               durumu = u.durumu,
                               daire_numarasi = u.daire_numarasi,
                               su_abone_no = u.su_abone_no,
                               elektrik_abone_no = u.elektrik_abone_no,
                               daire_adres_no = u.daire_adres_no,
                               blok_adi = u.blok_adi,
                               adres = u.daire_adres_no,
                               petek_boyu = u.petek_boyu,

                           }).SingleOrDefault();
            return control;
        }

        public bool BagimsizBolumBlokKaydet(bagimsiz_bolumler model)
        {
            try
            {
                int sayac = Convert.ToInt32(model.daire_numarasi);

                for (int i = 1; i < sayac + 1; i++)
                {
                    model.daire_numarasi = i.ToString();
                    _bagimsizBolumlerRepository.Insert(model);
                    _uow.SaveChanges();
                }
                if (sayac != 0)
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

        public bool BagimsizBolumGuncelle(bagimsiz_bolumler model)
        {
            var gelenBagimsizBolum = _bagimsizBolumlerRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenBagimsizBolum);

            _bagimsizBolumlerRepository.Update(gelenBagimsizBolum);

            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool BagimsizBolumKisilerKaydet(bagimsiz_bolum_kisiler model, string isim, int bagimsizId, int? katmaliki_id, int? kiraci_id)
        {

            try
            {
                model.aktif = "True";

                //if (model.tip == "Kat maliki" && model.oturan_katmaliki == "True")
                //{
                //    model.kiraci = "var";
                //}



                if (model.tip == "Kiracı")
                {

                    using (var context = new MyArchContext())
                    {
                        var kiracisizKisi = context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolum_kisiler set kiraci='var' WHERE bagimsiz_id='" + bagimsizId + "' and tip = 'Kat maliki' ");
                    }
                }

                _bagimsizBolumKisilerRepository.Insert(model);

                if (_uow.SaveChanges() > 0)
                {
                    if (model.tip == "Kat maliki")
                    {
                        using (var context = new MyArchContext())
                        {
                            var guncelleme = context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolumler set katmaliki='" + isim + "',katmaliki_id = '" + katmaliki_id + "'  WHERE id='" + bagimsizId + "' ");
                           // var guncellemeKisi = context.Database.ExecuteSqlCommand("UPDATE kisiler set durumu='Kat maliki' WHERE id='" + katmaliki_id + "' ");

                        }
                    }

                    else
                    {
                        using (var context = new MyArchContext())
                        {
                            var guncelleme = context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolumler set kiracı='" + isim + "',kiraci_id = '" + kiraci_id + "' WHERE id='" + bagimsizId + "' ");
                            // var guncellemeKisi = context.Database.ExecuteSqlCommand("UPDATE kisiler set durumu='Kiracı' WHERE id='" + kiraci_id + "' ");
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

        public List<EBagimsizBolumlerDTO> BagimsizBolumler()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var bagimsizBolumler = context.Database.SqlQuery<EBagimsizBolumlerDTO>("select id,kiraci_id,katmaliki_id,blok_adi,daire_numarasi,katmaliki,kiracı,toplam,su_abone_no,elektrik_abone_no,petek_boyu,kat from bagimsiz_bolumler as bb left join (select sum(borc-alacak) as toplam,bagimsiz_id from hesap_hareket group by bagimsiz_id) as brc on bb.id=brc.bagimsiz_id ").ToList();
                    return bagimsizBolumler;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBagimsizDetayKisiler> BagimsizBolumlerKisiler(int id, string paraBirimi)
        {
            try
            {
                if (paraBirimi == "TL" || paraBirimi == null)
                {
                    using (var context = new MyArchContext())
                    {

                        var kisiler = context.Database.SqlQuery<EBagimsizDetayKisiler>("SELECT borc_tl - alacak_tl as bakiye, kisi_id, bagimsiz_bolum_kisiler.tip,isim,tel1,giris_tarihi,cikis_tarihi,hisse_payi FROM bagimsiz_bolum_kisiler LEFT JOIN kisiler ON bagimsiz_bolum_kisiler.kisi_id = kisiler.id   LEFT JOIN bagimsiz_bolumler ON bagimsiz_bolum_kisiler.bagimsiz_id = bagimsiz_bolumler.id WHERE bagimsiz_bolum_kisiler.bagimsiz_id = '" + id + "'").ToList();
                        return kisiler;
                    }
                }
                else
                {
                    using (var context = new MyArchContext())
                    {

                        var kisiler = context.Database.SqlQuery<EBagimsizDetayKisiler>("SELECT borc_dolar - alacak_dolar as bakiye, kisi_id, bagimsiz_bolum_kisiler.tip,isim,tel1,giris_tarihi,cikis_tarihi,hisse_payi FROM bagimsiz_bolum_kisiler LEFT JOIN kisiler ON bagimsiz_bolum_kisiler.kisi_id = kisiler.id   LEFT JOIN bagimsiz_bolumler ON bagimsiz_bolum_kisiler.bagimsiz_id = bagimsiz_bolumler.id WHERE bagimsiz_bolum_kisiler.bagimsiz_id = '" + id + "'").ToList();
                        return kisiler;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool BagimsızBolumDetayKontrol(int bagimsizId)
        {
            var bagimsizBolumDetay = _bagimsizBolumlerRepository.GetAll().Where(x => x.id == bagimsizId).FirstOrDefault();

            var daireAdresNo = bagimsizBolumDetay.daire_adres_no;

            //dairesi adres no zorunlu oldugundan ona göre kontrol yapıldı
            if (daireAdresNo != null)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool BorcKontrol(int kisiId, int bagimsizId)
        {
            var toplamBorc = _hesapHareketRepository.GetAll().Where(x => x.kisi_id == kisiId && x.bagimsiz_id == bagimsizId).ToList();
            var toplamBorcToplam = toplamBorc.AsEnumerable().Sum(x => x.bakiye);

            if (toplamBorcToplam > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CikisTarihiGuncelle(bagimsiz_bolum_kisiler model)
        {

            try
            {
                //bagimsiz bolum kisiler durumu false yapma islemi
                var kisiModelFind = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.kisi_id == model.kisi_id && x.bagimsiz_id == model.bagimsiz_id).FirstOrDefault();
                kisiModelFind.cikis_tarihi = model.cikis_tarihi;

                kisiModelFind.aktif = "False";
                _bagimsizBolumKisilerRepository.Update(kisiModelFind);

                ////bagimsiz bolumler durumu boş yap
                //var bagimsModelFind = _bagimsizBolumlerRepository.GetAll().Where(x => x.kiraci_id == model.kisi_id && x.id == model.bagimsiz_id).FirstOrDefault();
                //bagimsModelFind.durumu = "boş";
                //_bagimsizBolumlerRepository.Update(bagimsModelFind);

                //kisiler tablosu durum guncelleme
                //var kisiModelDurumFind = _kisilerRepository.GetAll().Where(x => x.id == model.kisi_id).FirstOrDefault();
                //kisiModelDurumFind.durumu = "Çıkış";
                //_kisilerRepository.Update(kisiModelDurumFind);

                //kisiModelFind.aktif = "False";
                //_bagimsizBolumKisilerRepository.Update(kisiModelFind);


                //bagimsiz bolumlerde kat maliki ve kiracı silme
                if (model.tip == "Kiracı")
                {
                    var bagimsizBolumKiraci = _bagimsizBolumlerRepository.GetAll().Where(x => x.kiraci_id == model.kisi_id && x.id == model.bagimsiz_id).FirstOrDefault();
                    bagimsizBolumKiraci.kiracı = null;
                    bagimsizBolumKiraci.kiraci_id = null;
                    _bagimsizBolumlerRepository.Update(bagimsizBolumKiraci);

                    //bagimsiz bolumler kisiler kiracı varı null yap
                    var bagimsModelKisiFind = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.bagimsiz_id == model.bagimsiz_id && x.tip == "Kat maliki").FirstOrDefault();
                    bagimsModelKisiFind.kiraci = null;
                    _bagimsizBolumKisilerRepository.Update(bagimsModelKisiFind);
                }

                if (model.tip == "Kat maliki")
                {
                    var bagimsizBolumKatMaliki = _bagimsizBolumlerRepository.GetAll().Where(x => x.katmaliki_id == model.kisi_id && x.id == model.bagimsiz_id).FirstOrDefault();
                    bagimsizBolumKatMaliki.katmaliki = null;
                    bagimsizBolumKatMaliki.katmaliki_id = null;
                    _bagimsizBolumlerRepository.Update(bagimsizBolumKatMaliki);


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
            catch (Exception msg)
            {

                throw msg;
            }

        }

        public bool KatMalikiKiracimi(int bagimsiz_id)
        {
            try
            {
                var sonuc = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.bagimsiz_id == bagimsiz_id && x.tip == "Kat maliki" && x.aktif == "True").FirstOrDefault();
                if (sonuc != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        public bool KatMalikiKontrol(int bagimsiz_id, string tip)
        {
            try
            {
                var kisiVarmi = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.bagimsiz_id == bagimsiz_id && x.tip == "Kat maliki" && x.aktif == "True").FirstOrDefault();
                if (kisiVarmi != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        public bool KiraciKontrol(int bagimsiz_id, string tip)
        {

            try
            {
                var kisiVarmi = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.bagimsiz_id == bagimsiz_id && x.tip == "Kiracı" && x.aktif == "True").FirstOrDefault();
                if (kisiVarmi != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        //public bool KatMalikiVarmi(int bagimsiz_id)
        //{
        //    try
        //    {
        //        var kisiVarmi = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.bagimsiz_id == bagimsiz_id && x.aktif == "True").FirstOrDefault();
        //        if (kisiVarmi == null)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception msg)
        //    {
        //        throw msg;
        //    }
        //}

        public bool BagimsizKisiSil(hesap_hareket model, string tip)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    
                    //bagimsiz bolum kisi sil
                    context.Database.ExecuteSqlCommand("DELETE FROM bagimsiz_bolum_kisiler where bagimsiz_id = '" + model.bagimsiz_id + "' and kisi_id = '"+model.kisi_id+"' ");


                    if (tip == "Kat maliki")
                    {
                        context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolumler SET katmaliki='"+null+"',katmaliki_id='"+null+"' where katmaliki_id = '" + model.kisi_id + "' and  id = '" + model.bagimsiz_id + "' ");
                    }
                    else
                    {
                        context.Database.ExecuteSqlCommand("UPDATE bagimsiz_bolumler SET kiracı='"+null+"',kiraci_id='"+null+"' where kiraci_id = '" + model.kisi_id + "' and  id = '" + model.bagimsiz_id + "' ");
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

          
        }

    }
}
