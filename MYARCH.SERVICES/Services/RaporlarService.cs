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
    public class RaporlarService:IRaporlarService
    {
        private readonly IGenericRepository<hesap_hareket> _hesapHareketRepository;
        private readonly IUnitofWork _uow;
        private EBorcluListesiDTO _borcluListesi;

        public RaporlarService(UnitofWork uow)
        {
            _uow = uow;
            _hesapHareketRepository = _uow.GetRepository<hesap_hareket>();
            _borcluListesi = new EBorcluListesiDTO();
        }

        public List<EBorcluListesiDTO> AidatVadesiGecenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')   and (GETDATE()>sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE))and hh.borclandirma_turu = 'Aidat'").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> DemirbasVadesiGecenBorcluListesi()
        {

            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')   and (GETDATE()>sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE))and hh.borclandirma_turu = 'Demirbaş'").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> YakitVadesiGecenBorcluListesi()
        {

            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')   and (GETDATE()>sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE))and hh.borclandirma_turu = 'Akaryakıt'").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> VadesiGecenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')   and (GETDATE()>sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE))").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> VadesiBekleyenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat') and (GETDATE()<sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE)) ").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> AidatVadesiBekleyenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,kisi_id,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')  and hh.borclandirma_turu = 'Aidat' and (GETDATE()<sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE)) ").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> DemirbasVadesiBekleyenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,kisi_id,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')  and hh.borclandirma_turu = 'Demirbaş' and (GETDATE()<sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE)) ").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EBorcluListesiDTO> YakitVadesiBekleyenBorcluListesi()
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var borcListesi = context.Database.SqlQuery<EBorcluListesiDTO>("SELECT hh.id,refno,kisi_id,aciklama,tarih,sonodeme_tarihi,borc,hesap_adi,borclandirma_turu,daire_numarasi,tahsilat_durumu FROM hesap_hareket as hh LEFT JOIN bagimsiz_bolumler as bb ON hh.bagimsiz_id = bb.id WHERE (hh.tahsilat_durumu = 'ödenmedi' or hh.tahsilat_durumu  = 'eksik tahsilat')  and hh.borclandirma_turu = 'Akaryakıt' and (GETDATE()<sonodeme_tarihi and sonodeme_tarihi != cast (GETDATE() as DATE)) ").ToList();
                    return borcListesi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
