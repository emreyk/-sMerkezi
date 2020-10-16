using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EHesapHaraketDTO
    {
        public int id { get; set; }
        public string aciklama { get; set; }
        public DateTime? tarih { get; set; }
        public DateTime? sonodeme_tarihi { get; set; }
        public double borc { get; set; }
        public double alacak { get; set; }
        public string bagimsiz_id { get; set; }
        public int kisi_id { get; set; }
      
        public string islem_turu { get; set; }
        public DateTime? islem_tarihi { get; set; }
        public string borclandirma_turu { get; set; }
        public string isim { get; set; }
        public string blok_adi { get; set; }
        public string daire_numarasi { get; set; }
        public string islem { get; set; }
        public string refno { get; set; }
        public string tahsilat_durumu { get; set; }
        public string para_birimi { get; set; }
        public int kasa_id { get; set; }
        public int banka_id { get; set; }
        public string yil { get; set; }
    }
}
