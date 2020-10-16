using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBagimsizSayacService
    {
        List<EbagimsizBolumSayaclariDTO> BagimsizSayacGetir();

        bool BagimsizSayacKaydet(bagimsiz_bolum_sayaclari model);
    }
}
