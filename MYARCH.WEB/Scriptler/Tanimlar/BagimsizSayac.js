

$('#blok').change(function () {
    var blokId = $(this).val();

    if (blokId) {
        $.ajax({
            url: "/Tanimlar/BlokDaireleri/",
            type: "POST",
            data: { id: blokId },
            success: function (response) {
                if (response != null) {

                    $('select[id="daireNo"]').empty();
                    for (i = 1; i <= response; i++) {
                        $('select[id="daireNo"]').append('<option>' + i + '</option>');
                    }
                }
                else {
                    $('select[id="daireNo"]').empty();

                }
            }
        });
    }
    else {
        $('select[id="daireNo"]').empty();
    }
})


function BagimsizSayacKaydet() {
    var e = document.getElementById("blok");
    var blok = e.options[e.selectedIndex].text;

    var e = document.getElementById("daireNo");
    var daireNo = e.options[e.selectedIndex].text;



    var e = document.getElementById("anaSayac");
    var anaSayacAdi = e.options[e.selectedIndex].text;
    var e = document.getElementById("anaSayac");
    var anaSayacId = e.options[e.selectedIndex].value;

    var model = {
        blok_adi: blok,
        bagimsiz_bolum: daireNo,
        ana_sayac_adi: anaSayacAdi,
        ana_sayac_id: anaSayacId,
        tesisat_no: document.getElementById("tesisatNo").value,
        ilk_okuma: document.getElementById("ilkOkuma").value,
        aciklama: document.getElementById("aciklama").value
    };

    if (blok == '') {
        Uyari("error", "Lütfen blok  seçimini yapınız");
        return;
    }
    if (daireNo == '') {
        Uyari("error", "Lütfen bağımsız bölümü seçiniz");
        return;
    }
    if (anaSayacAdi == '') {
        Uyari("error", "Lütfen  anasayacı seçiniz");
        return;
    }
    if (anaSayacAdi == '') {
        Uyari("error", "Lütfen Ortak alan dağıtım şeklini seçiniz");
        return;
    }
    if (document.getElementById("tesisatNo").value == '') {
        Uyari("error", "Lütfen  tesisat numarasını giriniz");
        return;
    }
    if (document.getElementById("ilkOkuma").value == '') {
        Uyari("error", "Lütfen  ilk okuma değerini giriniz");
        return;
    }
    if (document.getElementById("aciklama").value == '') {
        Uyari("error", "Lütfen  aciklama  giriniz");
        return;
    }
    else {
        Kaydet("/Tanimlar/BagimsizSayacKaydetIslem", model, "Kayıt işlemi başarılı", "/Tanimlar/Sayac", "İşlem sırasında bir hata meydana geldi")
    }
}
