using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IKasaService
    {
        bool KasaKaydet(kasa model);

        List<kasa> KasaGetir();

        bool KasaGuncelle(kasa model);

        kasa KasaBilgileri(int id);

        List<kasa_hareket> KasaHareket();
    }
}
