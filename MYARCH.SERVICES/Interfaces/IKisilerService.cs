using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IKisilerService
    {
        bool KisiKaydet(kisiler model);

        List<EKisiDTO> KisiGetir();

        List<EKisiDTO> KisiBagimsizBolumleri(int kisiId);

        EKisiDTO KisiGetirId(int id);

        List<EKisiDTO> KisiBorclariGetir(int id);

        List<EKisiDTO> KisiBorclariGetirDolar(int id);


        bool KisiGuncelle(kisiler kisiGuncelle);

        EKisiDTO KiraciDetay(int kiraci_id);

        EKisiDTO KatmalikiDetay( int katmaliki_id);

        bool KisiSil(int id);

        List<kisiler> KisiListesiModal();

        int kisiId(int hhId);

        double kisiBakiye(int kisiId);

        EKisiDTO kisiTlBakiye(int kisiID);

        EKisiDTO kisiDolarBakiye(int kisiID);

        List<bagimsiz_bolum_kisiler> KatmalikiListesi();

        List<bagimsiz_bolum_kisiler> KiraciListesi();

        List<EKisiDTO> PasifKisiListesi();

        bool KisiKayitlimi(int id);
    }
}
