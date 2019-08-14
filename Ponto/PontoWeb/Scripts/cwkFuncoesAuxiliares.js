function lpad(str, tamanho, char) {
    str = str.toString();
    return str.length < tamanho ? lpad(char + str, tamanho, char) : str;
}

function rpad(str, tamanho, char) {   
    str = str.toString();
    return str.length < tamanho ? rpad(str + char, tamanho, char) : str;
}

/*Tratamentos de hora*/
$(function () {
    $(".time, .timeL").blur(function () {
        var valor = formataHora($(this).val(), 2, 2);
        if (valor != $(this).val()) {
            $(this).val(valor);
            $(this).blur();
        }
        else {
            $(this).val(valor);
        }
    });

    $(".time3, .time3L").blur(function () {
        var valor = formataHora($(this).val(), 3, 2);
        if (valor != $(this).val()) {
            $(this).val(valor);
            $(this).blur();
        }
        else {
            $(this).val(valor);
        }
    });

    $(".time5, .time5L").blur(function () {
        var valor = formataHora($(this).val(), 5, 2);
        if (valor != $(this).val()) {
            $(this).val(valor);
            $(this).blur();
        }
        else {
            $(this).val(valor);
        }
    });
});

// Valida se o navegador tem compatibilidade com as funções do ES6(ECMAScript 6)
function suportaES6() {
    var supportsES6 = function () {
        try {
            new Function("(a = 0) => a");
            return true;
        }
        catch (err) {
            return false;
        }
    }();

    if (!supportsES6) {
        $.getScript("https://cdnjs.cloudflare.com/ajax/libs/bowser/1.9.3/bowser.min.js", function (data, textStatus, jqxhr) {
            var mensagem = 'Seu navegador <b>' + bowser.name + '</b> versão: ' + bowser.version + ' do sistema operacional ' + bowser.osname + ' não é compatível com essa página, algumas funcionalidades podem ter sido removidas, desabilitadas ou não funcionar corretamente';

            if (bowser.name.indexOf('Chrome') !== -1 || bowser.name.indexOf('Firefox') !== -1 || bowser.name.indexOf('Edge') !== -1 || bowser.name.indexOf('Opera') !== -1) {
                mensagem = mensagem + ', atualize seu navegador.';
            }
            else {
                mensagem = mensagem + ', utilize outro navegador.';
            }
            cwkAlertaTit('Versão do Navegador Incompativel', mensagem);
        });
        return false;
    }
    else {
        return true;
    }
}

/*
 calc_digitos_posicoes
 
 Multiplica dígitos vezes posições
 
 @param string digitos Os digitos desejados
 @param string posicoes A posição que vai iniciar a regressão
 @param string soma_digitos A soma das multiplicações entre posições e dígitos
 @return string Os dígitos enviados concatenados com o último dígito
*/
function calc_digitos_posicoes(digitos, posicoes, soma_digitos) {
 
    if ((posicoes == undefined) ||
        (posicoes == null)) {
        posicoes = 10;
    }
    soma_digitos = 0;
    // Garante que o valor é uma string
    digitos = digitos.toString();
 
    // Faz a soma dos dígitos com a posição
    // Ex. para 10 posições:
    //   0    2    5    4    6    2    8    8   4
    // x10   x9   x8   x7   x6   x5   x4   x3  x2
    //   0 + 18 + 40 + 28 + 36 + 10 + 32 + 24 + 8 = 196
    for ( var i = 0; i < digitos.length; i++  ) {
        // Preenche a soma com o dígito vezes a posição
        soma_digitos = soma_digitos + ( digitos[i] * posicoes );
 
        // Subtrai 1 da posição
        posicoes--;
 
        // Parte específica para CNPJ
        // Ex.: 5-4-3-2-9-8-7-6-5-4-3-2
        if ( posicoes < 2 ) {
            // Retorno a posição para 9
            posicoes = 9;
        }
    }
 
    // Captura o resto da divisão entre soma_digitos dividido por 11
    // Ex.: 196 % 11 = 9
    soma_digitos = soma_digitos % 11;
 
    // Verifica se soma_digitos é menor que 2
    if ( soma_digitos < 2 ) {
        // soma_digitos agora será zero
        soma_digitos = 0;
    } else {
        // Se for maior que 2, o resultado é 11 menos soma_digitos
        // Ex.: 11 - 9 = 2
        // Nosso dígito procurado é 2
        soma_digitos = 11 - soma_digitos;
    }
 
    // Concatena mais um dígito aos primeiro nove dígitos
    // Ex.: 025462884 + 2 = 0254628842
    var cpf = digitos + soma_digitos;
 
    // Retorna
    return cpf;
    
} // calc_digitos_posicoes
 
    /*
     Valida CPF
     
     Valida se for CPF
     
     @param  string cpf O CPF com ou sem pontos e traço
     @return bool True para CPF correto - False para CPF incorreto
    */
