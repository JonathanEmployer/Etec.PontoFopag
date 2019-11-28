// AS CHAMADAS DE BOTÕES BÁSICOS ESTÃO AQUI. ESSA BIBLIOTECA IRÁ FAZER TODO O TRABALHO DE CHAMADAS DA GRID
// ADICIONAR ESSA BIBLIOTECA ENTRE AS ÚLTIMAS, NO BUNDLE.

/**
 *  Função que exibe uma caixa de confirmação para o usuário com opções "Sim"/"Não", um título e
 *  uma mensagem
 *
 *  @param {string} mensagem Mensagem que será exibida ao usuário
 *  @param {string} titulo Mensagem que será exibida no título da janela
 *  @param {string} msgBotaoYes Mensagem que será exibida no botão "Sim" da janela
 *  @param {string} msgBotaoNo Mensagem que será exibida no botão "Não" da janela
 *  @param {function} callbackSuccess Função que será chamada caso o botão "Sim" seja clicado
 *  @param {function} callbackCancel Função que será chamada caso o botão "Não" seja clicado
 *  @memberof cwkMetodosBotoesChamadas
 */
function messageBoxYesNoOption(mensagem, titulo, msgBotaoYes, msgBotaoNo, callbackSuccess, callbackCancel) {
    var box = bootbox.dialog({
        message: mensagem,
        title: titulo,
        buttons: {
            success: {
                label: msgBotaoYes,
                className: "btn-primary",
                callback: function () {
                    if (callbackSuccess !== undefined) {
                        callbackSuccess();
                    }
                }
            },
            danger: {
                label: msgBotaoNo,
                className: "btn-default",
                callback: function () {
                    if (callbackCancel !== undefined) {
                        callbackCancel();
                    }
                }
            }
        }
    });
    box.bind('shown.bs.modal'), function () {
        box.find("input").focus()
    }
}

/**
 *  Função que exibe uma caixa de confirmação para o usuário informando sobre a segurança do formato Excel de relatórios.
 *  Este método utiliza a função messageBoxYesNoOption como base.
 *
 *  @param {object} form Form onde exibir a caixa
 *  @returns {object} caixa de confirmação
 */
function mboxXls(form) {
    return messageBoxYesNoOption("O formato de exportação para Excel oferece menor segurança para os dados (os dados podem ser alterados). Deseja continuar?",
        "Alerta", "Sim", "Não",
        function () {
            form.submit();
        }, 
        function () {
        });
}

// Adiciona o evento click que irá chamar a tela de adicionar
// Esse método irá usar as funções dentro da biblioteca de métodos ajax. Certifique-se que ela foi declarada.
function cwk_EventoClickCadastroAdicionarAjax(botao, acao, controller) {
    $(botao).on("click", function () {
        ajax_CarregaTelaComLoading(acao, controller, 0, function () { $form.submit() });
    })
}

// Adiciona o evento click que irá chamar a tela de Editar do objeto
// Esse método irá usar as funções dentro da biblioteca de métodos ajax. Certifique-se que ela foi declarada.
function cwk_EventoClickCadastroAlterarAjax(botao, acao, controller, objetoTabela) {
    $(botao).on("click", function () {
        var id = cwk_GetIdSelecionado(objetoTabela);
        if (id > 0) {
            ajax_CarregaTelaComLoading(acao, controller, id);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de alterar um registro é necessário selecioná-lo!');
        }
    });
}

function cwk_EventoClickLogErroPNL(botao)
{
    $(botao).on("click", function () {
        var url = "/LogErroPainelAPI/Grid";
        windows.location = url;
    });
}

function mboxRegistraMarcacao(form) {
    return messageBoxYesNoOption("A marcação será registrada. Deseja continuar?",
        "Atenção", "Sim", "Não",
        function () {
            if ($("#idLat").val() === "" || $("#idLat").val() === "NaN")
            {
            return messageBoxYesNoOption("Não foi possível obter a sua localização, será considerado o horário do servidor. Deseja continuar?",
                "Atenção", "Sim", "Não",
                function () {
                    $("#idLat").val(0);
                    $("#idLon").val(0);
                    form.submit();
                },
                function () {
                });
            }
            else
            {
                form.submit();       
            }
        },
        function () {
        });
}

function cwk_ClickCadastroAdicionarAjaxParametro(botao, acao, controller, id, div) {
    $(botao).click(function (event) {
        event.preventDefault();
        Ajax_CarregaTelaParametro(acao, controller, 0, id, div);
    });
}

