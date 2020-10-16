using MYARCH.CORE;
using MYARCH.CORE.Constants;
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
    public class UserService : IUserService
    {
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IGenericRepository<bagimsiz_bolum_kisiler> _bagimsizKisiRepository;
        private readonly IUnitofWork _uow;
        private EUserDTO _userDTO;
        public UserService(UnitofWork uow)
        {
            _uow = uow;
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _bagimsizKisiRepository = _uow.GetRepository<bagimsiz_bolum_kisiler>();
            _userDTO = new EUserDTO();
        }

        public EUserDTO KullaniciKontrol(EUserDTO model)
        {
            var control = (from u in _kisilerRepository.GetAll()
                           where u.kullanici_adi == model.kullanici_adi && 
                           u.sifre == model.sifre 
                           select new EUserDTO
                           {
                               kullanici_adi = u.kullanici_adi,
                               sifre = u.sifre,
                               rutbe = u.rutbe,
                               kisi_id = u.id

                           }).SingleOrDefault();
            return control;
        }

        //bagımsız bolum kisi kontrol dilen
        public List<bagimsiz_bolum_kisiler> BagimsisKisiKontrol(int kisiId)
        {
            List<bagimsiz_bolum_kisiler> kayit = _bagimsizKisiRepository.GetAll().Where(x => x.kisi_id == kisiId && x.aktif == "true").ToList();
            return kayit;
        }
    }
}