function valida_cpf(valor) {
 
        // Garante que o valor é uma string
        valor = valor.toString();
    
        // Remove caracteres inválidos do valor
        valor = valor.replace(/[^0-9]/g, '');
 
 
        // Captura os 9 primeiros dígitos do CPF
        // Ex.: 02546288423 = 025462884
        var digitos = valor.substr(0, 9);
 
        // Faz o cálculo dos 9 primeiros dígitos do CPF para obter o primeiro dígito
        var novo_cpf = calc_digitos_posicoes( digitos );
 
        // Faz o cálculo dos 10 dígitos do CPF para obter o último dígito
        var novo_cpf = calc_digitos_posicoes( novo_cpf, 11 );
 
        // Verifica se o novo CPF gerado é idêntico ao CPF enviado
        if ( novo_cpf === valor ) {
            // CPF válido
            return true;
        } else {
            // CPF inválido
            return false;
        }
    
} // valida_cpf
 
    /*
     valida_cnpj
     
     Valida se for um CNPJ
     
     @param string cnpj
     @return bool true para CNPJ correto
    */
function valida_cnpj (valor) {
 
        // Garante que o valor é uma string
        valor = valor.toString();
    
        // Remove caracteres inválidos do valor
        valor = valor.replace(/[^0-9]/g, '');
 
    
        // O valor original
        var cnpj_original = valor;
 
        // Captura os primeiros 12 números do CNPJ
        var primeiros_numeros_cnpj = valor.substr( 0, 12 );
 
        // Faz o primeiro cálculo
        var primeiro_calculo = calc_digitos_posicoes( primeiros_numeros_cnpj, 5 );
 
        // O segundo cálculo é a mesma coisa do primeiro, porém, começa na posição 6
        var segundo_calculo = calc_digitos_posicoes( primeiro_calculo, 6 );
 
        // Concatena o segundo dígito ao CNPJ
        var cnpj = segundo_calculo;
 
        // Verifica se o CNPJ gerado é idêntico ao enviado
        if ( cnpj === cnpj_original ) {
            return true;
        }
    
        // Retorna falso por padrão
        return false;
    
    } // valida_cnpj


function formataHora(valor, digHora, digMin) {
    if (valor && valor.indexOf("-") == -1)
    {
        var tamanho = valor.length;
        var pos = valor.indexOf(":");
        if (pos == -1) { pos = tamanho }
        var horas = valor.substring(0, pos);
        var minutos = valor.substring(pos + 1, tamanho);
        if (parseInt(minutos) > 59) {
            minutos = parseInt(minutos) - 60;
            var horamaxima = lpad(9, digHora, 9);
            if (parseInt(horas) < horamaxima) {
                horas = parseInt(horas) + 1;
            }
            else {
                minutos = 59;
            }
        }
        horas = lpad(horas, digHora, 0);
        minutos = rpad(minutos, digMin, 0);
        valor = (horas + ":" + minutos);
    }
    return valor;
}

function formataHora24(valor, digHora, digMin) {
    if (valor && valor.indexOf("-") == -1) {
        var tamanho = valor.length;
        var pos = valor.indexOf(":");
        if (pos == -1) { pos = tamanho }
        var horas = valor.substring(0, pos);
        var minutos = valor.substring(pos + 1, tamanho);
        if (parseInt(minutos) > 59) {
            minutos = parseInt(minutos) - 60;
            var horamaxima = lpad(9, digHora, 9);
            if (parseInt(horas) < horamaxima) {
                horas = parseInt(horas) + 1;
                if (horas >= 24) {
                    horas = 0;
                }
            }
            else {
                minutos = 59;
            }
        }
        if (horas >= 24) {
            horas = 0;
        }
        horas = lpad(horas, digHora, 0);
        minutos = rpad(minutos, digMin, 0);
        valor = (horas + ":" + minutos);
    }
    return valor;
}

