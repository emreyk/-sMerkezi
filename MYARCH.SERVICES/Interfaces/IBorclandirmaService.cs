using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBorclandirmaService
    {
        bool BorclandirmaKaydet(hesap_hareket model);
        List<EHesapHaraketDTO> BorclandirmaListesi();
        List<EHesapHaraketDTO> TahsilatListesi();
        EFinansalBorcDuzenle FinansBorcDuzenle(int id);
        bool FinansalBorcGuncelle(EFinansalBorcDuzenle model);
        bool HesapHaraketSil(hesap_hareket model);
        EFinansalTahsilatDuzenle FinansTahsilatDuzenle(string refno);
        bool FinansalTahsilatGuncelle(EFinansalTahsilatDuzenle model, hesap_hareket hhModel);
        hesap_hareket HesapHareketId(int hhId);
        double KalanBakiyeKontrol(hesap_hareket model);
        bool PesinTahsilatKaydet(hesap_hareket hhHareket);
        int GunSayisiGetir(string ay);
        bool borclandirmaKontrol(toplu_borclandir model);
        string BorclandirmaTuru(int? id);
        bool TahsilatSil(hesap_hareket model);
        List<hesap_hareket> TumBorcListesi();
        bool TahsilatKontrolRefno(string refno);
        bool TahsilatKontrolId(int refno);
        bool BorcKontrolIdKisi(int kisi_id);
        bool TahsilatKontrolIdKisi(int kisi_id);
        bool AkaryakitDonemKontrol(int id);
        List<EHesapHaraketDTO> Yillar();
        

    }
}
