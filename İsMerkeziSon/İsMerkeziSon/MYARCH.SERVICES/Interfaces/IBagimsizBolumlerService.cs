using MYARCH.CORE.Entities;
using MYARCH.DTO.EEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.SERVICES.Interfaces
{
    public interface IBagimsizBolumlerService
    {
        bool BagimsizBolumBlokKaydet(bagimsiz_bolumler model);
        List<EBagimsizBolumlerDTO> BagimsizBolumler();
        EBagimsizBolumlerDTO BagimsizBolumBilgileriGetir(int id);
        bool BagimsizBolumGuncelle(bagimsiz_bolumler model);
        bool BagimsizBolumKisilerKaydet(bagimsiz_bolum_kisiler model, string isim, int bagimsizId, int? katmaliki_id, int? kiraci_id);
        List<EBagimsizDetayKisiler> BagimsizBolumlerKisiler(int id,string paraBirimi);
        bool CikisTarihiGuncelle(bagimsiz_bolum_kisiler model);
        bool KatMalikiKontrol(int bagimsiz_id, string tip);
        bool KatMalikiKiracimi(int bagimsiz_id);
        bool KiraciKontrol(int bagimsiz_id,string tip);
        bool BagimsızBolumDetayKontrol(int bagimsizId);
        bool BagimsizAdresNoKontrol(string adresNo,int id);
        bool BorcKontrol(int kisiId,int bagimsizId);
        bool BagimsizKisiSil(hesap_hareket model,string tip);
        //bool KatMalikiVarmi(int bagimsiz_id);

        bool BagimsizBolumKaydet(bagimsiz_bolumler model);
    }
}