function ConvertHoraMinuto(hora) {
    var min = null;
    if (hora) {
        if (hora === '--:--' || hora === '---:--') {
            hora = '00:00';
        }
        var tt = hora.split(":");
        min = tt[0] * 60 + tt[1] * 1;
    }
    return min;
}

function ConvertMinutoHora(min, casasHora) {
    if (min) {
        var horas = Math.floor(min / 60);
        var minutos = min % 60;
        var hhmm = lpad(horas, casasHora, 0) + ":" + lpad(minutos, 2, 0);
        return hhmm;
    }
}

function SaldoHoraMin(Hora1, Hora2) {
    return ConvertHoraMinuto(Hora1) - ConvertHoraMinuto(Hora2);
}

function SaldoHoraMinPositivo(Hora1, Hora2) {
    var min = ConvertHoraMinuto(Hora1) - ConvertHoraMinuto(Hora2);
    if (min < 0) {
        min = min * -1;
    }
    return min;
}

//Função recebe dois parametros de horas e um de resultado, e quando pelo menos 2 parametros forem passados a rotina calcula o terceiro baseado na soma
// Exemplo h1=120, h2=60, res=Null(retorna 180) ou h1=120, h2=Null(retorna 60), res=180 ou h1=Null(retorna 120), h2=60, res=180
//Exemplo de uso
//var hora1min = ConvertHoraMinuto($("#txtHoraAd1").val());
//var hora2min = ConvertHoraMinuto($("#txtHoraAd2").val());
//var resultadomin = ConvertHoraMinuto($("#txtResultadoAd").val());
//var retorno = somaHoras(hora1min, hora2min, resultadomin);
//$("#txtHora1").val(ConvertMinutoHora(retorno.hora1, 3));
//$("#txtHora2").val(ConvertMinutoHora(retorno.hora2, 3));
//$("#txtResultado").val(ConvertMinutoHora(retorno.resultado, 3));
function somaHoras(hora1min, hora2min, resultadomin) {
    if (hora1min && hora2min) {
        resultadomin = parseInt(hora1min) + parseInt(hora2min);
    }
    else {
        if (hora1min && resultadomin) {
            hora2min = parseInt(hora1min) - parseInt(resultadomin);
        }
        else if (hora2min && resultadomin) { hora1min = parseInt(resultadomin) - parseInt(hora2min) };
    }

    if (!isNaN(hora1min) && hora1min) {
        hora1min = Math.abs(hora1min)
    }

    if (!isNaN(hora2min) && hora2min) {
        hora2min = Math.abs(hora2min)
    }

    if (!isNaN(resultadomin) && resultadomin) {
        resultadomin = Math.abs(resultadomin)
    }

    return {
        hora1: hora1min,
        hora2: hora2min,
        resultado: resultadomin
    };
}
//Função recebe dois parametros de horas e um de resultado, e quando pelo menos 2 parametros forem passados a rotina calcula o terceiro baseado na subtração
//Exemplo de uso na rotina acima
function DiferencaHoras(hora1min, hora2min, resultadomin) {
    if (hora1min && hora2min) {
        resultadomin = parseInt(hora2min) - parseInt(hora1min);
    }
    else {
        if (hora1min && resultadomin) {
            hora2min = parseInt(hora1min) - parseInt(resultadomin);
        }
        else if (hora2min && resultadomin) { hora1min = parseInt(hora2min) + parseInt(resultadomin) };
    }

    if (!isNaN(hora1min) && hora1min) {
        hora1min = Math.abs(hora1min)
    }

    if (!isNaN(hora2min) && hora2min) {
        hora2min = Math.abs(hora2min)
    }

    if (!isNaN(resultadomin) && resultadomin) {
        resultadomin = Math.abs(resultadomin)
    }

    return {
        hora1: hora1min,
        hora2: hora2min,
        resultado: resultadomin
    };
}