function cwk_ClickCadastroAlterarAjax(botao, acao, controller, objetoTabela, div) {
    $(botao).click(function (event) {
        var id = cwk_GetIdSelecionado(objetoTabela);
        if (id > 0) {
            Ajax_CarregaTelaParametro(acao, controller, id, 0, div);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de alterar um registro é necessário selecioná-lo!');
        }
    });
}

// Adiciona o evento click que irá chamar a tela de excluir do objeto
// Deverá ser passado por parâmetro a mensagem de exclusão.
// Esse método irá usar as funções dentro da biblioteca de métodos ajax. Certifique-se que ela foi declarada.
function cwk_CallControllerSemRetorno(botao, acao, controller, objetoTabela) {
    $(botao).on('click', function () {
        var id = GetIdSelecionadoTable(objetoTabela);
        if (id > 0) {
            ajax_CallControllerSemRetorno(acao, controller, id);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de executar a rotina é necessário selecionar um registro!');
        }
    });
}

// Método genérico para a realização de movimentações AJAX quaisquer.
// O Controller deve estar preparado pra receber como dado JSON um 'id'.
// O método recebe todas as informações como parâmetro, tais como:
// botao - botão da grid/form que foi clicado
// acao - Ação que será chamada
// controller - Controller que será chamado
// objetoTabela - Tabela onde o registro está localizado
// msgtituloform - Mensagem que irá aparecer no título do form de confirmação
// titulomsg - Título da mensagem que irá aparecer caso a movimentação ocorra OK
// mensagem - Texto da mensagem que irá aparecer caso a movimentação ocorra OK
// label - Palavra que irá aparecer no botão "Confirmar" do form de confirmação
// removedatabela - Flag para identificar se o registro deve ser removido da tabela (ex: movimentação - true) ou somente alterado (false)
function cwk_EventoClickCadastroMovAjax(botao, acao, controller, objetoTabela, msgtituloform, titulomsg, mensagem, label, removedatabela) {
    $(botao).on('click', function () {
        var id = GetIdSelecionadoTable(objetoTabela);
        if (id > 0) {
            ajax_ModificarRegistro(acao, controller, id, msgtituloform, titulomsg, mensagem, label, objetoTabela, removedatabela);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de excluir um registro é necessário selecioná-lo!');
        }
    });
}

// Adiciona o evento key press no tab, para quando o usuário digite um codigo ou nome
// Abre uma modal pra seleção.
function cwk_EventoKeyPressConsulta(campo, acao, controller, filtro, callback) {
    $(campo).on('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        // Código do tab
        if (keyCode === 9 && !e.shiftKey) {
            var valorCampo = $(campo).val();
            var valorFiltro = $(filtro).val();
            //Abre uma grid com um getall
            ajax_CarregarConsultaEventoTab(acao, controller, valorCampo, campo, valorFiltro, callback);
        }
    });
}

function cwk_EventoBotaoConsulta(botao, campo, acao, controller, filtro, callback) {
    $(botao).on('click', function () {
        var valorFiltro = $(filtro).val();
        ajax_CarregarConsultaEventoTab(acao, controller, '{#botaopresionado#}', campo, valorFiltro, callback);
    });
}

function cwk_EventoConsulta(botao, campo, acao, controller, filtro, callback) {
    cwk_EventoKeyPressConsulta(campo, acao, controller, filtro, callback);
    cwk_EventoBotaoConsulta(botao, campo, acao, controller, filtro, callback);
}

function cwk_DataCalendario(botao, campo) {
    $(botao).click(function () {
        $(campo).focus();
    });
}

//Function para atualizar data com tecla space bar
function dataKeypress(idData) {
    if (event.keyCode === 32) { $(idData).val((new Date()).toLocaleDateString("pt-BR")).keyup(); }
}

function cwk_EventoKeyPressEsc(document, acao, controller) {
    $(document).on('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        // Código do Esc
        if (keyCode === 27) {
            var url = "/" + controller + "/" + acao + "/" + 0;
            window.location = url;
        }
    });
}

