using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IRaporlarService
    {
        List<EBorcluListesiDTO> VadesiGecenBorcluListesi();
        List<EBorcluListesiDTO> AidatVadesiGecenBorcluListesi();
        List<EBorcluListesiDTO> DemirbasVadesiGecenBorcluListesi();
        List<EBorcluListesiDTO> YakitVadesiGecenBorcluListesi();

        List<EBorcluListesiDTO> VadesiBekleyenBorcluListesi();
        List<EBorcluListesiDTO> AidatVadesiBekleyenBorcluListesi();
        List<EBorcluListesiDTO> DemirbasVadesiBekleyenBorcluListesi();
        List<EBorcluListesiDTO> YakitVadesiBekleyenBorcluListesi();
    }
}