//Função recebe dois parametros de horas e um de resultado, e quando pelo menos 2 parametros forem passados a rotina calcula o terceiro baseado na Divisao
//Exemplo de uso na rotina acima
function multiplicacaoHoras(hora1min, hora2min, resultadomin) {
    if (hora1min && hora2min) {
        resultadomin =  hora1min * hora2min;
    }
    else {
        if (hora1min && resultadomin) {
            hora2min = parseInt(resultadomin) / parseInt(hora1min);
        }
        else if (hora2min && resultadomin) { hora1min = parseInt(resultadomin) / hora2min };
    }

    if (!isNaN(hora1min) && hora1min) {
        hora1min = Math.abs(hora1min)
    }

    if (!isNaN(hora2min) && hora2min) {
        hora2min = Math.abs(hora2min)
    }

    if (!isNaN(resultadomin) && resultadomin) {
        resultadomin = Math.abs(resultadomin)
    }

    return {
        hora1: hora1min,
        hora2: hora2min,
        resultado: resultadomin
    };
}

//Função recebe dois parametros de horas e um de resultado, e quando pelo menos 2 parametros forem passados a rotina calcula o terceiro baseado na Multiplicacao
//Exemplo de uso na rotina acima
function divisaoHoras(hora1min, hora2min, resultadomin) {
    if (hora1min && hora2min) {
        resultadomin = (parseInt(hora1min) / hora2min);
    }
    else {
        if (hora1min && resultadomin) {
            hora2min = parseInt(hora1min) / parseInt(resultadomin);
        }
        else if (hora2min && resultadomin) { hora1min = parseInt(hora2min) * parseInt(resultadomin) };
    }

    if (!isNaN(hora1min) && hora1min) {
        hora1min = Math.abs(hora1min)
    }

    if (!isNaN(hora2min) && hora2min) {
        hora2min = Math.abs(hora2min)
    }

    if (!isNaN(resultadomin) && resultadomin) {
        resultadomin = Math.abs(resultadomin)
    }

    return {
        hora1: hora1min,
        hora2: hora2min,
        resultado: resultadomin
    };
}
function subtraiHoras(horaMaior, horaMenor) {
    if (isNaN(horaMaior) || !horaMaior) {
        horaMaior = 0;
    }
    if (isNaN(horaMenor) || !horaMenor) {
        horaMenor = 0;
    }
    resultadomin = parseInt(horaMaior) - parseInt(horaMenor);
    return resultadomin;
}
//Rotina que retorna Horas a Trabalhar Diurna e Noturna ou Mista
//Parametros:
//entradas: Array com as entradas em minutos ex: var entradas = []; entradas.push(valormin);
//saidas: Array com as entradas em minutos Ex: var saidas = []; saidas.push(valormin);
//inicioNoturno: Horário em que se inicia as horas noturnas em minutos Ex: inicioNoturno = 1320;(22:00)
//fimNoturno: Horário que termina as horas noturnas em minutos Ex: fimNoturno = 300;(05:00)
//tipoHorario: Tipo da Carga Horária, 1 para normal e 2 para mista;
//Exemplo de como pegar o retorno
//var retorno = CalculaCargaHoraria(entradas, saidas, inicioNoturno, fimNoturno, tipo);
//$("#resDiurno").val(ConvertMinutoHora(retorno.horasTrabalhadasDiurnas, 3));
//$("#resNoturno").val(ConvertMinutoHora(retorno.horasTrabalhadasNoturnas, 3));
//$("#resMisto").val(ConvertMinutoHora(retorno.horasTrabalhadasMista, 3));
function CalculaCargaHoraria(entradas, saidas, inicioNoturno, fimNoturno, tipoHorario, CalculaHorarioCafe, PrimeiroPeridoCafe, SegundoPeriodoCafe) {
    var horasTrabalhadasDiurnas = 0;
    var horasTrabalhadasNoturnas = 0;
    var horasTrabalhadasMista = 0;


    var newEntradas = new Array(8);
    var newSaidas = new Array(8);

    for (var index = 0; index < entradas.length; index++) {
        entrada = entradas[index];
        saida = saidas[index];
        if (saida < entrada) {
            if (entrada < inicioNoturno) {
                newEntradas.push(entrada);
                newSaidas.push(inicioNoturno);

                newEntradas.push(inicioNoturno);

            }
            else {
                newEntradas.push(entrada);

            }

            if (saida < fimNoturno) {
                newSaidas.push(saida);

            }
            else {
                newSaidas.push(fimNoturno);

                newEntradas.push(fimNoturno);
                newSaidas.push(saida);
            }
        }
        else {
            newEntradas.push(entrada);
            newSaidas.push(saida);
        }
    }

    for (index = 0; index < newEntradas.length; index++) {
        var entrada = 0;
        var saida = 0;
        var entradaNoturna = null;
        var saidaNoturna = null;
        var entradaDiurna = null;
        var saidaDiurna = null;
        entrada = newEntradas[index];
        saida = newSaidas[index];
        if (entrada > 0 || saida > 0) {
            if (tipoHorario == 2) {
                entradaDiurna = entrada;
                saidaDiurna = saida;
            }
            else {
                if ((inicioNoturno > fimNoturno && ((entrada > inicioNoturno && entrada <= 1439) || (entrada >= 0 && entrada <= fimNoturno))) ||
                    (inicioNoturno < fimNoturno && entrada >= 0 && entrada < fimNoturno)) {
                    entradaNoturna = entrada;
                }
                else {
                    entradaDiurna = entrada;
                }

                if ((inicioNoturno > fimNoturno && ((saida > inicioNoturno && saida <= 1439) || (saida >= 0 && saida <= fimNoturno))) ||
                    (inicioNoturno < fimNoturno && saida >= 0 && saida < fimNoturno)) {
                    saidaNoturna = saida;
                }
                else {
                    saidaDiurna = saida;
                }


                if (entradaNoturna == null && saidaNoturna != null) {
                    saidaDiurna = inicioNoturno;
                    entradaNoturna = inicioNoturno;
                }

                if ((entradaNoturna != null && saidaNoturna == null)) {
                    saidaNoturna = fimNoturno;
                    entradaDiurna = fimNoturno;
                }

            }
            if (saidaDiurna < entradaDiurna) {
                saidaDiurna += 1440;
            }
            if (saidaNoturna < entradaNoturna) {
                saidaNoturna += 1440;
            }

            if (tipoHorario == 2) {
                horasTrabalhadasMista += saidaDiurna - entradaDiurna;
            }
            else {
                horasTrabalhadasDiurnas += saidaDiurna - entradaDiurna;
                horasTrabalhadasNoturnas += saidaNoturna - entradaNoturna;


            }

        }
    }
    var horascafe = 0;
    if (CalculaHorarioCafe && PrimeiroPeridoCafe && (entradas[1] || entradas[1] == 0) && (saidas[0] || saidas[0] == 0)) {
        var entradac = entradas[1];
        var saidac = saidas[0];
        if (entradac < saidac) {
            entradac += 1440;
        }
        var retorno = DiferencaHoras(saidac, entradac, null);
        horascafe += retorno.resultado;
    }
    if (CalculaHorarioCafe && SegundoPeriodoCafe && (entradas[3] || entradas[3] == 0) && (saidas[2] || saidas[2] == 0)) {
        var entradac = entradas[3];
        var saidac = saidas[2];
        if (entradac < saidac) {
            entradac += 1440;
        }
        var retorno = DiferencaHoras(saidac, entradac, null);
        horascafe += retorno.resultado;
    }
    if (tipoHorario == 2) {
        horasTrabalhadasMista += horascafe;
    }
    else {
        horasTrabalhadasDiurnas += horascafe;
    }
    return {
        horasTrabalhadasDiurnas: Math.abs(horasTrabalhadasDiurnas),
        horasTrabalhadasNoturnas: Math.abs(horasTrabalhadasNoturnas),
        horasTrabalhadasMista: Math.abs(horasTrabalhadasMista)
    };
}
/*Fim dos Tratamentos de hora*/

