using MYARCH.CORE.Entities;
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
    public class FirmaService : IFirmaService
    {

        private readonly IGenericRepository<firmalar> _firmalarRepository;
        private readonly IUnitofWork _uow;
     
        public FirmaService(UnitofWork uow)
        {
            _uow = uow;
            _firmalarRepository = _uow.GetRepository<firmalar>();
         
        }

        public firmalar FirmaGetir(firmalar model)
        {
            try
            {
                var gelenFirma = _firmalarRepository.Find(model.id);
                return gelenFirma;
            }
            catch (Exception msg)
            {
                throw msg;
            }
        }

        public bool FirmaGuncelle(firmalar model)
        {
            try
            {
                var gelenBlok = _firmalarRepository.Find(model.id);

                AutoMapper.Mapper.DynamicMap(model, gelenBlok);

                _firmalarRepository.Update(gelenBlok);
                if (_uow.SaveChanges() != 0)
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

        public bool FirmaKaydet(firmalar model)
        {
            try
            {
                _firmalarRepository.Insert(model);
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

        public List<firmalar> FirmaListesi()
        {
            try
            {
                var firmaListesi = _firmalarRepository.GetAll().ToList();
                return firmaListesi;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool FirmaSil(int id)
        {
            try
            {
                var gelenFirma = _firmalarRepository.Find(id);
                _firmalarRepository.Delete(gelenFirma);
                if (_uow.SaveChanges()>0)
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
    }
}
