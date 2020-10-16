using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IFirmaService
    {
        bool FirmaKaydet(firmalar model);

        List<firmalar> FirmaListesi();

        bool FirmaGuncelle(firmalar firmaGuncelle);

        firmalar FirmaGetir(firmalar model);

        bool FirmaSil(int id);
    }
}
