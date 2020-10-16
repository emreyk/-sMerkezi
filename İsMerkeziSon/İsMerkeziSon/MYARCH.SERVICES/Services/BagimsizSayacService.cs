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
    public class BagimsizSayacService : IBagimsizSayacService
    {
        private readonly IGenericRepository<bagimsiz_bolum_sayaclari> _bagimsizSayacRepository;
        private readonly IUnitofWork _uow;
        private EbagimsizBolumSayaclariDTO _bagimsizSayacDTO;
        public BagimsizSayacService(UnitofWork uow)
        {
            _uow = uow;
            _bagimsizSayacRepository = _uow.GetRepository<bagimsiz_bolum_sayaclari>();
            _bagimsizSayacDTO = new EbagimsizBolumSayaclariDTO();
        }

        public List<EbagimsizBolumSayaclariDTO> BagimsizSayacGetir()
        {
            var control = (from u in _bagimsizSayacRepository.GetAll()

                           select new EbagimsizBolumSayaclariDTO
                           {
                               id = u.id,
                               blok_adi = u.blok_adi,
                               bagimsiz_bolum = u.bagimsiz_bolum,
                               ana_sayac_adi = u.ana_sayac_adi,
                               tesisat_no = u.tesisat_no,
                               ilk_okuma = u.ilk_okuma,
                               aciklama = u.aciklama,
                           }).OrderByDescending(x => x.id).ToList();

            return control;
        }

        public bool BagimsizSayacKaydet(bagimsiz_bolum_sayaclari model)
        {
            try
            {
                _bagimsizSayacRepository.Insert(model);
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
    }
}