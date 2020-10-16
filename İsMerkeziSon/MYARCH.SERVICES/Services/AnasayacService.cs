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
    public class AnasayacService : IAnasayacService
    {

        private readonly IGenericRepository<anasayac> _anaSayacRepository;
        private readonly IGenericRepository<bagimsiz_bolum_sayaclari> _bagimsizSayacRepository;
        private readonly IUnitofWork _uow;
        private EAnasayacDTO _anaSayacDTO;
        public AnasayacService(UnitofWork uow)
        {
            _uow = uow;
            _anaSayacRepository = _uow.GetRepository<anasayac>();
            _bagimsizSayacRepository = _uow.GetRepository<bagimsiz_bolum_sayaclari>();
            _anaSayacDTO = new EAnasayacDTO();
        }

        public List<EAnasayacDTO> AnaSayacGetir()
        {
            var control = (from u in _anaSayacRepository.GetAll()

                           select new EAnasayacDTO
                           {
                               id = u.id,
                               sayac_adi = u.sayac_adi,
                               sayac_tipi = u.sayac_tipi,
                               tesisat_no = u.tesisat_no,
                               ortak_alan_dagilim = u.ortak_alan_dagilim,
                               ortak_alan_yuzde = u.ortak_alan_yuzde
                           }).OrderByDescending(x => x.id).ToList();

            return control;
        }

      
        public EAnasayacDTO SayacBilgileriGetir(int id)
        {
            var control = (from u in _anaSayacRepository.GetAll()
                           where u.id == id
                           select new EAnasayacDTO
                           {
                               id = u.id,
                               sayac_adi = u.sayac_adi,
                               sayac_tipi = u.sayac_tipi,
                               tesisat_no = u.tesisat_no,
                               ortak_alan_dagilim = u.ortak_alan_dagilim,
                               ortak_alan_yuzde = u.ortak_alan_yuzde

                           }).SingleOrDefault();
            return control;
        }

        public bool SayacKaydet(anasayac model)
        {
            try
            {
                _anaSayacRepository.Insert(model);
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

        public List<ESayacTipiDTO> SayacTipi()
        {
            
            using (var context = new MyArchContext())
            {
                
                var sayacTipi = context.Database.SqlQuery<ESayacTipiDTO>("SELECT sayac_tipi,id FROM sayac_tipleri").ToList();
                return sayacTipi;
            }

        }


        public List<ESayacOrtakDagitim> OrtakAlan()
        {
            using (var context = new MyArchContext())
            {

                var sayacTipi = context.Database.SqlQuery<ESayacOrtakDagitim>("SELECT * FROM anasayac_ortak_dagitim").ToList();
                return sayacTipi;
            }
        }

        public bool AnaSayacSil(anasayac model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    if (context.Database.ExecuteSqlCommand("DELETE FROM ana_sayac where id = '" + model.id + "'") > 0)
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

        public bool AnaSayacGuncelle(EAnasayacDTO model)
        {
            var gelenBlok = _anaSayacRepository.Find(model.id);

            AutoMapper.Mapper.DynamicMap(model, gelenBlok);

            _anaSayacRepository.Update(gelenBlok);
            if (_uow.SaveChanges() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<bagimsiz_bolum_sayaclari> SayacSilKontrol(int id)
        {
            var control =  _bagimsizSayacRepository.GetAll().Where(x => x.ana_sayac_id == id).ToList();

            return control;

        }
    }
}
