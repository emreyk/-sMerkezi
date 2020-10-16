using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface ITopluBorclandirService
    {
        List<TopluBorclandirDTO> TopluBorclandirmalar();

        bool TopluBorclandirKaydet(toplu_borclandir model, hesap_hareket hesapModel, string tip);

        bool TopluBorcSil(string refno,string borclandirmaTuru);

        toplu_borclandir TopluBorclandirRefno(string refno);

        bool AkarYakitKontrol();

        bool AkaryakitBorclanidrmaSayisi();
    }
}
