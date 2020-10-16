using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class kasa_hareket : Base
    {
        public string kullanici_adi { get; set; }
        public string hesap_adi { get; set; }
        public string islem { get; set; }
        public string islem_turu { get; set; }
        public double kasa_borc_tl { get; set; }
        public double kasa_borc_dolar { get; set; }
        public double kasa_alacak_tl { get; set; }
        public double kasa_alacak_dolar { get; set; }
        public string aciklama { get; set; }
        public double bakiye { get; set; }
        public string refno { get; set; }
        public int kisi_id { get; set; }
        public int kasa_id { get; set; }
        public string toplama_katilim { get; set; }
        public string tarih { get; set; }
    }
}
