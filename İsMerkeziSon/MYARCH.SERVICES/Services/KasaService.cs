using MYARCH.CORE.Entities;
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
    public class KasaService : IKasaService
    {
        private readonly IGenericRepository<kasa> _kasaRepository;
        private readonly IGenericRepository<kasa_hareket> _kasaHareketRepository;
        private readonly IUnitofWork _uow;

        public KasaService(IUnitofWork uow)
        {
            _uow = uow;
            _kasaRepository = _uow.GetRepository<kasa>();
            _kasaHareketRepository = _uow.GetRepository<kasa_hareket>();
        }

        public kasa KasaBilgileri(int id)
        {
            var kasaBilgileri = _kasaRepository.GetAll().Where(u => u.id == id).SingleOrDefault();
            return kasaBilgileri;
        }

        public EKasaDTO KasaDTOGetir()
        {
            throw new NotImplementedException();
        }

        public List<kasa> KasaGetir()
        {
            var kasalar = _kasaRepository.GetAll().ToList();
            return kasalar;
        }

        public bool KasaGuncelle(kasa model)
        {
            var gelenKasa = _kasaRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenKasa);

            _kasaRepository.Update(gelenKasa);
            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    
        public bool KasaKaydet(kasa model)
        {
            _kasaRepository.Insert(model);

            int sonuc = _uow.SaveChanges();

            if (sonuc != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<kasa_hareket> KasaHareket()
        {
            try
            {
                var kasaHareket = _kasaHareketRepository.GetAll().ToList();
                if (kasaHareket != null)
                {
                    return kasaHareket;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

    }
}
