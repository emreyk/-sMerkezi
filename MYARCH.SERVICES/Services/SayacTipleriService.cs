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
    public class SayacTipleriService : ISayacTipleriService
    {


        private readonly IGenericRepository<sayac_tipleri> _sayacTipleriRepository;
        private readonly IUnitofWork _uow;

        public SayacTipleriService(UnitofWork uow)
        {
            _uow = uow;
            _sayacTipleriRepository = _uow.GetRepository<sayac_tipleri>();

        }

        public bool SayacTipiGuncelle(sayac_tipleri model)
        {
            var gelenBlok = _sayacTipleriRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenBlok);

            _sayacTipleriRepository.Update(gelenBlok);
            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SayacTipiKaydet(sayac_tipleri model)
        {
            try
            {
                _sayacTipleriRepository.Insert(model);
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

        public bool SayacTipiSil(sayac_tipleri model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM sayac_tipleri where id = '" + model.id + "'") > 0)
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

        public List<sayac_tipleri> SayacTipleri()
        {
            var sayacTipleri = _sayacTipleriRepository.GetAll().ToList();
            return sayacTipleri;
        }
    }
}
