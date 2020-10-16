using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class yakit:Base
    {
        public double birim_fiyat { get; set; }
        public double alinan_miktar { get; set; }
        public string para_birimi { get; set; }
        public double toplam_tutar { get; set; }
        public double toplam_petek_uzunluk { get; set; }
        public DateTime? tarih { get; set; }
    }
}
