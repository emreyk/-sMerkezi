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
    public class BorcTipleriService : IBorcTipleri
    {
        private readonly IGenericRepository<borc_tipleri> _borcTipleri;
        private readonly IUnitofWork _uow;

        public BorcTipleriService(UnitofWork uow)
        {
            _uow = uow;
            _borcTipleri = _uow.GetRepository<borc_tipleri>();

        }

        public string AidatParaBirimiKontrol()
        {
            try
            {
                var paraBirimi = _borcTipleri.GetAll().Where(x => x.para_birimi == "Aidat").SingleOrDefault().para_birimi;
                if (paraBirimi != null)
                {
                    return paraBirimi;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool borcTipiSil(borc_tipleri model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM borc_tipleri where id = '" + model.id + "'") > 0)
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

        public List<borc_tipleri> BorcTipleriGetir()
        {
            var borcTipleri = _borcTipleri.GetAll().ToList();
            return borcTipleri;
        }

        public bool BorcTipleriKaydet(borc_tipleri model)
        {
            try
            {
                _borcTipleri.Insert(model);
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

        public string ParaBirimiAidat()
        {
            try
            {
                var aidatParaBirimi = _borcTipleri.GetAll().Where(x => x.borclandirma_turu == "Aidat").FirstOrDefault();
                if (aidatParaBirimi != null)
                {
                    return aidatParaBirimi.para_birimi;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ParaBirimiAkaryakit()
        {
            try
            {
                var akaryakitParaBirimi = _borcTipleri.GetAll().Where(x => x.borclandirma_turu == "Akaryakıt").FirstOrDefault().para_birimi;
                return akaryakitParaBirimi;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string paraBirimiGetir(string borcTipi)
        {
            var paraBirmiDeger = _borcTipleri.GetAll().Where(x => x.borclandirma_turu ==borcTipi).FirstOrDefault().para_birimi;
            return paraBirmiDeger;
        }
    }
}
