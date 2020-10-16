using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EBagimsizDetayKisiler
    {
      
        public string isim { get; set; }
        public string tip { get; set; }
        public string tel1 { get; set; }
        public string hisse_payi { get; set; }
        public int kisi_id { get; set; }
        public double? bakiye { get; set; }
        public DateTime? giris_tarihi { get; set; }
        public DateTime? cikis_tarihi { get; set; }
    }
}
