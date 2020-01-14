function cwk_AlturaTela() {
    //alert("screen.availHeight => " + screen.availHeight + ' screen.height => ' + screen.height + ' $(document).height() => ' + $(document).height() + ' $(window).height() => ' + $(window).height());
    return $(window).height();
}

function cwk_LarguraTela() {
    return $(document).width();
}

function cwk_AlturaResolucao() {
    return screen.height;
}
function cwk_LarguraResolucao() {
    return screen.width;
}

function utilizadoGridGenerica()
{
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  // If Internet Explorer, return version number
    {
        return 230;
    }
    else  // If another browser, return 0
    {
        return 240;
    }
    //Barra Icones = 70 + Barra Menus = 50 + Rodapé = 44 + Titulo Página = 24 + Footer Botoes = 30 px + Padding da pagina de conteúdo = 30 + Padding Minimo Janela = 4
}

// Retira e redimensiona componentes de acordo com o tamanho da tela
function checkSize() {
    var largura = window.innerWidth || document.body.clientWidth;
    // Retira menu de icones
    if (largura > 900) {
        document.getElementById("divImagem").style.display = "block";
    }
    else {
        document.getElementById("divImagem").style.display = "none";
    }

    if ($("#panelBodyMaximo").length) {
        var tamanhoM = cwk_AlturaTela() - (utilizadoGridGenerica() + 20); // 20 deixa um espaço entre rodapé e pagina
        document.getElementById("panelBodyMaximo").style.maxHeight = tamanhoM + 'px';
        $("#panelBodyMaximo").css("padding-bottom", "0");
    }

    //Utilizado geralmente com uma divCenterGrande, divCenterMedio, divCenterPequeno
    if ($("#panelBodyFixo").length) {
        var tamanhoF = cwk_AlturaTela() - utilizadoGridGenerica(); // Retira tamanho da Barra de Icone, Barra de Navegação, do header e footer da página e Rodapé.
        if (parseInt(cwk_LarguraTela()) <=  764) {
            tamanhoF += 80; // Tira o desconto da barra de icone e margens
        }
        
        document.getElementById("panelBodyFixo").style.height = tamanhoF + 'px';
        $("#panelBodyFixo").css("padding-bottom", "0");
    }

    //Utilizado geralmente com uma divCenter
    if ($("#panelBodyFixodivCenter").length) {
        var tamanhoC = cwk_AlturaTela() - utilizadoGridGenerica();
        document.getElementById("panelBodyFixodivCenter").style.height = tamanhoC + 'px';
        $("#panelBodyFixodivCenter").css("padding-bottom", "0");
    }
}


//Retorna o ID que está na frente do campo, no lkp.
function cwk_GetIdLkp(lkp) {
    var retorno = '';
    if ($(lkp).val().indexOf('|') !== -1) {
        retorno = $(lkp).val().split("|")[0];
    } else {
        cwkErro('Preencha corretamente as informações.');
    }
    return retorno;
}

function cwk_GetIdLkpNaoObrigatorio(lkp) {
    var retorno = '';
    if ($(lkp).val().indexOf('|') !== -1) {
        retorno = $(lkp).val().split("|")[0];
    } else {
        retorno = '0';
    }
    return retorno;
}

function cwk_MontarDatePickerRange(nomeForm) {
    $('#form .input-daterange').datepicker({
        format: "dd/mm/yyyy",
        todayBtn: "linked",
        language: "pt-BR",
        orientation: "top auto",
        autoclose: true
    });
}

function cwkErroTit(informacao, erro) {
    informacao = '<span class="glyphicon glyphicon-remove-circle"></span> ' + informacao
    mensagem = '<div class="comment alert alert-danger">' + erro + '</div>';
    cwkMensagem(informacao, mensagem, BootstrapDialog.TYPE_DANGER);
}

function cwkErro(erro) {
    cwkErroTit("Erro!", erro);
}


