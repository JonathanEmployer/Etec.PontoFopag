function AcertaRelogio() {
    var hora = $("#DataHoraBatida").text();
    if (hora.length != 0) {
        var hr = hora.split(':');
        var d = new Date();  // Criando uma data
        d.setHours(+hr[0]);  // Colocando a hora
        d.setMinutes(hr[1]);
        d.setSeconds(hr[2]);
        d.setSeconds(d.getSeconds() + 1);
        $("#DataHoraBatida").text(formataHHMMSS(d));
    }
}

function getRetornoApiLatLon(latitude, longitude) {
    lat = latitude;
    lon = longitude;
    $.ajax({
        url: '/Registrador/GetDataHoraLocalizacao',
        type: 'GET',
        dataType: 'json',
        data: {
            'lat': latitude,
            'lon': longitude
        },
        cache: false,
        success: function (ret) {
            if (ret.Successo == false) {
                exibeAlerta(1, 'Não foi possível exibir o horário de acordo com sua localização. Exibindo Hora do servidor Pontofopag');
            }
            else {
                escondeAlerta();
            }
            var latitudeDecFix = parseFloat(latitude).toFixed(8).replace(".", ",");
            var longitudeDecFix = parseFloat(longitude).toFixed(8).replace(".", ",");
            $("#Latitude").val(latitudeDecFix != NaN ? latitudeDecFix : "");
            $("#Longitude").val(longitudeDecFix != NaN ? longitudeDecFix : "");
            $("#DataHoraBatida").text(ret.dataTimeZone);
            $("#TimeZone").val(ret.timezone);
            AcertaRelogio();
        },
        error: function (ex) {
            exibeAlerta(1, "Não foi possível exibir a hora do servidor.");
        }
    });
}

function getLocation() {
    exibeAlerta(0, 'Obtendo Localização, enquanto aguardamos sua localização é possível registrar o ponto normalmente, será computada a hora do servidor do pontofopag!');
    navigator.geolocation.getCurrentPosition(
       function (position) {
           showPosition(position);
       },
       function (error) {
           var latitude = null;
           var longitude = null;
           getRetornoApiLatLon(latitude, longitude);
       });
}

function showPosition(position) {
    var latitude;
    var longitude;
    if (position == undefined) {
        latitude = null;
        longitude = null;
        getRetornoApiLatLon(latitude, longitude);
    }
    else {
        latitude = position.coords.latitude;
        longitude = position.coords.longitude;
        getRetornoApiLatLon(latitude, longitude);
    }
}

function formataHHMMSS(d) {
    hours = formata2Digitos(d.getHours());
    minutes = formata2Digitos(d.getMinutes());
    seconds = formata2Digitos(d.getSeconds());
    return hours + ":" + minutes + ":" + seconds;
};

function formata2Digitos(n) {
    return n < 10 ? '0' + n : n;
};

function PrencheData() {
    var d = new Date();
    document.getElementById("date").innerHTML = d.toLocaleDateString();
}

function AtualizaDados() {
    PrencheData()
    getLocation();
}


$(function () {
    // hide the wrapper div on load if there's no server-side validation message
    $('div.valmsg-wrapper').not(':has(span.field-validation-error)').hide();
});


/// Tipo: 0 = Alerta, 1 = Erro
/// Mensagem: Mensagem a ser exibida
function exibeAlerta(tipo, mensagem) {
    escondeAlerta();
    $('#lbMensagemHora').text(mensagem);
    switch (tipo) {
        case 1: {
            $("#divAlerta").addClass("alert-warning");
            $("#divAlerta").removeClass("alert-danger");
        }
        case 2: {
            $("#divAlerta").removeClass("alert-warning");
            $("#divAlerta").addClass("alert-danger");

        }
    }
    $("#divAlerta").show();
}

function escondeAlerta() {
    $("#divAlerta").hide();
}