using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EBorcluListesiDTO
    {
        public int id { get; set; }
        public string refno { get; set; }
        public string aciklama { get; set; }
        public DateTime tarih  { get; set; }
        public DateTime sonodeme_tarihi { get; set; }
        public double borc { get; set; }
        public string hesap_adi { get; set; }
        public string borclandirma_turu { get; set; }
        public string daire_numarasi { get; set; }
        public string tahsilat_durumu { get; set; }
        public int kisi_id { get; set; }
    }
}
