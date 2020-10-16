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
    public class AnaSayacOrtakDagitimService : IAnaSayacOrtakDagitimService
    {

        private readonly IGenericRepository<anasayac_ortak_dagitim> _ortakAlanRepository;
        private readonly IUnitofWork _uow;

        public AnaSayacOrtakDagitimService(UnitofWork uow)
        {
            _uow = uow;
            _ortakAlanRepository = _uow.GetRepository<anasayac_ortak_dagitim>();

        }

        public List<anasayac_ortak_dagitim> OrtakAlanGetir()
        {
            var ortakAlan = _ortakAlanRepository.GetAll().ToList();
            return ortakAlan;
        }

        public bool OrtakAlanGuncelle(anasayac_ortak_dagitim model)
        {
            var gelenOrtak = _ortakAlanRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenOrtak);

            _ortakAlanRepository.Update(gelenOrtak);
            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool OrtakAlanKaydet(anasayac_ortak_dagitim model)
        {
            try
            {
                _ortakAlanRepository.Insert(model);
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

        public bool OrtakAlanSil(anasayac_ortak_dagitim model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM anasayac_ortak_dagitim where id = '" + model.id + "'") > 0)
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