//Limpa o erro adicionado pelo modalstate
//Passar a propriedade name do inputbox que deve limpar o erro
function limpaErro(nome) {
    var msg = $('form [name=' + nome + ']').attr("class");
    if (msg != '' && msg != undefined && msg != null) {
        if (msg.indexOf('input-validation-error') >= 0) {
            var input = $('form [name=' + nome + ']');
            input.addClass('input-validation-valid').removeClass('input-validation-error');
            var field = $('form [data-valmsg-for=' + nome + ']');
            field.addClass('field-validation-valid').removeClass('field-validation-error');
        }
    }
};

function limpaInput(idCampo, idForm) {
    var errorArray = {};
    var campo = $(idCampo);
    var nomeCampo = campo.attr('name');
    var $form = $(idForm);
    validator = $form.validate();
    errorArray[nomeCampo] = null;
    validator.showErrors(errorArray);
    
    if (campo.hasClass("input-validation-error")) {
        campo.addClass('input-validation-valid').removeClass('input-validation-error');
        var field = $(idForm + "[data-valmsg-for='" + nomeCampo + "']");
        field.addClass('field-validation-valid').removeClass('field-validation-error');
    }
};
//Adiciona erro em um input
//Passar a propriedade name do input e o erro que deseja add.
function addErroInput(key, erro) {
    descErr = $('form').find(".field-validation-valid, .field-validation-error")
          .filter("[data-valmsg-for='" + key + "']")
          .removeClass("field-validation-valid")
          .addClass("field-validation-error");

    inputErr = $('form').find("*[name='" + key + "']");

    if (!inputErr.is(":hidden") && !descErr.length) {
        inputErr.parent().append("<span class='field-validation-error' data-valmsg-for='" + key + "' data-valmsg-replace='false'> " + erro + "</span>");
    }
    else {
        descErr.text(erro);
    }
    inputErr.addClass("input-validation-error");
}

