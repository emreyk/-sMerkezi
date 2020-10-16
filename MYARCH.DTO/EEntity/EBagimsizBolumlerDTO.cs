using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EBagimsizBolumlerDTO
    {
        [Key]
        public int id { get; set; }
        public string blok_adi { get; set; }
        public int blok_id { get; set; }
        public string daire_numarasi { get; set; }
        public string daire_adres_no { get; set; }
        public string adres { get; set; }
        public string durumu { get; set; }
        public string kat { get; set; }
        public double? brut_alan { get; set; }
        public double? net_alan { get; set; }
        public string arsa_payı { get; set; }
        public string su_abone_no { get; set; }
        public string elektrik_abone_no { get; set; }
        public double? borc_toplam { get; set; }
        public double? alacak_toplam { get; set; }
        public double? bakiye { get; set; }
        public double? aidat_tutari { get; set; }
        public string para_birimi { get; set; }
        public string tip { get; set; }
        public int tip_id { get; set; }
        public string katmaliki { get; set; }
        public string kiracı { get; set; }
        public double? toplam { get; set; }
        public int? kiraci_id { get; set; }
        public int? katmaliki_id { get; set; }
        public string petek_boyu { get; set; }
        public double daire_katsayisi { get; set; }


    }
}
