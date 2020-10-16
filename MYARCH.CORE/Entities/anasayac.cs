using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{

    public partial class anasayac : Base
    {
        public string sayac_adi { get; set; }
        public string sayac_tipi { get; set; }
        public string tesisat_no { get; set; }
        public string ortak_alan_dagilim { get; set; }
        public double ortak_alan_yuzde { get; set; }
        public int sayac_id { get; set; }
        public int ortak_dagitim_id { get; set; }
    }
}
