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
    public class AidatGunService:IAidatGunService
    {
        private readonly IGenericRepository<aidat_gun> _aidatGunRepository;
        private readonly IUnitofWork _uow;

        public AidatGunService(UnitofWork uow)
        {
            _uow = uow;
            _aidatGunRepository = _uow.GetRepository<aidat_gun>();

        }

        public aidat_gun AidatGunGetir()
        {
            var aidatGun = _aidatGunRepository.GetAll().FirstOrDefault();
            return aidatGun;
        }

        public bool AidatGunuGuncelle(string aidatGunu)
        {
            using (var context = new MyArchContext())
            {

                if (context.Database.ExecuteSqlCommand("UPDATE aidat_gun SET aidat_gunu='"+aidatGunu+"' ")>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
