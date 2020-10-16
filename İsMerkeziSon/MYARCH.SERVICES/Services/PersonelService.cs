using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.DATA.Context;

namespace MYARCH.SERVICES.Services
{
    public class PersonelService : IPersonelService
    {

        private readonly IGenericRepository<personel> _personelRepository;
        private readonly IUnitofWork _uow;
        private EPersonelDTO _personelDTO;
        public PersonelService(UnitofWork uow)
        {
            _uow = uow;
            _personelRepository = _uow.GetRepository<personel>();
            _personelDTO = new EPersonelDTO();
        }

        public EPersonelDTO PersonelBilgileriGetir(int id)
        {
            var control = (from u in _personelRepository.GetAll()
                           where u.id == id
                           select new EPersonelDTO
                           {
                               id = u.id,
                               personel_adi = u.personel_adi,
                               personel_tc = u.personel_tc,
                               personel_gorev = u.personel_gorev,
                               personel_email = u.personel_email,
                               personel_tel = u.personel_tel,
                               personel_giristarihi = u.personel_giristarihi,
                               personel_cikisstarihi = u.personel_cikisstarihi,
                               personel_maas = u.personel_maas,
                               personel_cinsiyet = u.personel_cinsiyet

                           }).SingleOrDefault();
            return control;
        }


        public List<EPersonelDTO> PersonelGetir()
        {
            var control = (from u in _personelRepository.GetAll()

                           select new EPersonelDTO
                           {
                               id = u.id,
                               personel_adi = u.personel_adi,
                               personel_tc = u.personel_tc,
                               personel_gorev = u.personel_gorev,
                               personel_email = u.personel_email,
                               personel_tel = u.personel_tel,
                               personel_giristarihi = u.personel_giristarihi,
                               personel_cikisstarihi = u.personel_cikisstarihi,
                               personel_maas = u.personel_maas,
                               personel_cinsiyet = u.personel_cinsiyet

                           }).OrderByDescending(x => x.id).ToList();
            return control;
        }

        public bool PersonelKaydet(personel model)
        {
            try
            {
                _personelRepository.Insert(model);
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

        public bool PersonelSil(personel model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    int sonuc= context.Database.ExecuteSqlCommand("DELETE FROM personel where id = '" + model.id + "'");

                    if (sonuc > 0)
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
