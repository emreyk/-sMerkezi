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
    public class BlokService : IBlokService
    {

        private readonly IGenericRepository<blok> _blokRepository;
        private readonly IGenericRepository<bagimsiz_bolumler> _bagimsizBolumRepository;
        private readonly IUnitofWork _uow;
        private EBlokDTO _blokDTO;
        public BlokService(UnitofWork uow)
        {
            _uow = uow;
            _blokRepository = _uow.GetRepository<blok>();
            _bagimsizBolumRepository = _uow.GetRepository<bagimsiz_bolumler>();
            _blokDTO = new EBlokDTO();
        }

        public List<EBlokDTO> BlokGetir()
        {
            var control = (from u in _blokRepository.GetAll()

                           select new EBlokDTO
                           {
                               id = u.id,
                               blok_adi = u.blok_adi,
                               daire_sayisi = u.daire_sayisi,
                               doluluk_oran = u.doluluk_oran,
                           

                           }).OrderByDescending(x => x.id).ToList();
            return control;
        }

        public bool BlokKaydet(blok model)
        {
            try
            {
                _blokRepository.Insert(model);
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

        public bool BlokSil(blok model)
        {
            try
            {
                using (var context = new MyArchContext())
                {
                    context.Database.ExecuteSqlCommand("DELETE FROM bagimsiz_bolumler where blok_id = '" + model.id + "'");
                    if (context.Database.ExecuteSqlCommand("DELETE FROM blok where id = '" + model.id + "'") > 0)
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

        public bool BlokGuncelle(EBlokDTO model)
        {

            try
            {
                var gelenBlok = _blokRepository.Find(model.id);
                AutoMapper.Mapper.DynamicMap(model, gelenBlok);
                _blokRepository.Update(gelenBlok);
                if (_uow.SaveChanges() != 0)
                {
                    using (var context = new MyArchContext())
                    {
                        if (context.Database.ExecuteSqlCommand("update bagimsiz_bolumler set blok_adi='"+model.blok_adi+"' where blok_id = '" + model.id + "'") > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                
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

        public EBlokDTO BlokDaireleri(int id)
        {
            var daireSayisi = (from u in _blokRepository.GetAll().Where(x => x.id == id)

                               select new EBlokDTO
                               {
                                   daire_sayisi = u.daire_sayisi,
                               }).SingleOrDefault();
            return daireSayisi;
        }
    }
}
