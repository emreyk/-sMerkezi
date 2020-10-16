using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EbagimsizBolumSayaclariDTO
    {
        public int id { get; set; }
        public string blok_adi { get; set; }
        public int blok_id { get; set; }
        public string bagimsiz_bolum { get; set; }
        public string ana_sayac_adi { get; set; }
        public int ana_sayac_id { get; set; }
        public string tesisat_no { get; set; }
        public double ilk_okuma { get; set; }
        public string aciklama { get; set; }
    }
}
