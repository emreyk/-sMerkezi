using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahinSms
{
    public class Fonksiyonlar
    {
        public static string tr2en(string kelimecik)
        {
            kelimecik = kelimecik.Replace('ö', 'o');
            kelimecik = kelimecik.Replace('ü', 'u');
            kelimecik = kelimecik.Replace('ğ', 'g');
            kelimecik = kelimecik.Replace('ş', 's');
            kelimecik = kelimecik.Replace('ı', 'i');
            kelimecik = kelimecik.Replace('ç', 'c');
            kelimecik = kelimecik.Replace('Ö', 'O');
            kelimecik = kelimecik.Replace('Ü', 'U');
            kelimecik = kelimecik.Replace('Ğ', 'G');
            kelimecik = kelimecik.Replace('Ş', 'S');
            kelimecik = kelimecik.Replace('İ', 'I');
            kelimecik = kelimecik.Replace('Ç', 'C');

            return kelimecik;
        }
    }
}
