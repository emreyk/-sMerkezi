using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class kasa:Base
    {
       public double girentutar_tl { get; set; }
       public double cikantutar_tl { get; set; }
       public double girentutar_dolar { get; set; }
       public double cikantutar_dolar { get; set; }
       public double bakiye_tl { get; set; }
       public double bakiye_dolar { get; set; }
       public string kasa_no { get; set; }
       public string kasa_adi { get; set; }
       public DateTime? acilis_tarihi { get; set; }
       public double acilis_bakiye { get; set; }
       public string durum { get; set; }
       public string aciklama { get; set; }
    }
}
