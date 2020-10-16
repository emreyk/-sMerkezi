using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class hesap_hareket : Base
    {
        
        public string aciklama { get; set; }
        public DateTime tarih { get; set; }
        public string gun { get; set; }
        public string ay { get; set; }
        public string yil { get; set; }
        public string saat { get; set; }
        public DateTime? sonodeme_tarihi { get; set; }
        public string donem { get; set; }
        public double borc { get; set; }
        public double alacak { get; set; }
        public int bagimsiz_id { get; set; }
        public int kisi_id { get; set; }
        public string islem_turu { get; set; }
        public DateTime? islem_tarihi { get; set; }
        public string borclandirma_turu { get; set; }
        public double bakiye { get; set; }
        public string para_birimi { get; set; }
        public string islem { get; set; }
        public string hesap_adi { get; set; }
        public string refno { get; set; }
        public int kasa_id { get; set; }
        public int banka_id { get; set; }
        public int? tahsilat_id { get; set; }
        public string tahsilat_durumu { get; set; }
        public string pesin_tahsilat { get; set; }
    }
}