// Adiciona o evento key press no tab, para quando o usuário digite um codigo ou nome
// Abre uma modal pra seleção.
// Filtro para funcionário - Departamento e Departamento - Empresa
// Carrega detalhes serve pra carregar uma nova div com informações a mais. Exemplo a tela de marcação
// Quando seleciona um funcionário, as informações são carregadas na div ao lado.
function cwk_EventoKeyPressConsultaComFiltro(campo, CampoFiltro, acao, controller, carregaDetalhes) {
    $(campo).on('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        // Código do tab
        if (keyCode === 9) {
            var idFiltro = cwk_GetIdLkpNaoObrigatorio(CampoFiltro);
            var valorCampo = $(campo).val();
            //Abre uma grid com um getall
            if (idFiltro !== 0) {
                ajax_CarregarConsultaEventoTabComFiltro(acao, idFiltro, controller, valorCampo, campo, carregaDetalhes);
            } else {
                ajax_CarregarConsultaEventoTab(acao, controller, valorCampo, campo);
            }
        }
    });
}

function cwk_EventoKeyPressConsultaComFiltroMarcacao(campo, CampoFiltro, acao, controller, carregaDetalhes) {
    $(campo).on('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        // Código do tab
        if (keyCode === 9) {
            var idFiltro = cwk_GetIdLkpNaoObrigatorio(CampoFiltro);
            var valorCampo = $(campo).val();
            //Abre uma grid com um getall
            if (idFiltro !== 0) {
                ajax_CarregarConsultaEventoTabComFiltroMarcacao(acao, idFiltro, controller, valorCampo, campo, carregaDetalhes);
            }
        }
    });
}

function cwk_DbClickCadastroEditarAjaxParam(acao, controller, nomeTabela, parametros, divAbrir) {
    $(nomeTabela + ' tbody').on('dblclick', 'tr', function () {
        if (!($(this).hasClass('selected'))) {
            $(this).addClass('selected');
        }
        var id = GetIdSelecionadoTable(nomeTabela);
        if (id > 0) {
            cwk_CadastroAjaxParam(acao, controller, id, parametros, divAbrir);
        } else {
            cwkErroTit('Selecione um Registro!', 'É necessário selecionar um registro para realizar essa operação!');
        }
    });
}

//Abre um cadastro de inclusão com ajax (Na mesma tela, ver exemplo no cadastro de histórico de cliente)
function cwk_EventoIncluirAjaxParam(botao, acao, controller, parametros, divAbrir) {
    $(botao).on("click", function () {        
        cwk_CadastroAjaxParam(acao, controller, 0, parametros, divAbrir);
    });
}

function ButtonClickSendProcessJob(botao, acao, controller, type, nomeTabela, parametros) {
    $(botao).on("click", function () {
        url = "/" + controller + "/" + acao;
        var id = GetIdSelecionadoTable(nomeTabela);
        if (id > 0) {
            if (parametros === undefined || parametros === null || parametros === "") {
                parametros = { 'id': id };
            }
            else if (id !== undefined && id !== null && id !== "" && id !== 0) {
                parametros.id = id;
            }
            PostReturnJob(url, type, parametros);
        }
        else { cwkErroTit('Selecione um Registro!', 'É necessário selecionar um registo para realizar essa operação!'); }
    });
}

//Abre um cadastro de alteração com ajax (Na mesma tela, ver exemplo no cadastro de histórico de cliente)
//Necessita de uma grid como parâmetro para poder pegar o id do registro que vai ser alterado/Aberto
function cwk_EventoAlterarAjaxParam(botao, acao, controller, nomeTabela, parametros, divAbrir) {
    $(botao).on("click", function () {        
        var id = GetIdSelecionadoTable(nomeTabela);   
        if (id > 0) {
            cwk_CadastroAjaxParam(acao, controller, id, parametros, divAbrir);
        }
        else { cwkErroTit('Selecione um Registro!', 'É necessário selecionar um registo para realizar essa operação!'); }

    });
}

function cwk_EventoAlterarAjaxParamOld(botao, acao, controller, nomeTabela, parametros, divAbrir) {
    $(botao).on("click", function () {
        var id = cwk_GetIdSelecionado(nomeTabela);
        if (id > 0) {
            cwk_CadastroAjaxParam(acao, controller, id, parametros, divAbrir);
        }
        else { cwkErroTit('Selecione um Registro!', 'É necessário selecionar um registro para realizar essa operação!'); }

    });

}

function cwk_EventoAbrirCadastroAjaxParam(botao, acao, controller, parametros, divAbrir) {
    $(botao).on("click", function () {
        cwk_CadastroAjax(acao, controller, parametros, divAbrir);
    });
}

