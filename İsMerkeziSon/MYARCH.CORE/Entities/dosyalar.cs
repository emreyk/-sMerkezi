using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class dosyalar:Base
    {
        public int bagimsiz_id { get; set; }
        public int kisi_id { get; set; }
        public string dosya_yolu { get; set; }
        public string dosya_tipi { get; set; }
        public string dosya_adi { get; set; }
    }
}
