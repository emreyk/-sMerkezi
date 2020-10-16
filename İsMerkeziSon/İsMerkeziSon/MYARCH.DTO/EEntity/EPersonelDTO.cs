using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EPersonelDTO
    {
        public int id { get; set; }
        public string personel_adi { get; set; }
        public string personel_tc { get; set; }
        public string personel_gorev { get; set; }
        public string personel_email { get; set; }
        public string personel_tel { get; set; }
        public DateTime? personel_giristarihi { get; set; }
        public DateTime? personel_cikisstarihi { get; set; }
        public double personel_maas { get; set; }
        public string personel_cinsiyet { get; set; }
    }
}
