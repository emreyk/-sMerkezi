using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IMuhtelifBasliklarService
    {
        bool Kaydet(muhtelif_baslikler model);

        List<muhtelif_baslikler> basliklar();

        bool baslikSil(muhtelif_baslikler model);

        string ToplamaEkleSecim(string baslikKodu);

        muhtelif_baslikler BaslikGetir(int id);

        bool Guncelle(muhtelif_baslikler model);


        bool MuhtelifIslemSil(muhtelif_islemler model);


    }
}
