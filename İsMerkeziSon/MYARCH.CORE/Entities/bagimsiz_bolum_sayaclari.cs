using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class bagimsiz_bolum_sayaclari:Base
    {
        public string blok_adi { get; set; }
     
        public string bagimsiz_bolum { get; set; }
        public string ana_sayac_adi { get; set; }
        public int ana_sayac_id { get; set; }
        public string tesisat_no { get; set; }
        public double ilk_okuma { get; set; }
        public string aciklama { get; set; }
    }
}
