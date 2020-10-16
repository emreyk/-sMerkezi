using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class muhtelif_islemler : Base
    {

        public string refno { get; set; }
        public string islem_adi { get; set; }
        public string islem_no { get; set; }
        public string islem_turu { get; set; }
        public string muhtelif_tipi { get; set; }
        public string tarih { get; set; }
        public string para_birimi { get; set; }
        public string aciklama { get; set; }
        public string evrak_no { get; set; }
        public double tutar { get; set; }
        public int kasa_id { get; set; }
        public int banka_id { get; set; }
        public string toplama_katilim { get; set; }
    }
}
