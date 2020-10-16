using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class toplu_borclandir:Base
    {

        public DateTime tarih { get; set; }
        public DateTime sonodeme_tarihi { get; set; }
        public string donem { get; set; }
        public string refno { get; set; }
        public string kullanici { get; set; }
        public string aciklama { get; set; }
        public string dagitim_sekli { get; set; }
        public string tip { get; set; }
        public int gun { get; set; }
        public int ay { get; set; }
        public int yil { get; set; }
        public string borclandirma_turu { get; set; }
        public double? tutar { get; set; }
        
    }
}
