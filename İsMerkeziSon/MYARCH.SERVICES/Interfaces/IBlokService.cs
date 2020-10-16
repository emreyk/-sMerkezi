using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBlokService
    {
        List<EBlokDTO> BlokGetir();

        bool BlokKaydet(blok model);

        bool BlokSil(blok model);

        bool BlokGuncelle(EBlokDTO model);

        EBlokDTO BlokDaireleri(int id);


    }
}
