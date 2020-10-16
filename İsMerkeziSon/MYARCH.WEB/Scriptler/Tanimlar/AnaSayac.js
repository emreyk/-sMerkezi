
SadeceRakam("#ortakAlanYuzdesi");
SadeceRakam("#ortakAlanYuzdesiGuncelle");
function SayacKaydet() {
    var e = document.getElementById("sayacTipleri");
    var sayacTipi = e.options[e.selectedIndex].text;
    var e = document.getElementById("sayacTipleri");
    var sayacId = e.options[e.selectedIndex].value;

    var e = document.getElementById("sayacDagilim");
    var sayacDagilim = e.options[e.selectedIndex].text;
    var e = document.getElementById("sayacDagilim");
    var ortakDagitimId = e.options[e.selectedIndex].value;

    var model = {
        sayac_adi: document.getElementById("sayacAdi").value,
        sayac_tipi: sayacTipi,
        tesisat_no: document.getElementById("tesisatNo").value,
        ortak_alan_dagilim: sayacDagilim,
        ortak_alan_yuzde: document.getElementById("ortakAlanYuzdesi").value,
        sayac_id: sayacId,
        ortak_dagitim_id: ortakDagitimId
    };

    if (document.getElementById("sayacAdi").value == '') {
        Uyari("error", "Lütfen sayac  adını giriniz");
        return;
    }
    if (sayacTipi == '') {
        Uyari("error", "Lütfen sayac  tipini seçiniz");
        return;
    }
    if (document.getElementById("tesisatNo").value == '') {
        Uyari("error", "Lütfen tesisat numarasını  adını giriniz");
        return;
    }
    if (sayacDagilim == '') {
        Uyari("error", "Lütfen Ortak alan dağıtım şeklini seçiniz");
        return;
    }
    if (document.getElementById("ortakAlanYuzdesi").value == '') {
        Uyari("error", "Lütfen ortak Alan yüzdesini giriniz");
        return;
    }

    else {
        Kaydet("/Tanimlar/SayacKaydetIslem", model, "Kayıt işlemi başarılı", "/Tanimlar/Sayac", "İşlem sırasında bir hata meydana geldi")
    }

}

function AnaSayacGuncelle() {

    var e = document.getElementById("sayacTipleriGuncelle");
    var sayacTipiGuncelle = e.options[e.selectedIndex].text;
    var e = document.getElementById("sayacTipleriGuncelle");
    var sayacId = e.options[e.selectedIndex].value;


    var e = document.getElementById("sayacDagilimGuncelle");
    var sayacDagilimGuncelle = e.options[e.selectedIndex].text;
    var e = document.getElementById("sayacDagilimGuncelle");
    var dagilimId = e.options[e.selectedIndex].value;



    var sayac_adi = document.getElementById("sayacAdiGuncelle").value;
    var tesisat_no = document.getElementById("tesisatNoGuncelle").value;
    var ortak_alan_yuzde = document.getElementById("ortakAlanYuzdesiGuncelle").value;
    var id = document.getElementById("id").value;


    var model = {
        sayac_adi: sayac_adi,
        tesisat_no: tesisat_no,
        ortak_alan_yuzde: ortak_alan_yuzde,
        id: id,
        sayac_tipi: sayacTipiGuncelle,
        ortak_alan_dagilim: sayacDagilimGuncelle,
        sayac_id: sayacId,
        ortak_dagitim_id: dagilimId

    };


    if (sayac_adi == '') {
        Uyari("error", "Sayac adi boş olamaz");
        return;
    }

    if (sayacTipiGuncelle == '') {
        Uyari("error", "Tip seçimi boş olamaz");
        return
    }
    if (tesisat_no == '') {
        Uyari("error", "Sayac tesisat numarası boş olamaz");
        return;
    }

    if (sayacDagilimGuncelle == '') {
        Uyari("error", "Orta alan dağıtım şekli  boş olamaz");
        return
    }
    if (ortak_alan_yuzde == '') {
        Uyari("error", "Ortak alan yüzdesi   boş olamaz");
        return;
    }

    else {
        Guncelle("/Tanimlar/SayacGuncelleIslem", model, "Güncelleme işlemi başarılı", "/Tanimlar/Sayac", "İşlem sırasında bir hata meydana geldi")
    }
}


function SayacTipiKaydet() {
    var sayacTipi = document.getElementById("sayacTipi").value;

    var model = {
        sayac_tipi: sayacTipi,

    };


    if (sayacTipi == '') {
        Uyari("error", "Sayaç tipi boş bırakılamaz");

    }

    else {
        Kaydet("/Tanimlar/SayacTipleriKaydet", model, "Kayıt işlemi başarılı", "/Tanimlar/SayacTipleri", "İşlem sırasında bir hata meydana geldi")
    }

}


function SayacTipiGuncelle() {

    var sayacTipiGuncelle = document.getElementById("sayacTipiGuncelle").value;
    var id = document.getElementById("id").value;

    var model = {
        sayac_tipi: sayacTipiGuncelle,
        id: id
    };


    if (sayacTipiGuncelle == '') {
        Uyari("error", "Saya. tipi boş bırakılamaz");
    }

    else {
        Guncelle("/Tanimlar/SayacTipGuncelle", model, "Güncelleme işlemi başarılı", "/Tanimlar/SayacTipleri", "İşlem sırasında bir hata meydana geldi")
    }


}

function OrtakAlanKaydet() {
    var ortakAlanAdi = document.getElementById("ortakAlanAdi").value;

    var model = {
        ortak_dagitim_adi: ortakAlanAdi,

    };


    if (ortakAlanAdi == '') {
        Uyari("error", "Ortak alan dağıtım şekli boş bırakılamaz");

    }

    else {
        Kaydet("/Tanimlar/OrtakAlanKaydet", model, "Kayıt işlemi başarılı", "/Tanimlar/OrtakAlanDagitim", "İşlem sırasında bir hata meydana geldi")
    }

}


function OrtakAlanGuncelle() {

  
    var ortakAlanGuncelle = document.getElementById("ortakAlanGuncelle").value;
    var id = document.getElementById("id").value;

    var model = {
        ortak_dagitim_adi: ortakAlanGuncelle,
        id: id
    };

    if (ortakAlanGuncelle == '') {
        Uyari("error", "Saya. tipi boş bırakılamaz");
    }

    else {
        Guncelle("/Tanimlar/OrtakAlanGuncelle", model, "Güncelleme işlemi başarılı", "/Tanimlar/OrtakAlanDagitim", "İşlem sırasında bir hata meydana geldi")
    }

}