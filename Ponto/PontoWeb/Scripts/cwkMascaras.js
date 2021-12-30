$(function () {
    $('.MascDate').mask('00/00/0000');
    $('.time, .timeL').mask('00:00');
    $('.time3, .time3L').mask('000:00', { reverse: true });
    $('.time#3, .time#3L').mask('#00:00', { reverse: true });
    $('.time5, .time5L').mask('00000:00');
    $('.time#5, .time#5L').mask('###00:00');
    $('.timeHMS').mask('00:00:00');
    $('.date_time').mask('00/00/0000 00:00:00');
    $('.MascDataTime').mask('00/00/0000 00:00');
    $('.cep').mask('00000-000');
    $('.cei').mask('00.000.00000/00');
    $('.cnpj').mask('00.000.000/0000-00');
    $('.phone').mask('0000-0000');
    $('.phone_with_ddd').mask('(00) 0000-0000');
    $('.phone_us').mask('(000) 000-0000');
    $('.mixed').mask('AAA 000-S0S');
    $('.ip_address').mask('099.099.099.099');
    $('.percent').mask('##0,00%', { reverse: true });
    $('.intpercent').mask('000');
    $('.int2').mask('00');
    $('.decimalPercent').mask("#.##0,00", { reverse: true, maxlength:true });
    $('.decimalPercent1').mask("#,0#", { maxlength: true });
    
    var options = {
        onComplete: function (cep) {
            cwkErro('CEP Completed!:' + cep);
        },
        onKeyPress: function (cep, event, currentField, options) {
            cwkErro('An key was pressed!:', cep, ' event: ', event,
                        'currentField: ', currentField, ' options: ', options);
        },
        onChange: function (cep) {
            cwkErro('cep changed! ', cep);
        }
    };

    $('.cep_with_callback').mask('00000-000', options);

    $('.crazy_cep').mask('00000-000', {
        onKeyPress: function (cep) {
            var masks = ['00000-000', '0-00-00-00'];
            mask = (cep.length > 7) ? masks[1] : masks[0];
            $('.crazy_cep').mask(mask, this);
        }
    });

    $('.cpf').mask('000.000.000-00', { reverse: true });
    $('.pis').mask('000.00000.00-0', { reverse: true });
    $('.money').mask('#.##0,00', { reverse: true, maxlength: false });

    var SaoPauloCelphoneMask = function (phone, e, currentField, options) {
        return phone.match(/^(\(?11\)? ?9(5[0-9]|6[0-9]|7[01234569]|8[0-9]|9[0-9])[0-9]{1})/g) ? '(00) 00000-0000' : '(00) 0000-0000';
    };

    $(".sp_celphones").mask('(00) 00009-0000');

    $(".bt-mask-it").click(function () {
        $(".mask-on-div").mask("000.000.000-00");
        $(".mask-on-div").fadeOut(500).fadeIn(500)
    })

    $('pre').each(function (i, e) { hljs.highlightBlock(e) });
});

/*Configurações Calendário(datepicker) Bootstrap*/
var optionsDP = new Array();
optionsDP['language'] = 'pt-BR';
optionsDP['format'] = "dd/mm/yyyy";
optionsDP['todayBtn'] = "linked";
optionsDP['autoclose'] = "true";
optionsDP['todayHighlight'] = "true";
optionsDP['orientation'] = "auto";
optionsDP['startDate'] = '01/01/2000';
optionsDP['endDate'] = '31/12/2099';
function setaDatePiker(idDatePiker) {
    $(idDatePiker).datepicker(optionsDP);
}

$(function () {
    $('.datepickerpt:enabled:not([readonly])').datepicker(optionsDP);
});

function corrigeDatePicker() {
    $('.datepickerpt:enabled:not([readonly])').datepicker(optionsDP);
    $('.datepickerpt:enabled:not([readonly])').datepicker('show').datepicker('hide');
};

//Função utilizada para permitir digitar apenas números no textbox, exemplo de uso na página de configuração geral.
function apenasNumero(e) {
    var tecla = (e.which) ? e.which : event.keyCode;
    if ((tecla > 31) && (tecla < 48 || tecla > 57)) {
        return false;
    }
    return true
}

function apenasNumeroInput(e) {
    {
        e.value = e.value.replace(/\D/g, '');
        //e.value = trimChar(e.value, '0');
    }
}

function trimChar(string, charToRemove) {
    while (string.charAt(0) == charToRemove) {
        string = string.substring(1);
        console.log(string);
    }
    return string;
}

Number.prototype.padDigit = function () {
    return (this < 10) ? '0' + this : this;
}