//Utilizado pelos métodos cwk_EventoIncluirAjaxParam e cwk_EventoAlterarAjaxParam
function cwk_CadastroAjaxParam(acao, controller, id, parametros, divAbrir, fCallBack) {
    //Se não foi passado parametro, crio o parâmetro apenas com o id  
    if (parametros === undefined || parametros === null || parametros === "") {
        parametros = { 'id': id };
    }        
    else if (id !== undefined && id !== null && id !== "" && id !== 0) {
        parametros.id = id;
    }
    cwk_CadastroAjax(acao, controller, parametros, divAbrir, fCallBack);
}

function cwk_CadastroAjax(acao, controller, parametros, divAbrir, fCallBack) {
    if (divAbrir === undefined || divAbrir === null || divAbrir === "") {
        divAbrir = '#divLoadCadastroModal';
    }
    url = "/" + controller + "/" + acao;
    $.ajax({
        cache: false, 
        type: "GET",
        url: url,
        data: parametros,
        beforeSend: function () {
            $(divAbrir).modal('hide');
            $("#loading").modal();
        },
        complete: function (e, xhr, settings) {
            //console.log(this.url);
            switch (e.status) {
                case 500:
                    cwkErro('Erro interno!');
                    break;
                case 404:
                    cwkErro('Página solicitada não foi encontrada!');
                    break;
                case 401:
                    cwkErro('Acesso negado!');
                    break;
            }
        }
    }).done(function (data) {
        $(divAbrir).modal('hide');
        $("#loading").modal('hide');
        if (data.erro) {
            cwkErro(data.mensagemErro);
            $(divAbrir).html("");
        } else {
            if (data.indexOf("Usuario/LogIn") > -1) {
                window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
            }
            else {
                $(divAbrir).html(data);
                $.validator.unobtrusive.parse(divAbrir);
                $(divAbrir).modal();
            }
        }
        setaFocoPrimeiroCampoDiv(divAbrir);

        if (fCallBack && typeof (fCallBack) !== "undefined" && fCallBack !== "") {
            fCallBack();
        }
    });
}

function cwk_CarregarPartialAjax(acao, controller, parametros, divPartial, fCallBack, setarFocoPrimeiroCampoDiv) {
    if (divPartial === undefined || divPartial === null || divPartial === "") {
        divPartial = '#divPartial';
    }
    url = "/" + controller + "/" + acao;
    $.ajax({
        cache: false, 
        type: "GET",
        url: url,
        data: parametros,
        beforeSend: function () {
            $("#loading").modal();
            $(divPartial).html("");
        },
        complete: function (e, xhr, settings) {
            switch (e.status) {
                case 500:
                    cwkErro('Erro interno!');
                    break;
                case 404:
                    cwkErro('Página solicitada não foi encontrada!');
                    break;
                case 401:
                    cwkErro('Acesso negado!');
                    break;
            }
        }
    }).done(function (data) {
        $("#loading").modal('hide');
        if (data.erro) {
            cwkErro(data.mensagemErro);
            $(divPartial).html("");
        } else {
            if (data.indexOf("Usuario/LogIn") > -1) {
                window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
            }
            else {
                $(divPartial).html(data);
            }
        }
        
        if (setarFocoPrimeiroCampoDiv && typeof (setarFocoPrimeiroCampoDiv) !== "undefined" && setarFocoPrimeiroCampoDiv !== "" && setarFocoPrimeiroCampoDiv === true) {
            setaFocoPrimeiroCampoDiv(divPartial);
        }

        if (fCallBack && typeof (fCallBack) !== "undefined" && fCallBack !== "") {
            fCallBack();
        }
    });
}

function cwk_CarregarPartialAjaxPorPost(acao, controller, parametros, divPartial, fCallBack, setarFocoPrimeiroCampoDiv) {
    if (divPartial === undefined || divPartial === null || divPartial === "") {
        divPartial = '#divPartial';
    }
    url = "/" + controller + "/" + acao;
    $.ajax({
        cache: false,
        type: "POST",
        url: url,
        data: parametros,
        beforeSend: function () {
            $("#loading").modal();
            $(divPartial).html("");
        },
        complete: function (e, xhr, settings) {
            switch (e.status) {
                case 500:
                    cwkErro('Erro interno!');
                    break;
                case 404:
                    cwkErro('Página solicitada não foi encontrada!');
                    break;
                case 401:
                    cwkErro('Acesso negado!');
                    break;
            }
        }
    }).done(function (data) {
        $("#loading").modal('hide');
        if (data.erro) {
            cwkErro(data.mensagemErro);
            $(divPartial).html("");
        } else {
            if (data.indexOf("Usuario/LogIn") > -1) {
                window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
            }
            else {
                $(divPartial).html(data);
            }
        }

        if (setarFocoPrimeiroCampoDiv && typeof (setarFocoPrimeiroCampoDiv) !== "undefined" && setarFocoPrimeiroCampoDiv !== "" && setarFocoPrimeiroCampoDiv === true) {
            setaFocoPrimeiroCampoDiv(divPartial);
        }

        if (fCallBack && typeof (fCallBack) !== "undefined" && fCallBack !== "") {
            fCallBack();
        }
    });
}

