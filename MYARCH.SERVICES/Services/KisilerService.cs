using MYARCH.CORE.Entities;
using MYARCH.DATA.Context;
using MYARCH.DATA.GenericRepository;
using MYARCH.DATA.UnitofWork;
using MYARCH.DTO.EEntity;
using MYARCH.SERVICES.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MYARCH.SERVICES.Services
{
    public class KisilerService : IKisilerService
    {
        private readonly IGenericRepository<kisiler> _kisilerRepository;
        private readonly IGenericRepository<bagimsiz_bolum_kisiler> _vagimsizkisilerRepository;
        private readonly IGenericRepository<hesap_hareket> _hesapHaraketRepository;
        private readonly IGenericRepository<bagimsiz_bolumler> _bagimsizBolumlerRepository;
        private readonly IGenericRepository<bagimsiz_bolum_kisiler> _bagimsizBolumKisilerRepository;
        private readonly IUnitofWork _uow;
        private EKisiDTO _kisilerkDTO;

        public KisilerService(UnitofWork uow)
        {
            _uow = uow;
            _kisilerRepository = _uow.GetRepository<kisiler>();
            _kisilerkDTO = new EKisiDTO();
            _bagimsizBolumlerRepository = _uow.GetRepository<bagimsiz_bolumler>();
            _bagimsizBolumKisilerRepository = _uow.GetRepository<bagimsiz_bolum_kisiler>();
            _hesapHaraketRepository = _uow.GetRepository<hesap_hareket>();
        }

        public bool KisiSil(int id)
        {
            try
            {
                var kisiID = _kisilerRepository.Find(id);
                _kisilerRepository.Delete(kisiID);
                if (_uow.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        public EKisiDTO KatmalikiDetay(int katmaliki_id)
        {
            var gelenVeri = (from u in _kisilerRepository.GetAll().Where(x => x.id == katmaliki_id)
                             where u.id == katmaliki_id
                             select new EKisiDTO
                             {

                                 tc = u.tc,
                                 tel1 = u.tel1,
                                 eposta = u.eposta,
                                 adres = u.adres,


                             }).FirstOrDefault();
            return gelenVeri;
        }

        public EKisiDTO KiraciDetay(int kiraci_id)
        {
            var gelenVeri = (from u in _kisilerRepository.GetAll().Where(x => x.id == kiraci_id)
                             where u.id == kiraci_id
                             select new EKisiDTO
                             {

                                 tc = u.tc,
                                 tel1 = u.tel1,
                                 eposta = u.eposta,
                                 adres = u.adres,


                             }).FirstOrDefault();
            return gelenVeri;
        }

        public List<EKisiDTO> KisiBagimsizBolumleri(int kisiId)
        {
            try
            {
                using (var context = new MyArchContext())
                {

                    var sayacTipi = context.Database.SqlQuery<EKisiDTO>("select blok_adi,daire_numarasi,bagimsiz_id from bagimsiz_bolumler as bgb left join bagimsiz_bolum_kisiler as bbk on bbk.bagimsiz_id=bgb.id WHERE bbk.kisi_id = '" + kisiId + "' ").ToList();
                    return sayacTipi;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EKisiDTO> KisiBorclariGetir(int id)
        {
            using (var context = new MyArchContext())
            {

                var sayacTipi = context.Database.SqlQuery<EKisiDTO>("select br.id,tarih,sonodeme_tarihi,blok_adi,daire_numarasi,aciklama,borc,alacak,br.bakiye,islem_turu,borclandirma_turu,tahsilat_durumu,br.para_birimi,Sum([BORC] - [ALACAK]) OVER (ORDER BY  br.id) AS [BAKIYE]   from bagimsiz_bolumler as bb left join hesap_hareket as br on br.bagimsiz_id=bb.id WHERE kisi_id ='" + id+ "' and br.para_birimi = 'TL' ORDER BY id DESC").ToList();
                return sayacTipi;
            }
        }

        public List<EKisiDTO> KisiBorclariGetirDolar(int id)
        {
            using (var context = new MyArchContext())
            {

                var sayacTipi = context.Database.SqlQuery<EKisiDTO>("select br.id,tarih,sonodeme_tarihi,blok_adi,daire_numarasi,aciklama,borc,alacak,br.bakiye,islem_turu,borclandirma_turu,tahsilat_durumu,br.donem,br.para_birimi,Sum([BORC] - [ALACAK]) OVER (ORDER BY  br.id) AS [BAKIYE]   from bagimsiz_bolumler as bb left join hesap_hareket as br on br.bagimsiz_id=bb.id WHERE kisi_id ='" + id + "' and br.para_birimi = 'USD' ORDER BY id DESC").ToList();
                return sayacTipi;
            }
        }

        public List<EKisiDTO> KisiGetir()
        {
            //var control = (from u in _kisilerRepository.GetAll()

            //               select new EKisiDTO
            //               {
            //                   id = u.id,
            //                   tc = u.tc,
            //                   cinsiyet = u.cinsiyet,
            //                   isim = u.isim,
            //                   tel1 = u.tel1,
            //                   eposta = u.eposta,
            //                   adres = u.adres,
            //                   ogrenim_durumu = u.ogrenim_durumu,
            //                   meslek = u.meslek,


            //               }).OrderByDescending(x => x.id).ToList();
            //return control;

            using (var context = new MyArchContext())
            {

                var kisiler = context.Database.SqlQuery<EKisiDTO>("SELECT kisiler.id,isim,kisiler.durumu,blok_adi,daire_numarasi,eposta,sifre,kullanici_adi,rutbe,tel1,adres,bagimsiz_bolum_kisiler.aktif,bagimsiz_bolum_kisiler.bagimsiz_id,bagimsiz_bolum_kisiler.tip     FROM bagimsiz_bolum_kisiler RIGHT JOIN bagimsiz_bolumler ON bagimsiz_bolum_kisiler.bagimsiz_id = bagimsiz_bolumler.id RIGHT JOIN kisiler ON kisiler.id =bagimsiz_bolum_kisiler.kisi_id where kisiler.rutbe != 'admin'  ").ToList();
                return kisiler;
            }
        }

        public EKisiDTO KisiGetirId(int id)
        {
            var control = (from u in _kisilerRepository.GetAll()
                           where u.id==id
                           select new EKisiDTO
                           {
                               id = u.id,
                               tc = u.tc,
                               cinsiyet = u.cinsiyet,
                               isim = u.isim,
                               tel1 = u.tel1,
                               eposta = u.eposta,
                               adres = u.adres,
                               ogrenim_durumu = u.ogrenim_durumu,
                               meslek = u.meslek,
                               durumu = u.durumu,
                               kullanici_adi = u.kullanici_adi,
                               rutbe = u.rutbe,
                               sifre = u.sifre
                               

                           }).OrderByDescending(x => x.id).SingleOrDefault();
            return control;
        }

        public bool KisiGuncelle(kisiler kisiGuncelle)
        {
            try
            {
                var gelenKisi = _kisilerRepository.Find(kisiGuncelle.id);
                kisiGuncelle.kullanici_adi = gelenKisi.kullanici_adi;

                AutoMapper.Mapper.DynamicMap(kisiGuncelle, gelenKisi);

                _kisilerRepository.Update(gelenKisi);
                if (_uow.SaveChanges() != 0)
                {

                    var gelenGuncelKisi = _kisilerRepository.Find(kisiGuncelle.id);
                    var durum = gelenGuncelKisi.durumu;
                    //bagimsiz bolum kat maliki ve kiracı guncelleme
                    if (durum == "Kiracı")
                    {
                        using (var context = new MyArchContext())
                        {
                            if (context.Database.ExecuteSqlCommand("update bagimsiz_bolumler set kiracı='" + kisiGuncelle.isim + "' where kiraci_id = '" + kisiGuncelle.id + "'") > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                    else if(durum == "Kat maliki")
                    {
                       
                            using (var context = new MyArchContext())
                            {
                                if (context.Database.ExecuteSqlCommand("update bagimsiz_bolumler set katmaliki='" + kisiGuncelle.isim + "' where katmaliki_id = '" + kisiGuncelle.id + "'") > 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception msg)
            {

                throw msg;
            }
        }

        public bool KisiKaydet(kisiler model)
        {
            try
            {
                _kisilerRepository.Insert(model);
                if (_uow.SaveChanges() > 0)
                {
                    return true;
                }
                //if (_uow.SaveChanges() > 0)
                //{
                //    try
                //    {

                        
                        
                //        //SahinHaberlesme sms = new SahinHaberlesme();

                //        //sms.gettoken("5332563356", "7304707");
                //        //string mesaj = "Kullanıcı adı";
                //        //string gonMesaj = Fonksiyonlar.tr2en(mesaj);
                //        //string gonderimzanani = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //        //sms.singlesmsgonder("BETOYAZILIM", gonMesaj, "tr", "0", gonderimzanani, Numaralar);

                //        //System.Net.NetworkCredential cred = new System.Net.NetworkCredential("eyuksek@betoyazilim.com.tr", "EmreTr61*");
                //        //// mail göndermek için oturum açtık

                //        //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(); // yeni mail oluşturduk
                //        //mail.From = new System.Net.Mail.MailAddress("eyuksek@betoyazilim.com.tr", ""); // maili gönderecek hesabı belirttik
                //        //mail.To.Add(model.eposta); // mail gönderilecek adres
                //        //mail.Subject = "Bayraktar İş Merkezi kullanıcı bilgileri"; // mailin konusu
                //        //mail.IsBodyHtml = true; // mail içeriği html olarak gönderilsin
                //        //mail.Body = "Kullanıcı adınız :"+model.kullanici_adi + "<br>" + "Şifreniz :" +model.sifre; // mailin içeriği
                //        //mail.Attachments.Clear(); // mail eklerini temizledik


                //        //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("mail.betoyazilim.com.tr", 587); // smtp servere bağlandık
                //        //smtp.UseDefaultCredentials = false; // varsayılan girişi kullanmadık
                //        //                                    // smtp.EnableSsl = true; // ssl kullanımına izin verdik
                //        //smtp.Credentials = cred; // server üzerindeki oturumumuzu yukarıda belirttiğimiz NetworkCredential üzerinden sağladık.
                //        //smtp.Send(mail); // mailimizi gönderdik.
                //        //                 // smtp yani Simple Mail Transfer Protocol üzerinden maili gönderiyoruz.
                //    }
                //    catch (Exception ex)
                //    {

                //        throw ex;
                //    }

                //    return true;
                //}
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<kisiler> KisiListesiModal()
        {
            var kisiListesiModal = _kisilerRepository.GetAll().Where(x=>x.rutbe != "admin").ToList();
            return kisiListesiModal;
        }

        public int kisiId(int hhId)
        {
            var kisiId = _hesapHaraketRepository.GetAll().Where(x=>x.id==hhId).FirstOrDefault().kisi_id;
            return kisiId;
        }

        public double kisiBakiye(int kisiId)
        {
            using (MyArchContext context = new MyArchContext())
            {
                double bakiye = context.Database.SqlQuery<double>("SELECT SUM(borc-alacak) as bakiye FROM kisiler WHERE id = '"+kisiId+"' ").FirstOrDefault();
                return bakiye;
            }
        }

        public EKisiDTO kisiTlBakiye(int kisiID)
        {
            var tlBakiye = (from u in _kisilerRepository.GetAll()
                           where u.id == kisiID
                           select new EKisiDTO
                           {
                               borc_tl = u.borc_tl,
                               alacak_tl = u.alacak_tl,
                           }).FirstOrDefault();
            return tlBakiye;
        }

        public EKisiDTO kisiDolarBakiye(int kisiID)
        {
            var dolarBakiye = (from u in _kisilerRepository.GetAll()
                            where u.id == kisiID
                            select new EKisiDTO
                            {
                                borc_dolar = u.borc_dolar,
                                alacak_dolar = u.alacak_dolar,
                            }).FirstOrDefault();
            return dolarBakiye;
        }

        public List<bagimsiz_bolum_kisiler> KatmalikiListesi()
        {
            var aktifKatMaliki = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.tip == "Kat maliki" && x.aktif == "True" ).ToList();
            return aktifKatMaliki;
        }

        public List<bagimsiz_bolum_kisiler> KiraciListesi()
        {
            var aktifKatMaliki = _bagimsizBolumKisilerRepository.GetAll().Where(x => x.tip == "Kiracı" && x.aktif == "True").ToList();
            return aktifKatMaliki;
        }

        public List<EKisiDTO> PasifKisiListesi()
        {
            var control = (from u in _kisilerRepository.GetAll().Where(x=>x.rutbe != "admin")

                           select new EKisiDTO
                           {
                               id = u.id,
                               tc = u.tc,
                               cinsiyet = u.cinsiyet,
                               isim = u.isim,
                               tel1 = u.tel1,
                               eposta = u.eposta,
                               adres = u.adres,
                               ogrenim_durumu = u.ogrenim_durumu,
                               meslek = u.meslek,


                           }).OrderByDescending(x => x.id).ToList();
            return control;
        }

        public bool KisiKayitlimi(int id)
        {
            var kisiKontrol = _bagimsizBolumKisilerRepository.GetAll().Where(x=>x.kisi_id==id).ToList();
            if (kisiKontrol.Count > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
