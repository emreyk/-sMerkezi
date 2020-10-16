using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EFinansalTahsilatDuzenle
    {
        public int id { get; set; }
        public DateTime tarih { get; set; }
        public string islem { get; set; }
        public double alacak { get; set; }
        public int kisi_id { get; set; }
        public string aciklama { get; set; }
        public string refno { get; set; }
        public int kasa_id { get; set; }
        public int banka_id { get; set; }
        public string para_birimi { get; set; }
        public string islem_turu { get; set; }
        public int bagimsiz_id { get; set; }
        public int tahsilat_id { get; set; }
        public string borclandirma_turu { get; set; }
    }
}