function cwk_PostAjax(acao, controller, parametros, divAbrir, fCallBack, fCallBackErro) {
    url = "/" + controller + "/" + acao;
    $.ajax({
        type: "POST",
        url: url,
        data: parametros,
        success: function (data) {
            if (data.JobId !== "" && data !== null && data.JobId !== undefined) {
                trackJobProgress(data, fCallBack, fCallBackErro, true);
            }
            else {
                ProgressLimpo();
                $("#divModalProgress").modal('hide');
                cwkErro(data.Erro);
            }
        },
        error: function (ex) {
            if (ex.status !== 200) {
                $("#divModalProgress").modal('hide')
            }
        }
    }).done(function (data) {
        $("#loading").modal('hide');
        corrigeDatePickerAjax();
        if (fCallBack !== "" && fCallBack !== null && fCallBack !== undefined) {
            fCallBack();
        }
    });
}

function setaFocoPrimeiroCampoDiv(div)
{
    //var camposDiv = $(div).find("input:visible:enabled:not([readonly]),textarea:visible:enabled:not([readonly]),select:visible:enabled:not([readonly])");
    //primeiroCampo = camposDiv.first();
    //if (primeiroCampo.hasClass('close')) {
    //    var nextIndex = camposDiv.index(primeiroCampo) + 1;
    //    var f = camposDiv[nextIndex].focus();
    //}
    //else {
    //    var f = camposDiv.first().focus();
    //}
    $('.datepickerpt').datepicker('show').datepicker('hide');
    var camposDiv = $(div).find("*").filter(function () {
        if (/^select|textarea|button|input$/i.test(this.tagName)) { //not-null
            //Optionally, filter the same elements as above
            if (/^input$/i.test(this.tagName) && !/^checkbox|radio|text$/i.test(this.type)) {
                // Not the right input element
                return false;
            }
            return !this.readOnly &&
                   !this.disabled &&
                   $(this).parentsUntil('form', 'div').css('display') !== "none";
        }
        return false;
    });
    var primeiroCampo = camposDiv.first();
    var f;
    if (primeiroCampo.hasClass('close')) {
        var nextIndex = camposDiv.index(primeiroCampo) + 1;
        f = $(camposDiv[nextIndex]).focus();
    }
    else {
        f = camposDiv.first().focus();
    }
}

function corrigeDatePickerAjax() {
    var options = new Array();
    options['language'] = 'pt-BR';
    options['format'] = "dd/mm/yyyy";
    options['todayBtn'] = "linked";
    options['autoclose'] = "true";
    options['todayHighlight'] = "true";
    $('.datepickerpt').datepicker(options);
    $('.datepickerpt').datepicker('show').datepicker('hide');
}

function cwk_SalvaCadastroAjax(Objeto, divAbrir, fCallBack) {
    if (divAbrir === undefined || divAbrir === null || divAbrir === "") {
        divAbrir = "#divLoadCadastroModal";
    }
    $.ajax({
        url: Objeto.action,
        type: Objeto.method,
        data: $(Objeto).serialize(),
        beforeSend: function () {
            $(divAbrir).modal('hide');
            $("#loading").modal();
        },
        error: function (ex) {
            cwkErroTit("Erro", ex.status + ': ' + ex.statusText + ' | ' + ex.Erro);
        },
        complete: function (e) {
            //$("#loading").modal('hide');
            $.unblockUI();
            switch (e.status) {
                case 500:
                    cwkErro('Erro interno!');
                    break;
                case 404:
                    cwkErro('Página solicitada não foi encontrada!');
                    break;
                case 401:
                    cwkErro('Acesso negado!');
                    break;
            }
        }
    }).done(function (data) {
        $(divAbrir).modal('hide');
        $("#loading").modal('hide');
        $(divAbrir).html(data);
        $(divAbrir).modal();
        corrigeDatePickerAjax();
        var primeiroCampoDiv = $(divAbrir).find('input:visible:first');
        primeiroCampoDiv.focus();
        if (fCallBack && typeof (fCallBack) !== "undefined" && fCallBack !== "") {
            fCallBack();
        }
    });
}

