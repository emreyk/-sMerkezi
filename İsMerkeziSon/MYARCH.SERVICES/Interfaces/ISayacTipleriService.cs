using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface ISayacTipleriService
    {
        List<sayac_tipleri> SayacTipleri();

        bool SayacTipiKaydet(sayac_tipleri model);

        bool SayacTipiSil(sayac_tipleri model);

        bool SayacTipiGuncelle(sayac_tipleri model);

    }
}