function cwkSucessoTit(informacao, mensagem) {
    informacao = '<span class="glyphicon glyphicon-ok-circle"></span> ' + informacao
    mensagem = '<div class="comment alert alert-success">' + mensagem + '</div>';
    cwkMensagem(informacao, mensagem, BootstrapDialog.TYPE_SUCCESS);
}

function cwkSucesso(mensagem) {
    cwkSucessoTit('Sucesso', mensagem);
}

function cwkNotificacaoTit(informacao, mensagem) {
    informacao = '<span class="glyphicon glyphicon-info-sign"></span> ' + informacao
    mensagem = '<div class="comment alert alert-info">' + mensagem + '</div>';
    cwkMensagem(informacao, mensagem, BootstrapDialog.TYPE_INFO);
}

function cwkNotificacao(mensagem) {
    cwkNotificacaoTit("Informação", mensagem)
}

function cwkAlertaTit(informacao, mensagem) {
    informacao = '<span class="glyphicon glyphicon-warning-sign"></span> ' + informacao
    mensagem = '<div class="comment alert alert-warning">' + mensagem + '</div>';
    cwkMensagem(informacao, mensagem, BootstrapDialog.TYPE_WARNING);
}

function cwkAlerta(mensagem) {
    cwkNotificacaoTit("Informação", mensagem)
}

function cwkMensagem(titulo, mensagem, tipoDialogo) {
    titulo = '<h4>' + titulo + ' </h4>';
    //https://nakupanda.github.io/bootstrap3-dialog/
    BootstrapDialog.show({
        type: tipoDialogo,
        title: titulo,
        message: mensagem,
        onshown: function (dialogRef) {
            $(".comment").shorten({
                "showChars": 400
            });
        },
    });
}

function cwkConfirmacao(mensagem, titulo, fCallBackExcluiu, fCallBackCancelou) {
    if (!mensagem || mensagem === undefinedfined) {
        mensagem = "Ao confirmar a ação será permanente!";
    }

    if (!titulo || titulo === undefined) {
        titulo = "Deseja realmente excluir?";
    }
    bootbox.dialog({
        message: mensagem,
        title: titulo,
        buttons: {
            success: {
                label: "Excluir!",
                className: "btn-primary",
                callback: function () {
                    if (fCallBackExcluiu && fCallBackExcluiu !== undefined) {
                        fCallBackExcluiu();
                    }
                }
            },
            danger: {
                label: "Cancelar!",
                className: "btn-default",
                callback: function () {
                    if (fCallBackCancelou && fCallBackCancelou !== undefined) {
                        fCallBackCancelou();
                    }
                }
            }
        }
    });
}

(function ($) {
    $.fn.shorten = function (settings) {

        var config = {
            showChars: 100,
            ellipsesText: "...",
            moreText: "<i class='glyphicon glyphicon-plus'></i>",
            lessText: "<i class='glyphicon glyphicon-minus'></i>"
        };

        if (settings) {
            $.extend(config, settings);
        }

        $(document).off("click", '.morelink');

        $(document).on({
            click: function () {

                var $this = $(this);
                if ($this.hasClass('less')) {
                    $this.removeClass('less');
                    $this.html(config.moreText);
                } else {
                    $this.addClass('less');
                    $this.html(config.lessText);
                }
                $this.parent().prev().toggle();
                $this.prev().toggle();
                return false;
            }
        }, '.morelink');

        return this.each(function () {
            var $this = $(this);
            if ($this.hasClass("shortened")) return;

            $this.addClass("shortened");
            var content = $this.html();
            if (content.length > config.showChars) {
                var c = content.substr(0, config.showChars);
                var h = content.substr(config.showChars, content.length - config.showChars);
                var html = c + '<span class="moreellipses">' + config.ellipsesText + ' </span><span class="morecontent"><span>' + h + '</span> <a href="#" class="morelink">' + config.moreText + '</a></span>';
                //html = '<div class="alert alert-danger">' + html + '</div>';

                $this.html(html);
                $(".morecontent span").hide();
            }
        });
    };

})(jQuery);