/// Verifica se existe alguma mensagem de erro na página
function PaginaValida() {
    if ($('span[class="field-validation-error"]').length > 0) {
        return false;
    }
    else
    {
        return true;
    }
}

/*Recebe um tipo data e retorno a hora no formato HH:MM:SS*/
function formataHHMMSS(d) {
    hours = formata2Digitos(d.getHours());
    minutes = formata2Digitos(d.getMinutes());
    seconds = formata2Digitos(d.getSeconds());
    return hours + ":" + minutes + ":" + seconds;
};

//Adiciona o zero a esquerda quando menor que 10
function formata2Digitos(n) {
    return n < 10 ? '0' + n : n;
};

function diasPeriodo(dataini, datafin) {
    var one_day = 1000 * 60 * 60 * 24;
    var x = dataini.split("/");
    var y = datafin.split("/");
    var date1 = new Date(Date.UTC(x[2], (x[1] - 1), x[0]));
    var date2 = new Date(Date.UTC(y[2], (y[1] - 1), y[0]))
    return _Diff = Math.ceil((date2.getTime() - date1.getTime()) / (one_day)) + 1;
}

function PeriodoMaiorQueAno(dataini, datafin, qtdAnos) {
    var x = dataini.split("/");
    var y = datafin.split("/");
    var dataI = new Date(Date.UTC((parseInt(x[2]) + parseInt(qtdAnos)), (x[1] - 1), x[0]));
    var dataF = new Date(Date.UTC(y[2], (y[1] - 1), y[0]))
    // Se data final for maior que a data inicial acrescida da quantidade de anos passado, significa que o período seleciona é maior que a quantidade em anos permitido
    if (dataF > dataI)
        return true;
    else
        return false;
}

// Funcao utilizada para validar período, exemplo de uso na Tabela de Marcacao
//idDtIni = Id do componente de data inicial
//idDtFin = Id do componente de data final
//idPnl = Id do panel onde está os componentes de data (esse panel receberá a cor vermelha quando o perído estiver inválido e verde quando válido)
//nomeValidation = nome do campo onde seré setada a mensagem de erro
//Idform = Nome do Form onde estão os componentes para que seja realizada a validação
function validaPeriodo(idDtIni, idDtFin, idPnl, nomeValidation, Idform, naoValidaPeriodoMaxMes, maxAno) {
    var errorArray = {};
    errorArray[nomeValidation] = null;
    $(Idform).validate().showErrors(errorArray);
    $(idPnl).removeClass("panel-danger").removeClass("panel-success");
    $(idPnl).addClass("panel-default");
    var vDtIni = $(idDtIni).val();
    var vDtFin = $(idDtFin).val();
    if ((vDtIni != null && vDtIni != "") && (vDtFin != null && vDtFin != "")) {
        var dias = diasPeriodo(vDtIni, vDtFin);
        if (naoValidaPeriodoMaxMes != true && dias > 31) {
            errorArray[nomeValidation] = 'Período Máximo 31 Dias';
            $(Idform).validate().showErrors(errorArray);
            $(idPnl).removeClass("panel-default").removeClass("panel-success");
            $(idPnl).addClass("panel-danger");
            return false;
        }
        else {
            if (dias <= 0) {
                errorArray[nomeValidation] = 'Data Inícial deve ser menor que a Final';
                $(Idform).validate().showErrors(errorArray);
                $(idPnl).removeClass("panel-default").removeClass("panel-success");
                $(idPnl).addClass("panel-danger");
                return false;
            }
            else if (maxAno != '' && maxAno != undefined && maxAno != null && maxAno > 0 && PeriodoMaiorQueAno(vDtIni, vDtFin, maxAno))
            {
                errorArray[nomeValidation] = 'O período entre a data inicial e a final não pode ultrapassar ' + maxAno + ' ano(s)';
                $(Idform).validate().showErrors(errorArray);
                $(idPnl).removeClass("panel-default").removeClass("panel-success");
                $(idPnl).addClass("panel-danger");
            } 
            else
            {
                $(idPnl).removeClass("panel-default").removeClass("panel-danger");
                $(idPnl).addClass("panel-success");
                $('label[for=' + nomeValidation + ']').remove();
                return true;
            }
        }
    }
    else {
        if (($(idDtIni).val() == null || $(idDtIni).val() == "") && ($(idDtFin).val() == null || $(idDtFin).val() == "")) {
            errorArray[nomeValidation] = 'A Data Inicial e Final devem ser preenchidas!';
            $(idDtIni).focus();
        }
        else {

            if ($(idDtIni).val() == null || $(idDtIni).val() == "") {
                errorArray[nomeValidation] = 'A Data Inicial deve ser preenchida!';
                $(idDtIni).focus();
            }
            if ($(idDtFin).val() == null || $(idDtFin).val() == "") {
                errorArray[nomeValidation] = 'A Data Final deve ser preenchida!';
                $(idDtFin).focus();
            }
        }

        $(Idform).validate().showErrors(errorArray);
        $(idPnl).removeClass("panel-default").removeClass("panel-success");
        $(idPnl).addClass("panel-danger");
        return false;
    }
}

