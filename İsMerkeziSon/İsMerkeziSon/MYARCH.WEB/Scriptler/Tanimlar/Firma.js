SadeceRakam("#acilisBakiyesi");
TelefonFormatla('#telefon');


function FirmaKaydet() {

    var model = {
        firma_adi: document.getElementById("firmaAdi").value,
        telefon: document.getElementById("telefon").value,
        email: document.getElementById("ePosta").value,
        adres: document.getElementById("adres").value,
        yetkili_kisi: document.getElementById("yetkiliKisi").value,
        acilis_tarihi: document.getElementById("tarih").value,
        acilis_bakiyesi: document.getElementById("acilisBakiyesi").value,

    };


    if (document.getElementById("firmaAdi").value == '') {
        Uyari("error", "Lütfen firma adını giriniz");
        return;
    }

    if (document.getElementById("telefon").value == '') {
        Uyari("error", "Lütfen telefon numarasını giriniz");
        return;
    }
    if (document.getElementById("ePosta").value == '') {
        Uyari("error", "Lütfen email bilgisini giriniz");
        return;
    }
    if (document.getElementById("adres").value == '') {
        Uyari("error", "Lütfen adres bilgisini giriniz");
        return;
    }
    if (document.getElementById("yetkiliKisi").value == '') {
        Uyari("error", "Lütfen yetkili kişi  giriniz");
        return;
    }
    if (document.getElementById("acilisBakiyesi").value == '') {
        Uyari("error", "Lütfen bakiye  giriniz");
        return;
    }
    else {
        Kaydet("/Tanimlar/FirmaKaydetIslem", model, "Kayıt işlemi başarılı", "/Tanimlar/Firma", "İşlem sırasında bir hata meydana geldi")
    }

}

function FirmaGuncelle() {

    var model = {
        id: document.getElementById("id").value,
        firma_adi: document.getElementById("firmaAdiGuncelle").value,
        telefon: document.getElementById("telefonGuncelle").value,
        email: document.getElementById("ePostaGuncelle").value,
        adres: document.getElementById("adresGuncelle").value,
        yetkili_kisi: document.getElementById("yetkiliKisiGuncelle").value,
        acilis_tarihi: document.getElementById("tarihGuncelle").value,
        acilis_bakiyesi: document.getElementById("acilisBakiyesiGuncelle").value,
    };


    if (document.getElementById("firmaAdiGuncelle").value == '') {
        Uyari("error", "Lütfen firma adını giriniz");
        return;
    }

    if (document.getElementById("telefonGuncelle").value == '') {
        Uyari("error", "Lütfen telefon numarasını giriniz");
        return;
    }
    if (document.getElementById("ePostaGuncelle").value == '') {
        Uyari("error", "Lütfen email bilgisini giriniz");
        return;
    }
    if (document.getElementById("adresGuncelle").value == '') {
        Uyari("error", "Lütfen adres bilgisini giriniz");
        return;
    }
    if (document.getElementById("yetkiliKisiGuncelle").value == '') {
        Uyari("error", "Lütfen yetkili kişi  giriniz");
        return;
    }
    if (document.getElementById("acilisBakiyesiGuncelle").value == '') {
        Uyari("error", "Lütfen bakiye  giriniz");
        return;
    }
    else {
       
        Guncelle("/Tanimlar/FirmaGuncelleIslem", model, "Kayıt işlemi başarılı", "/Tanimlar/Firma", "İşlem sırasında bir hata meydana geldi")
    }
}


