using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IMuhtelifIslemlerService
    {
        //Banka
        bool Kaydet(muhtelif_islemler model, int bankaId);

        //Kasa kaydet
        bool KaydetKasa(muhtelif_islemler model, int kasaId);

        List<muhtelif_islemler> Liste();

        muhtelif_islemler MuhtelifGetir(int id);

        bool KasaGuncelle(muhtelif_islemler model);

        double BagimsizAlanHesabı(string paraBirimi,string basTarih,string bitTarih);
    }
}
