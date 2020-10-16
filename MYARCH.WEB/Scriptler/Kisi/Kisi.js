

SadeceRakam("#tc");
SadeceRakam("#tc");
$("#tc").attr('maxlength', '11');
TelefonFormatla('#tel1');
TelefonFormatla('#tel2');



function KisiKaydet() {

    var e = document.getElementById("ogrenimDurum");
    var ogrenimDurum = e.options[e.selectedIndex].text;

    var cinsiyet= document.querySelector('input[name="deger"]:checked').value

    //var durumu = document.querySelector('input[name="durumDeger"]:checked').value

    
    var model = {
        isim: document.getElementById("isim").value,
        tc: document.getElementById("tc").value,
        tel1: document.getElementById("tel1").value,
        tel2: document.getElementById("tel2").value,
        eposta: document.getElementById("ePosta").value,
        adres: document.getElementById("adres").value,
        meslek: document.getElementById("meslek").value,
        //kullanici_adi: document.getElementById("kullaniciAdi").value,
        ogrenim_durumu: ogrenimDurum,
        cinsiyet: cinsiyet,
        //durumu: durumu
      
    };


    if (document.getElementById("isim").value == '') {
        Uyari("error", "Lütfen kişi adını giriniz");
        return;
    }
    //if (document.getElementById("kullaniciAdi").value == '') {
    //    Uyari("error", "Lütfen kullanıcı adını giriniz");
    //    return;
    //}
    //if (document.getElementById("tc").value == '') {
    //    Uyari("error", "Lütfen tc numarasını giriniz");
    //    return;
    //}
    if (document.getElementById("tel1").value == '') {
        Uyari("error", "Lütfen varsayılan telefon numarasını giriniz");
        return;
    }
    //if (document.getElementById("ePosta").value == '') {
    //    Uyari("error", "Lütfen e-posta adresini giriniz");
    //    return;
    //}
    if (document.getElementById("adres").value == '') {
        Uyari("error", "Lütfen adresi giriniz giriniz");
        return;
    }
    //if (document.getElementById("meslek").value == '') {
    //    Uyari("error", "Lütfen kişinin mesleğini giriniz  giriniz");
    //    return;
    //}
    //if (ogrenimDurum == '') {
    //    Uyari("error", "Öğrenim durumunu seçiniz");
    //    return;
    //}
    //if (durumu == '') {
    //    Uyari("error", "Kişi durumunu seçiniz");
    //    return;
    //}
    
    else {
        Kaydet("/Kisiler/KaydetIslem", model, "Kayıt işlemi başarılı", "/Kisiler", "İşlem sırasında bir hata meydana geldi")
    }

}