using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBorcTipleri
    {
        List<borc_tipleri> BorcTipleriGetir();
        bool BorcTipleriKaydet(borc_tipleri model);
        bool borcTipiSil(borc_tipleri model);
        string ParaBirimiAidat();
        string ParaBirimiAkaryakit();
        string AidatParaBirimiKontrol();
        string paraBirimiGetir(string borcTipi);

    }
}
