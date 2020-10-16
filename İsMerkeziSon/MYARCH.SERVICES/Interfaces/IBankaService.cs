using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBankaService
    {
        bool BankaKaydet(banka model);

        List<banka> BankaGetir();

        banka BankaBilgileri(int id);

        bool BankaGuncelle(banka model);

        List<banka_hareket> BankaHareket();
    }
}
