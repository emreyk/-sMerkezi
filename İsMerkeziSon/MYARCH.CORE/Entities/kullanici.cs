using System;
using System.ComponentModel.DataAnnotations;
namespace MYARCH.CORE
{
    public partial class kullanici:Base
    {
        public string kullanici_adi { get; set; }
        public string kullanici_kodu { get; set; }
        public string sifre { get; set; }
    }
}
