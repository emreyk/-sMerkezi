

function SessionKontolEt() {
    $.ajax({
        url: "/Login/SessionKontrolEt",
        type: "POST",
        dataType: "json",
        success: function (res) {
            if (res) {
             
                console.log('Session doğrulama başarılı');
            } else {
            
                console.log('Session doğrulama başarısız');
            }
        }
    });
}


var simdikiZaman = new Date();
setTimeout(function () {
    //console.log('Şu anki saniye    : ' + simdikiZaman.getSeconds());
    //console.log('SetTimeout süresi : ' + ((60 - simdikiZaman.getSeconds()) * 1000));
    setInterval(SessionKontolEt, 60000);
    SessionKontolEt();
}, (60 - simdikiZaman.getSeconds()) * 1000);