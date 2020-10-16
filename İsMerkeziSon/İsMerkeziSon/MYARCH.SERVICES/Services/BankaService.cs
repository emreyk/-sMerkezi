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
    public class BankaService : IBankaService
    {

        private readonly IGenericRepository<banka> _bankaRepository;
        private readonly IGenericRepository<banka_hareket> _bankaHareketRepository;
        private readonly IUnitofWork _uow;

        public BankaService(UnitofWork uow)
        {
            _uow = uow;
            _bankaRepository = _uow.GetRepository<banka>();
            _bankaHareketRepository = _uow.GetRepository<banka_hareket>();

        }

        public List<banka> BankaGetir()
        {
            var bankalar = _bankaRepository.GetAll().ToList();
            return bankalar;
        }

        public banka BankaBilgileri(int id)
        {
            var bankaBilgileri = _bankaRepository.GetAll().Where(u => u.id == id).SingleOrDefault();
            return bankaBilgileri;
        }

        public bool BankaKaydet(banka model)
        {
            _bankaRepository.Insert(model);

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

        public bool BankaGuncelle(banka model)
        {
            var gelenBanka = _bankaRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenBanka);

            _bankaRepository.Update(gelenBanka);

            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<banka_hareket> BankaHareket()
        {
            try
            {
                var bankaHareket = _bankaHareketRepository.GetAll().ToList();
                if (bankaHareket != null)
                {
                    return bankaHareket;
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
