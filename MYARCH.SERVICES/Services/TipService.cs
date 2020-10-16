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
    public class TipService : ITipService
    {

        private readonly IGenericRepository<tipler> _tiplerRepository;
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IGenericRepository<hesap_hareket> _hesapHareketRepository;
        private readonly IUnitofWork _uow;
        private ETiplerDTO _tipDTO;
        public TipService(UnitofWork uow)
        {
            _uow = uow;
            _tiplerRepository = _uow.GetRepository<tipler>();
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _hesapHareketRepository = _uow.GetRepository<hesap_hareket>();
            _tipDTO = new ETiplerDTO();
        }

        public bool AidatTahsilatKontrol()
        {
            var aidatTahsilatVarmi = _hesapHareketRepository.GetAll().Where(x => x.islem_turu == "alacak dekontu" && x.borclandirma_turu == "Aidat").ToList();
            if (aidatTahsilatVarmi.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<ETiplerDTO> TipGetir()
        {
            var tipGetir = (from u in _tiplerRepository.GetAll()

                            select new ETiplerDTO
                            {
                                id = u.id,
                                bagimsiz_tip = u.bagimsiz_tip,
                                aidat_tutar = u.aidat_tutar,
                                para_birimi = u.para_birimi,
                                yil = u.yil


                            }).OrderByDescending(x => x.id).ToList();
            return tipGetir;
        }

        public tipler TipGetirId(int id)
        {
            try
            {
                var tipGelen = _tiplerRepository.Find(id);
                return tipGelen;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }

        public List<ETiplerDTO> TipGetirYıl()
        {
            var tipGetir = (from u in _tiplerRepository.GetAll().Where(x => x.yil == DateTime.Now.Year.ToString())

                            select new ETiplerDTO
                            {
                                id = u.id,
                                bagimsiz_tip = u.bagimsiz_tip,
                                aidat_tutar = u.aidat_tutar,
                                para_birimi = u.para_birimi,
                                yil = u.yil


                            }).OrderByDescending(x => x.id).ToList();
            return tipGetir;
        }

        public bool TipGuncelle(ETiplerDTO model)
        {
            List<hesap_hareket> aidatBorclandirmasi = new List<hesap_hareket>();
            using (var context = new MyArchContext())
            {
                 aidatBorclandirmasi = context.Database.SqlQuery <hesap_hareket>("SELECT * FROM hesap_hareket LEFT JOIN bagimsiz_bolumler ON hesap_hareket.bagimsiz_id = bagimsiz_bolumler.id WHERE bagimsiz_bolumler.tip ='"+model.bagimsiz_tip+"' and hesap_hareket.yil = '"+DateTime.Now.Year+ "' and hesap_hareket.para_birimi = 'TL' ").ToList();
            }
             
            try
            {
                foreach (var item in aidatBorclandirmasi)
                {
                    kisiler kisi = new kisiler();
                    using (var context = new MyArchContext())
                    {

                        kisiler kisiIdModel = _kisilerRepository.Find(item.kisi_id);

                        //hesap hareket aidat borçlandırması güncelleme işlemi
                        int hhGuncelle = context.Database.ExecuteSqlCommand("update hesap_hareket set borc='" + model.aidat_tutar + "',bakiye='"+model.aidat_tutar+"' where id = '" + item.id + "'");

                        //kisi güncelle
                        if (item.para_birimi == "TL")
                        {
                            var borc = (from u in _hesapHareketRepository.GetAll().Where(x=>x.para_birimi == "TL")
                                        where u.kisi_id == item.kisi_id
                                        select new EKisiDTO
                                        {
                                            borc_tl = u.borc,
                                        }).ToList();

                            double toplamAlacak = borc.AsEnumerable().Sum(o => o.borc_tl);
                            kisiIdModel.borc_tl = toplamAlacak;

                            _kisilerRepository.Update(kisiIdModel);
                            _uow.SaveChanges();

                        }
                        if (item.para_birimi == "USD")
                        {
                            var borc = (from u in _hesapHareketRepository.GetAll().Where(x => x.para_birimi == "USD")
                                        where u.kisi_id == item.kisi_id
                                        select new EKisiDTO
                                        {
                                            borc_dolar = u.borc,
                                        }).ToList();

                            double toplamAlacak = borc.AsEnumerable().Sum(o => o.borc_dolar);
                            kisiIdModel.borc_dolar = toplamAlacak;

                            _kisilerRepository.Update(kisiIdModel);
                            _uow.SaveChanges();

                        }
                        //euro yapılacak
                        //if (item.para_birimi == "USD")
                        //{
                        //    var borc = (from u in _hesapHareketRepository.GetAll()
                        //                where u.kisi_id == item.kisi_id
                        //                select new EKisiDTO
                        //                {
                        //                    borc_dolar = u.borc,
                        //                }).ToList();

                        //    double toplamAlacak = borc.AsEnumerable().Sum(o => o.borc_dolar);
                        //    kisiIdModel.borc_dolar = toplamAlacak;
                        //}
                    }
                }

                    using (var context = new MyArchContext())
                    {
                        int tipGuncelle = context.Database.ExecuteSqlCommand("update tipler set aidat_tutar='" + model.aidat_tutar + "' where id = '" + model.id + "'");

                        int bbAidatGuncelle = context.Database.ExecuteSqlCommand("update bagimsiz_bolumler set aidat_tutari='" + model.aidat_tutar + "' where tip_id = '" + model.id + "'");

                    if (tipGuncelle>0 && bbAidatGuncelle >0)
                    {
                        return true;
                    }
                    else return false;
                    }

                   
               
              
            }
            catch (Exception)
            {

                throw;
            }


        }

        public bool TipKaydet(tipler model)
        {
            try
            {
                _tiplerRepository.Insert(model);
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

        public bool TipSil(tipler model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM yakit where id = '" + model.id + "'") > 0)
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


    }
}
