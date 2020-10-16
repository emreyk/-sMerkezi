using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.CORE.Entities
{
    public partial class tipler:Base
    {
        public string bagimsiz_tip { get; set; }
        public string para_birimi { get; set; }
        public double aidat_tutar { get; set; }
        public string yil { get; set; }
    }
}
