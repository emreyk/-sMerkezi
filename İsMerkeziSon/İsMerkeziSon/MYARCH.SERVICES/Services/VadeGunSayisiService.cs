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
    public class VadeGunSayisiService : IVadeGunSayisiService
    {

        private readonly IGenericRepository<vade_gun> _aidatGunRepository;
        private readonly IGenericRepository<yakit> _yakitRepository;
        private readonly IUnitofWork _uow;

        public VadeGunSayisiService(UnitofWork uow)
        {
            _uow = uow;
            _aidatGunRepository = _uow.GetRepository<vade_gun>();
            _yakitRepository = _uow.GetRepository<yakit>();
        }

    
        public vade_gun VadeGunSayisiGetir()
        {
            var vadeGunSayisi = _aidatGunRepository.GetAll().FirstOrDefault();
            return vadeGunSayisi;
        }

        public bool VadeGunSayisiGuncelle(string vadeGunSayisi)
        {
            using (var context = new MyArchContext())
            {

                if (context.Database.ExecuteSqlCommand("UPDATE vade_gun SET vade_gun_sayisi='" + vadeGunSayisi + "' ") > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool YakitKaydet(yakit model)
        {
            try
            {
                model.toplam_tutar = model.birim_fiyat * model.alinan_miktar;

                _yakitRepository.Insert(model);
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

        public List<yakit> YakitBilgileri()
        {
            var yakitBilgileri = _yakitRepository.GetAll().OrderByDescending(x=>x.id).ToList();
            return yakitBilgileri;
        }
    }
}
