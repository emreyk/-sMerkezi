using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface ITipService
    {
        List<ETiplerDTO> TipGetir();

        bool TipKaydet(tipler model);

        bool TipGuncelle(ETiplerDTO model);

        bool TipSil(tipler model);

        tipler TipGetirId(int id);

        List<ETiplerDTO> TipGetirYıl();

        bool AidatTahsilatKontrol();

    }
}
