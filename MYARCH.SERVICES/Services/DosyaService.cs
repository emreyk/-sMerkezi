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
    public class DosyaService : IDosyaService
    {
        private readonly IGenericRepository<dosyalar> _dosyalarRepository;
        private readonly IUnitofWork _uow;
        public DosyaService(UnitofWork uow)
        {
            _uow = uow;
            _dosyalarRepository = _uow.GetRepository<dosyalar>();
        }

        public bool DosyaKaydet(dosyalar model)
        {
            try
            {
                _dosyalarRepository.Insert(model);
                if (_uow.SaveChanges()>0)
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
    }
}
