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
    public class SmsService:ISmsService
    {

        private readonly IGenericRepository<sms> _smsRepository;
        private readonly IUnitofWork _uow;

        public SmsService(UnitofWork uow)
        {
            _uow = uow;
            _smsRepository = _uow.GetRepository<sms>();

        }

        public bool SmsKaydet(sms model)
        {
            try
            {
                _smsRepository.Insert(model);
                if (_uow.SaveChanges()>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //sms listesi
        public List<ESmsKisi> SmsListesi()
        {
            using (var context = new MyArchContext())
            {

                var smsListesi = context.Database.SqlQuery<ESmsKisi>("SELECT icerik,tel1,isim,tarih FROM sms LEFT JOIN kisiler ON sms.kisi_id = kisiler.id").ToList();
                return smsListesi;
            }
        }
    }
}
