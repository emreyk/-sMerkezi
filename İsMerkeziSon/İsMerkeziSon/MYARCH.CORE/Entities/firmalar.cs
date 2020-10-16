using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class firmalar:Base
    {
        public string firma_adi { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string adres { get; set; }
        public string yetkili_kisi { get; set; }
        public DateTime acilis_tarihi { get; set; }
        public double acilis_bakiyesi { get; set; }
    }
}
