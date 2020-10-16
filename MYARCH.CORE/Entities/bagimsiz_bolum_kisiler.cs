using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class bagimsiz_bolum_kisiler : Base
    {
        public int bagimsiz_id { get; set; }
        public int kisi_id { get; set; }
        public DateTime? giris_tarihi { get; set; }
        public DateTime? cikis_tarihi { get; set; }
        public string hisse_payi { get; set; }
        public string tip { get; set; }
        public string aktif { get; set; }
        public string oturan_katmaliki { get; set; }
        public string kiraci { get; set; }
        public double daire_katsayisi { get; set; }
    }
}
