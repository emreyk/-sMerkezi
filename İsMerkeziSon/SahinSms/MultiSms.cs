using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahinSms
{
    public class MultiSms
    {
        public string apikey { get; set; }
        public string type { get; set; }
        public string orjin { get; set; }
        public string gonderimzamani { get; set; }
        public string dil { get; set; }
        public string flashsms { get; set; }
        public Mesajpaket[] mesajpaket { get; set; }
    }

    public class Mesajpaket
    {
        public string mesaj { get; set; }
        public string tel { get; set; }
    }
}
