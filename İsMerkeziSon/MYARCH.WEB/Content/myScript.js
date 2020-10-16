Noty.overrideDefaults({
    theme: 'limitless',
    layout: 'topRight',
    type: 'alert',
    timeout: 2500
});
function Uyari(tip, baslik, mesaj, position) {
    //if (position == null) position = "top-center";
    //toastr.options = {
    //    "closeButton": true,
    //    "debug": false,
    //    "progressBar": true,
    //    "preventDuplicates": false,
    //    "positionClass": "toast-" + position,
    //    "onclick": null,
    //    "showDuration": "400",
    //    "hideDuration": "1000",
    //    "timeOut": "7000",
    //    "extendedTimeOut": "1000",
    //    "showEasing": "swing",
    //    "hideEasing": "linear",
    //    "showMethod": "fadeIn",
    //    "hideMethod": "fadeOut"
    //}
    //toastr[tip](mesaj, baslik);
    if (tip == "error") {
        tip = "danger";
    }
    new Noty({
        theme: ' alert bg-' + tip +'  text-white alert-styled-left p-0',
        text: baslik,
        type: tip,
        progressBar: true,
        closeWith: ['button']
    }).show();
}



function Kaydet(url,data,MesajBasarili,YonlencekUrl,MesajBasarisiz) {
    $.ajax({
        url: url,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
         data: JSON.stringify(data),
        success: function (response) {
            if (response == true) {

             
                Uyari("success", MesajBasarili);
                setTimeout(function () {

                    window.location.href = YonlencekUrl;

                }, 1000);

            }

            else {

                Uyari("error", MesajBasarisiz);
            }

        }
    });
}


function Sil(url, id, MesajBasarili, MesajBasarisiz) {
    $.ajax({
        url: url,
        type: "POST",
        data: { id: id },
        success: function (response) {
            if (response == true) {

                Uyari("success", MesajBasarili);
            }
            else {
                Uyari("error", MesajBasarisiz);

            }
        }
    });
}


function Guncelle(url, data, MesajBasarili, YonlencekUrl, MesajBasarisiz) {
    $.ajax({
        url: url,
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function (response) {
            if (response == true) {

              
                Uyari("success", MesajBasarili);
                setTimeout(function () {

                    window.location.href = YonlencekUrl;

                }, 1000);

            }

            else {
                Uyari("error", MesajBasarisiz);
            }

        }
    });
}


function SadeceRakam(inputId) {
 $(inputId).keydown(function (e) {

        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||

            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||

            (e.keyCode >= 35 && e.keyCode <= 40)) {
            
            return;
        }

        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) && e.keyCode != 188) {
            e.preventDefault();
        }
    });
}


function TelefonFormatla(inputId) {

        var phones = [{ "mask": "(###)-#######" }, { "mask": "(###)-#######" }];
        $(inputId).inputmask({
            mask: phones,
            greedy: false,
            definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
        });
}


function NoktaEngel(inputId) {
    $(inputId).keydown(function (e) {
        if (e.keyCode == 190)
            return false;
    });
}

function DisiableTrue(inputId) {
    document.getElementById(inputId).disabled = true;
}