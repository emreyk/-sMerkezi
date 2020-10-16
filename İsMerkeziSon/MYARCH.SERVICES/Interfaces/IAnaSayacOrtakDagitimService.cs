using MYARCH.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IAnaSayacOrtakDagitimService
    {
        List<anasayac_ortak_dagitim> OrtakAlanGetir();

        bool OrtakAlanGuncelle(anasayac_ortak_dagitim model);

        bool OrtakAlanKaydet(anasayac_ortak_dagitim model);

        bool OrtakAlanSil(anasayac_ortak_dagitim model);
    }
}
