using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
   public class EFinansalBorcDuzenle
    {
        public int id { get; set; }
        public string aciklama { get; set; }
        public DateTime tarih { get; set; }
        public DateTime sonodeme_tarihi { get; set; }
        public string isim { get; set; }
        public string blok_adi { get; set; }
        public string daire_numarasi { get; set; }
        public string borclandirma_turu { get; set; }
        public double borc { get; set; }
        public int kisi_id { get; set; }
        public string para_birimi { get; set; }
    }
}