//Valida o período, permitindo que o usuário selecione mais de 31 dias para imprimir cartão ponto ou tratar marcações
function validaPeriodoCartaoPontoMarcacao(idDtIni, idDtFin, idPnl, nomeValidation, Idform) {
    var errorArray = {};
    errorArray[nomeValidation] = null;
    $(Idform).validate().showErrors(errorArray);
    $(idPnl).removeClass("panel-danger").removeClass("panel-success");
    $(idPnl).addClass("panel-default");
    var vDtIni = $(idDtIni).val();
    var vDtFin = $(idDtFin).val();
    if ((vDtIni != null && vDtIni != "") && (vDtFin != null && vDtFin != "")) {
        var dias = diasPeriodo(vDtIni, vDtFin);
        if (dias <= 0) {
            errorArray[nomeValidation] = 'Data Inícial deve ser menor que a Final';
            $(Idform).validate().showErrors(errorArray);
            $(idPnl).removeClass("panel-default").removeClass("panel-success");
            $(idPnl).addClass("panel-danger");
            return false;
        }
        else {
            $(idPnl).removeClass("panel-default").removeClass("panel-danger");
            $(idPnl).addClass("panel-success");
            return true;
        }
    }
    else {
        if (($(idDtIni).val() == null || $(idDtIni).val() == "") && ($(idDtFin).val() == null || $(idDtFin).val() == "")) {
            errorArray[nomeValidation] = 'A Data Inicial e Final devem ser preenchidas!';
            $(idDtIni).focus();
        }
        else {

            if ($(idDtIni).val() == null || $(idDtIni).val() == "") {
                errorArray[nomeValidation] = 'A Data Inicial deve ser preenchida!';
                $(idDtIni).focus();
            }
            if ($(idDtFin).val() == null || $(idDtFin).val() == "") {
                errorArray[nomeValidation] = 'A Data Final deve ser preenchida!';
                $(idDtFin).focus();
            }
        }

        $(Idform).validate().showErrors(errorArray);
        $(idPnl).removeClass("panel-default").removeClass("panel-success");
        $(idPnl).addClass("panel-danger");
        return false;
    }
}

// Converter um string dd/MM/yyyy em date
function ConvertDate(date) {
    var dtHr = date.split(" ");
    var parts = dtHr[0].split("/");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}

//Altera string apartir de determinado indice
String.prototype.replaceAt = function (index, character) {
    return this.substr(0, index) + character + this.substr(index + character.length);
}

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}


// Retorna o Código dos componentes de pesquisa que tem o padrão: 4 | Descrição
function getCodigoCampo(idCampo) {
    var valor = $(idCampo).val();
    var codigo = valor.split('|');
    codigo = $.trim(codigo[0]);
    return codigo;
}