///Método para esconder ou exibir o menu
function NavbarToggle(button) {
    if ($('.navbar').is(':visible')) {
        $('.divPage').css('top', '0');
    }
    else {
        $('.divPage').css('top', '120px');
    }
    $('.navbar').toggle();
    $(button).find('span').toggleClass('fa-window-maximize').toggleClass('fa-window-minimize');
}

function isEmpty(value) {
    return typeof value === 'string' && !value.trim() || typeof value === 'undefined' || value === null;
}

///Controla a comunicação dos assincronos com a aplicação, signalr
/// Controle mensageria
$(function () {
    var notificationHub = $.connection.notificationHub;

    notificationHub.client.taskComplete = function (job) {
        var layout =
            '                     <div class="row">                                                                                        ' +
            '                         <div class="col-md-8">                                                                               ' +
            '                               <h4>                                                                               ' +
            '                                   @item.IdTask - ' + job.DescricaoJob + ' &nbsp; &nbsp;                                              ' +
            '                               </h4>                                                                                                ' +
            '                         </div>                                                                                               '+
            '                     </div>                                                                                                   ' +
            '                 <p id="paramExib@item.IdTask">' + job.ParametrosExibicao + '</p>    ' +
            '                 <p id="msg@item.IdTask">' + job.Mensagem + '</p>    ' +
            ' <a href="' + job.UrlHost + job.UrlReferencia + '" > <span class="glyphicon glyphicon-eye-open"></span> detalhes </a>';
        if (isEmpty(job.FileDownload) === false) {
            if (job.FileDownload.indexOf(".html") > 0) {
                    layout = layout + 
                        ' <a href="#" id="HtmlView@item.IdTask" idtask="@item.IdTask" onclick = "GetHtmlViewTask(this);return false;"> <span class="glyphicon glyphicon-file"></span> Visualizar </a>';
            } else {
                layout = layout +
                    ' <a href="#" id="ArquivoDown@item.IdTask" idtask="@item.IdTask" onclick = "GetFileDownloadTask(this);return false;"> <span class="glyphicon glyphicon-floppy-save"></span> Retorno </a>';
            }
        }
        layout = layout.replace(new RegExp('@item.IdTask', 'g'), job.IdTask);
        var typeNotify = 'success';
        if (job.Progress === -9) {
            typeNotify = 'error';
        }
        var PNotifyComplete = new PNotify({
            text: layout,
            styling: "bootstrap3",
            type: typeNotify,
            delay: 5000,
            Buttons: {
                    closer: false,
                    sticker: false
            }
        });

        PNotifyComplete.get().click(function () {
            PNotifyComplete.remove();
        });

        //PNotifyComplete.get().css('cursor', 'pointer').click(function (e) {
        //    if ($(e.target).is('.ui-pnotify-closer *, .ui-pnotify-sticker *')) return;
        //    PNotifyComplete.remove();
        //});

        var divpr = job.UrlReferencia.replace(/\//g, "");
        if (divpr !== window.location.pathname.replace(/\//g, "")) {
            IncremetaContadorAvisoTask();
        }
        CriarOuReportarProgresso(divpr, job);
        //Cada tela deve implementar esse método, caso queira tomar alguma ação especifica na tela quando o job for concluído.
        if (typeof OnJobComplete === 'function') {
            OnJobComplete(job);
        }
    };

    notificationHub.client.taskProgress = function (job) {
        var divAbrirJobs = job.UrlReferencia.replace(/\//g, "");
        CriarOuReportarProgresso(divAbrirJobs, job, false);
    };

    $.connection.hub.start().done(function () {
        //console.log("hub conectado");
    });

    $.connection.hub.disconnected(function () {
        if ($.connection.hub.lastError) { console.log("Disconnected. Reason: " + $.connection.hub.lastError.message); }
        setTimeout(function () {
            $.connection.hub.start();
        }, 2000); // Restart connection after 5 seconds.
    });

    $.connection.hub.connectionSlow(function () {
        console.log("Conexao Lenta");
    });

    $.connection.hub.reconnecting(function () {
        console.log("Reconectando");
    });

    $.connection.hub.reconnected(function () {
        console.log("Reconectado");
    });

    $.connection.hub.disconnected(function () {
        console.log("Desconectado");
    });

    $("#btCalculo").click(function (e) {
        e.preventDefault();
        $.post("/Home/Calcular", { usuario: "userFixo" }, function (data) {
            var divAbrirJobs = window.location.pathname.replace(/\//g, "");
            CriarOuReportarProgresso(divAbrirJobs, data);
        }, 'json');
    });
});

function IncremetaContadorAvisoTask() {
    var processados = $("#badgeCalcProcessados").text();
    var qtdProcess = parseInt(processados);
    qtdProcess = qtdProcess + 1;
    $("#badgeCalcProcessados").text(qtdProcess);
}

function CriarOuReportarProgresso(divIncluir, pxyJobReturn, append) {
    CriarProgresso(divIncluir, pxyJobReturn, append);
    ReportarProgresso(pxyJobReturn);
}

function ReportarProgresso(pxyJobReturn, append) {
    if (pxyJobReturn !== undefined && $('#liTask' + pxyJobReturn.IdTask).length > 0) {
        $('#iconProgress' + pxyJobReturn.IdTask).removeClass("fa-check fa-spinner fa-times fa-exclamation fa-check fa-hourglass-start fa-trash-o");
        $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").removeClass("progress-bar-striped progress-bar-info progress-bar-success progress-bar-warning progress-bar-danger");
        $('#liTask' + pxyJobReturn.IdTask).removeClass("list-group-item-success list-group-item-info list-group-item-warning list-group-item-danger disabled");
        $('#msg' + pxyJobReturn.IdTask).text(pxyJobReturn.Mensagem);
        $('#ReprocessarTask' + pxyJobReturn.IdTask).hide();
        $('#CancelarTask' + pxyJobReturn.IdTask).hide();
        
        $('#progress' + pxyJobReturn.IdTask).show();

        if (isEmpty(pxyJobReturn.FileUpload) === false) {
            $('#ArquivoUp' + pxyJobReturn.IdTask).show();
        }
        else {
            $('#ArquivoUp' + pxyJobReturn.IdTask).hide();
        }

        if (isEmpty(pxyJobReturn.FileDownload) === false) {
            if (pxyJobReturn.FileDownload.indexOf(".html") > 0) {
                $('#ArquivoDown' + pxyJobReturn.IdTask).hide();
                $('#HtmlView' + pxyJobReturn.IdTask).show();
            }
            else {
                $('#ArquivoDown' + pxyJobReturn.IdTask).show();
                $('#HtmlView' + pxyJobReturn.IdTask).hide();
            }
            
        }
        else {
            $('#ArquivoDown' + pxyJobReturn.IdTask).hide();
            $('#HtmlView' + pxyJobReturn.IdTask).hide();
        }

        if (pxyJobReturn.Reprocessar === true) {
            $('#ReprocessarTask' + pxyJobReturn.IdTask).show();
        }
        
        if (pxyJobReturn.Progress === 100) ///Job Completo 
        {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-check");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-success");
            $('#progress' + pxyJobReturn.IdTask).hide();
            //$('#msg' + pxyJobReturn.IdTask).hide();
            $('#liTask' + pxyJobReturn.IdTask).addClass("list-group-item-success");
        }
        else if (pxyJobReturn.Progress === -9) /// Job com erro
        {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-times");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-danger");
            $('#progress' + pxyJobReturn.IdTask).hide();
            $('#liTask' + pxyJobReturn.IdTask).addClass("list-group-item-danger");
        }
        else if (pxyJobReturn.Progress === -8) /// Deletado/Cancelado
        {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-trash-o");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-danger");
            $('#progress' + pxyJobReturn.IdTask).hide();
            $('#liTask' + pxyJobReturn.IdTask).addClass("disabled");
        }
        else if (pxyJobReturn.Progress === -3) /// Job na fila para tentar processamento novamente
        {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-exclamation");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-warning  progress-bar-striped");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").css("width", "100%").text("");
            $('#liTask' + pxyJobReturn.IdTask).addClass("list-group-item-warning");
        }
        else if (pxyJobReturn.Progress === -2) /// Job Criado, aguardando a fila
        {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-hourglass-start");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-info progress-bar-striped");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").css("width", "100%").text("");
            $('#liTask' + pxyJobReturn.IdTask).addClass("disabled");
            $('#msg' + pxyJobReturn.IdTask).text(pxyJobReturn.Mensagem);
        }
        else if (pxyJobReturn.Progress >= 0) {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-spinner");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-info");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").css("width", pxyJobReturn.Progress + "%").text(pxyJobReturn.Progress + " %");
            $('#liTask' + pxyJobReturn.IdTask).addClass("list-group-item-info");
            if (pxyJobReturn.PermiteCancelar === true)
                $('#CancelarTask' + pxyJobReturn.IdTask).show();
        }
        else {
            $('#iconProgress' + pxyJobReturn.IdTask).addClass("fa-spinner");
            $('#liTask' + pxyJobReturn.IdTask).addClass("list-group-item-info");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").addClass("progress-bar-info progress-bar-striped");
            $('#progress' + pxyJobReturn.IdTask).find(".progress-bar").css("width", "100%").text("");
            if (pxyJobReturn.PermiteCancelar === true)
                $('#CancelarTask' + pxyJobReturn.IdTask).show();
        }
    }
}

