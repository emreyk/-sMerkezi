using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class kisiler : Base
    {

        public string isim { get; set; }
        public string kullanici_adi { get; set; }
        public string sifre { get; set; }
        public string tc { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string eposta { get; set; }
        public string adres { get; set; }
        public string meslek { get; set; }
        public string ogrenim_durumu { get; set; }
        public string rutbe { get; set; }
        public string cinsiyet { get; set; }
        public double alacak_tl { get; set; }
        public double alacak_dolar { get; set; }
        public double borc_tl { get; set; }
        public double borc_dolar { get; set; }
        public string durumu { get; set; }
        public string aktif { get; set; }

    }
}