function cwk_AbrirRelatorioAjax(Objeto, callBack, callBackErro) {
    $.ajax({
        url: Objeto.action,
        type: Objeto.method,
        data: $(Objeto).serialize(),
        success: function (data) {
            if (data.JobId !== "" && data !== null && data.JobId !== undefined) {
                trackJobProgress(data, callBack, callBackErro, true);
            }
            else {
                ProgressLimpo();
                $("#divModalProgress").modal('hide');
                cwkErro(data.Erro);
            }
        },        
        error: function (xhr, textStatus, errorThrown) {
            var sErrMsg = "";
            sErrMsg += "Erro ao Salvar: ";
            sErrMsg += "\n\n" + " - textStatus :" + textStatus;
            sErrMsg += "\n\n" + " - Error Status :" + xhr.status;
            sErrMsg += "\n\n" + " - Error type :" + errorThrown;
            sErrMsg += "\n\n" + " - Error message :" + xhr.responseText;
            cwkErro(sErrMsg);
            $("#divModalProgress").modal('hide')
        }
    }).done(function (data) {
        $("#loading").modal('hide');
        corrigeDatePickerAjax();
        if (callBack !== "" && callBack !== null && callBack !== undefined) {
            callBack();
        }
    });
}

// Objeto = Dados para o post
// divAbrir = Div do "cadastro", caso passado vazio o método vai assumir que é a div divLoadCadastroModal
// callBack = Método com o que deseja executar após executar o post
// exibeLoading = Indica se deseja exibir o "gif" de loading
function cwk_SalvaCadastroAjaxProgress(Objeto, divAbrir, callBack, exibeLoading) {
    if (divAbrir === undefined || divAbrir === null || divAbrir === "") {
        divAbrir = "#divLoadCadastroModal";
    }
    if (exibeLoading === undefined || exibeLoading === null || exibeLoading === "") {
        exibeLoading = false;
    }

    $.ajax({
        url: Objeto.action,
        type: Objeto.method,
        data: $(Objeto).serialize(),
        beforeSend: function () {  
            $(".salvando").addClass("disabled");
            if (exibeLoading) {
                $("#loading").modal();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            var sErrMsg = "";
            sErrMsg += "Erro ao Salvar: ";
            sErrMsg += "\n\n" + " - textStatus :" + textStatus;
            sErrMsg += "\n\n" + " - Error Status :" + xhr.status;
            sErrMsg += "\n\n" + " - Error type :" + errorThrown;
            sErrMsg += "\n\n" + " - Error message :" + xhr.responseText;
            cwkErro(sErrMsg);
            //cwkErro("Erro!!\n" + ex.status + ': ' + ex.statusText + ' | ' + ex.Erro);
        },
        complete: function (e) {
            //$("#loading").modal('hide');
            $.unblockUI();
            if (e.status !== 200) {
                switch (e.status) {
                    case 500:
                        cwkErro('Erro interno!');
                        break;
                    case 404:
                        cwkErro('Página solicitada não foi encontrada!');
                        break;
                    case 401:
                        cwkErro('Acesso negado!');
                        break;
                    default:
                        cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText + eval(error));
                        break;
                }
            }
            else if (e.responseText.indexOf('<title>Acesso Negado</title>') > 0) {
                cwkErroTit('Acesso negado', 'As alterações não foram realizadas');
            }
        }
    }).done(function (data) {
        $(".salvando").removeClass("disabled");
        if (exibeLoading) {
            $("#loading").modal('hide');
        }
        if ((data !== null && data !== undefined) && (data.Erro === null || data.Erro === undefined ||data.Erro === ""))
        {
            if (data.JobId ===  null || data.JobId === undefined || data.JobId === "" || data.JobId === 0 ) {
             callBack();
            }
            else {
                trackJobProgress(data, callBack);
            }
            $(divAbrir).modal('hide');
            
        }
        else {
            ProgressLimpo();
            $("#divModalProgress").modal('hide');
            cwkErro(data.Erro);
        }
    });
}