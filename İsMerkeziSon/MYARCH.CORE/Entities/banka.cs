using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class banka:Base
    {
        public string  banka_no  { get; set; }
        public string  banka_adi { get; set; }
        public string  hesap_adi  { get; set; }
        public string  sube_kodu { get; set; }
        public string  hesap_numarası  { get; set; }
        public string iban  { get; set; }
        public string  durum { get; set; }
        public double girentutar_tl { get; set; }
        public double girentutar_dolar { get; set; }
        public double cikantutar_tl { get; set; }
        public double cikantutar_dolar { get; set; }
        public double bakiye_tl { get; set; }
        public double bakiye_dolar { get; set; }
        public DateTime? tarih { get; set; }
        public double acilis_bakiye { get; set; }
        public string aciklama { get; set; }
      
    }
}