function CriarProgresso(divIncluir, pxyJobReturn, append) {
    if (isEmpty(divIncluir)) {
        divIncluir = 'TasksProgress';
    }
    if (pxyJobReturn !== undefined && $('#liTask' + pxyJobReturn.IdTask).length === 0) {
        var divProgress =
            '             <li class="list-group-item list-group-item-info" id="liTask@item.IdTask">                                   ' +
            '                 <div class="list-group-item-heading">                                                                        ' +
            '                     <div class="row">                                                                                        ' +
            '                         <div class="col-md-8">                                                                               ' +
            '                               <h4 style="display:inline;">  <i id="iconProgress@item.IdTask" class="fa fa-spinner" aria-hidden="true"></i>                                                                                          ' +
            '                                   @item.IdTask - ' + pxyJobReturn.DescricaoJob + ' &nbsp; &nbsp;                                              ' +
            '                               </h4> ' +
            '                               <h5 style="display:inline; text-align:right;"> Criado em: ' + pxyJobReturn.InchoraStr + '</h5>' +
            '                         </div>                                                                                               ' +
            '                         <div class="col-md-4" style="text-align:right">                                                      ' +
            '                             <button type="button" class="btn btn-default" onclick = "GetFileUploadTask(this)" idtask="@item.IdTask" id="ArquivoUp@item.IdTask" style="display:none;"><span class="glyphicon glyphicon-send"></span> Enviado</button>            ' +
            '                             <button type="button" class="btn btn-default" onclick = "GetFileDownloadTask(this)" idtask="@item.IdTask" id="ArquivoDown@item.IdTask" style="display:none;"> <span class="glyphicon glyphicon-floppy-save"></span> Retorno</button>          ' +
            '                             <button type="button" class="btn btn-default" onclick = "GetHtmlViewTask(this)" idtask="@item.IdTask" id="HtmlView@item.IdTask" style="display:none;"><span class="glyphicon glyphicon-eye-open"></span> Visualizar</button>          ' +
            '                             <button type="button" class="btn btn-default" onclick = "ReprocessarTask(this)" idtask="@item.IdTask" id="ReprocessarTask@item.IdTask" style="display:none;"><span class="glyphicon glyphicon-refresh"></span> Reprocessar</button>          ' +
            '                             <button type="button" class="btn btn-danger" onclick = "CancelarTask(this)" idtask="@item.IdTask" id="CancelarTask@item.IdTask" style="display:none;"><span class="glyphicon glyphicon-stop"></span> Cancelar</button>          ' +
            '                         </div>                                                                                               ' +
            '                     </div>                                                                                                   ' +
            '                 </div>                                                                                                       ' +
            '                 <p class="list-group-item-text" id="paramExibicao@item.IdTask">' + pxyJobReturn.ParametrosExibicao + '</p>                          ' +
            '                 <p class="list-group-item-text" id="msg@item.IdTask">' + pxyJobReturn.Mensagem + '</p>                                    ' +
            '                 <div class="progress active" id="progress@item.IdTask" style="margin-bottom:0">                            ' +
            '                     <div class="progress-bar progress-bar-info progress-bar-striped" style="width: 100%;"></div>                          ' +
            '                 </div>                                                                                                       ' +
            '             </li>                                                                                                            ';
        divProgress = divProgress.replace(new RegExp('@item.IdTask', 'g'), pxyJobReturn.IdTask);
        var divIncProgress = "#" + divIncluir + ",#JobManagerIndex,#JobManager";
        if (append) {
            $(divIncProgress).append(divProgress);
        }
        else {
            $(divIncProgress).prepend(divProgress);
        }
    }
}

