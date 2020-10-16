using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYARCH.DTO.EEntity
{
    public class EKisiDTO
    {
        public int id { get; set; }
        public string isim { get; set; }
        public string tc { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string eposta { get; set; }
        public string adres { get; set; }
        public string meslek { get; set; }
        public string ogrenim_durumu { get; set; }
        public string cinsiyet { get; set; }
        public int? bagimsiz_id { get; set; }
        public string blok_adi { get; set; }
        public string daire_numarasi { get; set; }
        public DateTime? tarih { get; set; }
        public DateTime? sonodeme_tarihi { get; set; }
        public string aciklama { get; set; }
        public double borc { get; set; }
        public double alacak { get; set; }
        public double? bakiye { get; set; }
        public double? BAKIYE { get; set; }
        public string durumu { get; set; }
        public string aktif { get; set; }
        public string borclandirma_turu { get; set; }
        public string islem_turu { get; set; }
        public string tahsilat_durumu { get; set; }
        public string para_birimi { get; set; }
        public double borc_tl { get; set; }
        public double alacak_tl { get; set; }
        public double borc_dolar { get; set; }
        public double alacak_dolar { get; set; }
        public string kullanici_adi { get; set; }
        public string sifre { get; set; }
        public string rutbe { get; set; }
        public string donem { get; set; }
        public string tip { get; set; }
    }
}
