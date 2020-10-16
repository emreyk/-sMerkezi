

SadeceRakam("#tcNo");
SadeceRakam("#tcNo");
$("#tcNo").attr('maxlength', '11');
TelefonFormatla('#personelTelefon');

function PersonelKaydet() {

    var model = {
        personel_adi: document.getElementById("personelAdi").value,
        personel_tc: document.getElementById("tcNo").value,
        personel_gorev: document.getElementById("personelGorev").value,
        personel_email: document.getElementById("personelEmail").value,
        personel_tel: document.getElementById("personelTelefon").value,
        personel_giristarihi: document.getElementById("personelGirisTarih").value,
        personel_cikistarihi: document.getElementById("personelCikisTarih").value,
        personel_maas: document.getElementById("personelMaas").value,
        personel_cinsiyet: document.querySelector('input[name="deger"]:checked').value
    };


    if (document.getElementById("personelAdi").value == '') {
        Uyari("error", "Lütfen personel adını giriniz");
        return;
    }

    if (document.getElementById("tcNo").value == '') {
        Uyari("error", "Lütfen tc numarasını giriniz");
        return;
    }
    if (document.getElementById("personelGorev").value == '') {
        Uyari("error", "Lütfen personel görevini giriniz");
        return;
    }
    if (document.getElementById("personelEmail").value == '') {
        Uyari("error", "Lütfen personel email adresini giriniz");
        return;
    }
    if (document.getElementById("personelTelefon").value == '') {
        Uyari("error", "Lütfen personel telefonunu  giriniz");
        return;
    }
    if (document.getElementById("personelMaas").value == '') {
        Uyari("error", "Lütfen personel maaşını  giriniz");
        return;
    }
    else {
        Kaydet("/Tanimlar/PersonelKaydetIslem", model, "Kayıt işlemi başarılı", "/Tanimlar/Personel", "İşlem sırasında bir hata meydana geldi")
    }

}