function ReprocessarTask(btn) {
    $.post("/JobManager/ReprocessarJob", { jobId: $(btn).attr("idtask") }, function (data) {
        var divAbrirJobs = window.location.pathname.replace(/\//g, "");
        CriarOuReportarProgresso(divAbrirJobs, data);
    }, 'json');
}

function CancelarTask(btn) {
    $.post("/JobManager/DeleteJob", { jobId: $(btn).attr("idtask") }, function (data) {
        var divAbrirJobs = window.location.pathname.replace(/\//g, "");
        CriarOuReportarProgresso(divAbrirJobs, data);
    }, 'json');
}

function GetFileUploadTask(btn) {
    window.location = '/JobManager/FileUpload?jobId=' + $(btn).attr("idtask");
}

function GetFileDownloadTask(btn) {
    window.location = '/JobManager/FileDownload?jobId=' + $(btn).attr("idtask");
}

function GetHtmlViewTask(btn) {
    var url = '/JobManager/FileDownload?jobId=' + $(btn).attr("idtask");
    $("#divLoadRelatorio").load(url);
    $("#divRelatorio").modal();
}

$(".btnReprocessarTask").click(function (e) {
    e.preventDefault();

});

function GetJobs(skip) {
    $("#loadJob").show();
    $("#recarregarJobs").attr("disabled", "disabled");
    $.post("/JobManager/LoadJobs", { skip: skip }, function (data) {
        if (data.length < 10) {
            $("#btnCarregarJobs").attr("disabled", "disabled");
            $("#btnCarregarJobs").text("Sem dados");
        }
        else {
            $("#btnCarregarJobs").removeAttr("disabled");
        }
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            var divAbrirJobs = window.location.pathname.replace(/\//g, "");
            CriarOuReportarProgresso(divAbrirJobs, item, true);
        }
    }, 'json').done(function () {
        $("#loadJob").hide();
        $("#recarregarJobs").removeAttr("disabled");
    });
}

function RemoveJobs() {
    var divAbrirJobs = window.location.pathname.replace(/\//g, "");
    $("#" + divAbrirJobs +" li").remove();
}
/// Fim Controle mensageria
