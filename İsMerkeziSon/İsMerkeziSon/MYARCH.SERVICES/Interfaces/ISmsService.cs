using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface ISmsService
    {
        List<ESmsKisi> SmsListesi();

        bool SmsKaydet(sms model);
    }
}
