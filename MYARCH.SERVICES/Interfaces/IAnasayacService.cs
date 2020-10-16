using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IAnasayacService
    {
        List<EAnasayacDTO> AnaSayacGetir();

        bool SayacKaydet(anasayac model);

        EAnasayacDTO SayacBilgileriGetir(int id);

        List<ESayacTipiDTO> SayacTipi();

        List<ESayacOrtakDagitim> OrtakAlan();

        bool AnaSayacSil(anasayac model);


        bool AnaSayacGuncelle(EAnasayacDTO gorev);

        List<bagimsiz_bolum_sayaclari> SayacSilKontrol(int id);
        
    }
}
