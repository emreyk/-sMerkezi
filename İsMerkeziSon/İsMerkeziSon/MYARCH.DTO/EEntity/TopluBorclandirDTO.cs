using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class TopluBorclandirDTO
    {
        public int id { get; set; }
        public DateTime tarih { get; set; }
        public DateTime sonodeme_tarihi { get; set; }
        public int kisi_id { get; set; }
        public string refno { get; set; }
        public string dagitim_sekli { get; set; }
        public string borclandirma_turu { get; set; }
        public double tutar { get; set; }
        public string tip { get; set; }
    }
}
