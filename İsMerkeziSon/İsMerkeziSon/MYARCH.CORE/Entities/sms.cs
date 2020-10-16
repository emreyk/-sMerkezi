using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public class sms:Base
    {

        public string baslik { get; set; }
        public string icerik { get; set; }
        public string tarih { get; set; }
        public int kisi_id { get; set; }
        public string tel { get; set; }
    }
}
