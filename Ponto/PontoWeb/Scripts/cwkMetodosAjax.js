
// ADICIONAR ESSA BIBLIOTECA POR ÚLTIMO, NO BUNDLE.
// AUTOR: MAYKON RISSI

// Carrega uma nova tela na divLoadCadastros, mostrando o GIF de Loading. Deve ser usada para Create ou Update. 
function ajax_CarregaTelaComLoading(acao, controller, id) {
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        crossDomain: true,
        data: { 'id': id },
        beforeSend: function () {
            $("#loading").modal();
        },
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
            $('#divLoadCadastros').html(ret);
            $('form').removeData("validator");

            //Refresh the validators
            $.validator.unobtrusive.parse(document);
        },
        complete: function () {
            $("#loading").modal('hide');
        }
    });
}

// Carrega uma grid na div de cadastros. Usar nos métodos complete de ajax.
function ajax_CarregaGrid(controller, acao) {
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        crossDomain: true,
        beforeSend: function () {
            $("#loading").modal();
        },
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
            $('#divLoadCadastros').html(ret);
        },
        complete: function () {
            $("#loading").modal('hide')
        }
    });
}

// Carrega uma grid na div de cadastros. Usar nos métodos complete de ajax.
function ajax_CarregaGridAlertas(controller, acao, tipo) {
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        data: { 'tipo': tipo },
        crossDomain: true,
        beforeSend: function () {
            $("#loading").modal();
        },
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
            $('#divLoadCadastros').html(ret);
        },
        complete: function () {
            $("#loading").modal('hide')
        }
    });
}

// Mesma função do CarregaTelaComLoading, mas sem o GIF
function ajax_CarregaTela(acao, controller, id) {
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        data: { 'id': id },
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
        }
    });
}

function ajax_CallControllerSemRetorno(acao, controller, id)
{
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: 'json',
        data: { 'id': id },
        beforeSend: function () {
            $.blockUI({ message: '<h2>Aguarde<img src="../../Content/img/circulosLoading.GIF"></h2>' });
        },
        error: function (xhr, textStatus, errorThrown) {
            $.unblockUI();
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                $.unblockUI();
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
            }
        },
        success: function (ret) {
            $.unblockUI();
            if (ret.Success === true) {
                verificaProgress();
            } else {
                if (ret.Aviso != undefined && ret.Aviso != "" && ret.Aviso == true) {
                    cwkNotificacaoTit('Alerta!', ret.Erro);
                }
                else {
                    cwkErroTit('Erro!', ret.Erro);
                }
            }
        },
    });
}

// Remove um registro. Ele não adiciona o evento click, mas já adiciona as mensagens de validação.
// O Controller deve estar preparado pra receber como dado JSON um 'id'.
// O método já remove o objeto da grid.
function ajax_ExcluirRegistro(acao, controller, id, mensagem, tb, callBackSucesso, mensagemConfirmacaoPersonalizada) {
    var mensagemConfirmacao = "Ao confirmar a ação será permanente!";
    if (mensagemConfirmacaoPersonalizada && typeof (mensagemConfirmacaoPersonalizada) !== "undefined" && mensagemConfirmacaoPersonalizada !== "") {
        mensagemConfirmacao = mensagemConfirmacaoPersonalizada;
    }
    bootbox.dialog({
        message: mensagemConfirmacao,
        title: "Deseja realmente excluir?",
        buttons: {
            success: {
                label: "Excluir!",
                className: "btn-primary",
                callback: function () {
                    $.ajax({
                        url: '/' + controller + '/' + acao,
                        type: 'POST',
                        dataType: 'json',
                        data: { 'id': id },
                        beforeSend: function () {
                            $.blockUI({ message: '<h2>Excluindo<img src="../../Content/img/circulosLoading.GIF"></h2>' });
                        },
                        error: function (xhr, textStatus, errorThrown) {
                            $.unblockUI();
                            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                                cwkErro("Acesso negado, Contate o administrador do sistema!");
                            }
                            else {
                                $.unblockUI();
                                var sErrMsg = "";
                                sErrMsg += "Erro: ";
                                sErrMsg += "\n\n" + " - Status :" + textStatus;
                                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                                cwkErro(sErrMsg);
                            }
                        },
                        success: function (ret) {
                            $.unblockUI();
                            if (ret.Success === true) {
                                cwkSucessoTit('Registro Excluído!', mensagem);
                                cwk_RemoverLinhasSelecionadas(tb);
                                if (callBackSucesso && typeof (callBackSucesso) !== "undefined" && callBackSucesso !== "") {
                                    callBackSucesso(ret);
                                }
                                verificaProgress();
                            } else {
                                if (ret.Aviso != undefined && ret.Aviso != "" && ret.Aviso == true) {
                                    cwkNotificacaoTit('Alerta!', ret.Erro);
                                }
                                else {
                                    cwkErroTit('Erro!', ret.Erro);
                                }
                            }
                        },
                    });
                }
            },
            danger: {
                label: "Cancelar!",
                className: "btn-default",
                callback: function () {

                }
            }
        }
    });
}

function BloqueiaSalvando()
{
    $.blockUI({ message: '<h2>Salvando...<img src="../../Content/img/circulosLoading.GIF"></h1>' });
}

function ajax_ValidarRegistro(acao, controller, consulta, callback) {
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: 'json',
        data: { 'consulta': consulta },
        error: function (ex) {
            cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
        },
        success: function (ret) {
            callback(ret);
        },
    });
}

// Método genérico para a realização de movimentações AJAX quaisquer.
// O Controller deve estar preparado pra receber como dado JSON um 'id'.
// O método recebe todas as informações como parâmetro, tais como:
// acao - Ação que será chamada
// controller - Controller que será chamado
// id - Id do objeto à ser modificado
// msgTituloFormConfirmar - Mensagem que irá aparecer no título do form de confirmação
// tituloMsgOk - Título da mensagem que irá aparecer caso a movimentação ocorra OK
// mensagemOk - Texto da mensagem que irá aparecer caso a movimentação ocorra OK
// labelBtnConfirmar - Palavra que irá aparecer no botão "Confirmar" do form de confirmação
// tb - Tabela onde o registro está localizado
// removedatabela - Flag para identificar se o registro deve ser removido da tabela (ex: movimentação - true) ou somente alterado (false)
function ajax_ModificarRegistro(acao, controller, id, msgTituloFormConfirmar, tituloMsgOk, mensagemOk, labelBtnConfirmar, tb, removedatabela) {
    bootbox.dialog({
        message: "Ao confirmar a ação será permanente!",
        title: msgTituloFormConfirmar,
        buttons: {
            success: {
                label: labelBtnConfirmar,
                className: "btn-primary",
                callback: function () {
                    $.ajax({
                        url: '/' + controller + '/' + acao,
                        type: 'POST',
                        dataType: 'json',
                        data: { 'id': id },
                        error: function (ex) {
                            cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
                        },
                        success: function (ret) {
                            if (ret.success === true) {
                                cwkSucessoTit(tituloMsgOk, mensagemOk);
                                if (removedatabela) {
                                    var objetoTabela = $(tb).closest('table').DataTable();
                                    cwk_RemoverLinhasSelecionadas(objetoTabela);
                                }
                                var divAbrirJobs = window.location.pathname.replace(/\//g, "");
                                CriarOuReportarProgresso(divAbrirJobs, ret.job);
                            } else {
                                cwkErroTit('Erro!', ret.Erro);
                            }
                        },
                    });
                }
            },
            danger: {
                label: "Cancelar!",
                className: "btn-default",
                callback: function () {

                }
            }
        }
    });
}

// Exibe a div de loading que estava oculta. Usado ao chamar ajax com o MVC
function onAjaxBegin() {
    document.getElementById("loading").style.display = "block";
}

// Oculta a div de loading que estava sendo exibida. Usado ao chamar ajax com o MVC
function onAjaxComplete() {
    document.getElementById("loading").style.display = "none";
}

//Metodo privado do cwkMetodosAjax. Não use (:
function ajax_CarregarDetalhes(acao, Controller, id) {
    $('#divInf').load('/' + Controller + '/' + acao + '/' + id);
}


/// Método que consome um método get com retorno em json e executa uma função passada por callback utilizando os dados retornados
/// Exemplo TabelaMarcacao.cshtml
function ajax_GetObjetoExecCallBack(controller, acao, parms, eventoCallBack, bloquearTela) {
    if (bloquearTela == "" || bloquearTela == undefined) {
        bloquearTela = false;
    }

    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: 'json',
        data: parms,
        beforeSend: function () {
            if (bloquearTela) {
                $.blockUI({ message: '<h2>Excluindo<img src="../../Content/img/circulosLoading.GIF"></h2>' });
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (bloquearTela) {
                $.unblockUI();
            }
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
            }
        },
        success: function (ret) {
            if (bloquearTela) {
                $.unblockUI();
            }
            if (ret.Sucesso === true) {
                if (eventoCallBack && typeof (eventoCallBack) !== "undefined" && eventoCallBack !== "") {
                    eventoCallBack(ret);
                }
            } else {
                if (ret.Aviso != undefined && ret.Aviso != "" && ret.Aviso == true) {
                    cwkNotificacaoTit('Alerta!', ret.Erro);
                }
                else {
                    if (ret.Erro != undefined && ret.Erro != "") {
                        cwkErroTit('Erro!', ret.Erro);
                    }
                }
            }
        },
    });
}

function ajax_CarregarConsultaEventoTab(acao, controller, consulta, campo, filtro, eventoCallBack, posicaoNome, paramAdicionais, eventoCallBackErro) {
    if ($(campo).is('[readonly]') ) { 
        return;
    }

    var parametros = {};
    var botao = false;
    if (consulta === "{#botaopresionado#}") {
        consulta = "";
        botao = true;
    }

    if (posicaoNome === undefined || posicaoNome === null || posicaoNome === "") {
        posicaoNome = "1";
    }

    if (typeof (filtro) === "undefined") {
        filtro = "T";
    }

    if ((typeof (paramAdicionais) !== "undefined") && paramAdicionais !== null && paramAdicionais !== "") {
        parametros = paramAdicionais;
    }

    parametros.consulta = consulta;

    if (filtro !== "") {
        parametros.filtro = filtro
    }
    if (consulta.indexOf('|') == -1) {
        $.blockUI({ message: '<h2>Pesquisando<img src="../../Content/img/circulosLoading.GIF"></h1>' });
        $.ajax({
            url: '/' + controller + '/' + acao,
            type: 'GET',
            dataType: "html",
            data: parametros,
            crossDomain: true,
            cache: false,
            error: function (ex) {
                if (eventoCallBackErro && typeof (eventoCallBackErro) !== "undefined" && eventoCallBackErro !== "") {
                    eventoCallBackErro(campo);
                }
                if (ex.status !== 200) {
                    cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText, campo);
                    $.unblockUI();
                }
            },
            success: function (ret) {
                //console.log("url=" + this.url);
                if (ret.indexOf("Usuario/LogIn") > -1) {
                    window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
                }
                else {
                    $('#divLoadModalLkp').html(ret);
                    var tbPesquisa = $("#divLoadModalLkp table").eq(0).DataTable();

                    if (tbPesquisa.rows().count() === 1 && botao === false) {
                        var dados = tbPesquisa.rows(0).data()[0];
                        var id = dados[0].replace('undefined', '');
                        var nome = dados[1].replace('undefined', '');
                        $(campo).val(id + ' | ' + nome);
                        $.unblockUI();
                        setaFocoProximo(campo);
                        if (eventoCallBack && typeof (eventoCallBack) !== "undefined" && eventoCallBack !== "") {
                            eventoCallBack(campo);
                        }
                    } else if (tbPesquisa.rows().count() === 0) {
                        $(".datepickerpt").datepicker('hide');
                        if (eventoCallBackErro && typeof (eventoCallBackErro) !== "undefined" && eventoCallBackErro !== "") {
                            eventoCallBackErro(campo);
                        }
                        var campoR = campo.replace('#', '');
                        document.getElementById(campoR).focus();
                        cwkErroTit('Erro', 'Registro não encontrado.');
                    } else {
                        $('#divLoadModalLkp').appendTo('Body').modal();
                        $(".datepickerpt").datepicker('hide');
                        $('#btSelec').focus();
                        $('#btSelec').on('click', function () {
                            var ids = cwk_GetIdSelecionado(tbPesquisa);
                            if (ids >= 0) {
                                var dados = tbPesquisa.rows('.selected').data()[0];
                                var id = dados[0].replace('undefined', '');
                                var nome = dados[1].replace('undefined', '');
                                $(campo).val(id + ' | ' + nome);
                                $("#divLoadModalLkp").modal('hide');
                                $.unblockUI();
                                setaFocoProximo(campo);
                                if (eventoCallBack && typeof (eventoCallBack) !== "undefined" && eventoCallBack !== "") {
                                    eventoCallBack(campo);
                                }
                            } else {
                                if (eventoCallBackErro && typeof (eventoCallBackErro) !== "undefined" && eventoCallBackErro !== "") {
                                    eventoCallBackErro(campo);
                                }
                                cwkErroTit('Erro', 'Selecione um registro.');
                            }
                        });
                        $('#tbSel tbody tr').dblclick(function () {
                            $(this).addClass('selected');
                            var ids = cwk_GetIdSelecionado(tbPesquisa);
                            if (ids >= 0) {
                                var dados = tbPesquisa.rows('.selected').data()[0];
                                var id = dados[0].replace('undefined', '');
                                var nome = dados[1].replace('undefined', '');
                                $(campo).val(id + ' | ' + nome);
                                $("#divLoadModalLkp").modal('hide');
                                $.unblockUI();
                                setaFocoProximo(campo);
                                if (eventoCallBack && typeof (eventoCallBack) !== "undefined" && eventoCallBack !== "") {
                                    eventoCallBack(campo);
                                }
                            } else {
                                if (eventoCallBackErro && typeof (eventoCallBackErro) !== "undefined" && eventoCallBackErro !== "") {
                                    eventoCallBackErro(campo);
                                }
                                cwkErroTit('Erro', 'Selecione um registro.');
                            }
                        });
                    }
                    $.unblockUI();
                }
            }
        });
    }
}

// Evendo para pesquisas de lkp genérico (chamados via class). Exemplo de uso:
//$(".lkpJornada").bind("keydown click", function (e) {
//      cwk_EventoConsultaUnico(e, this, "", "Parametro", "");
//});
//     event: passar como parametro o evento do objeto que chamou a função
//    objeto: passar o objeto que fez a chamado
//      acao: método do controller que será utilizado na pesquisa, caso passe vazio ("") a função assumirá como "EventoConsulta" que é o nome padrão do métodos de pesquisa.
//controller: controller responsável pelo método utilizado para pesquisa.
//    filtro: campo auxiliar que será utilizado no filtro da pesquisa, caso a pesquisa não dependa de nenhum outro campo passar vazio ("")
//    posicaoNome: posicao do campo, no select,  que ficara depois do |
function cwk_EventoConsultaUnico(event, objeto, acao, controller, filtro, eventoCallBack, posicaoNome, paramAdicionais, eventoCallBackErro) {
    if (acao == "") {
        acao = "EventoConsulta";
    }
    var idObjeto = objeto.id;
    var valorFiltro = $(filtro).val();
    var lkp = "";
    var keyCode = event.keyCode || event.which;
    // Se o campo NÃO for um "btn" funciona apenas quando precionado a tecla tab
    if (keyCode == 9 && !event.shiftKey && idObjeto.toLowerCase().indexOf("btn") == -1) {
        lkp = "#" + idObjeto;
        var valorlkp = $(lkp).val();
        ajax_CarregarConsultaEventoTab(acao, controller, valorlkp, lkp, valorFiltro, eventoCallBack, posicaoNome, paramAdicionais, eventoCallBackErro);
    }
        // Se o campo  que chamou for um "btn" funciona apenas no método click do botão
    else {
        if (event.type == "click" && idObjeto.toLowerCase().indexOf("btn") >= 0) {
            var idCampo = idObjeto.replace("btn", "");
            var lkp = "#lkp" + idCampo;
            ajax_CarregarConsultaEventoTab(acao, controller, '{#botaopresionado#}', lkp, valorFiltro, eventoCallBack, posicaoNome, paramAdicionais, eventoCallBackErro);
        }
    }
}

//Apenas colocar a classe lkpGenerico no lkp e no btn de qualquer lkp e já vai funcionar
//Tem que ser lkp simples, que não necessita de parametro e tem o evento consulta padrão e o nome do componente tem que ser lkp+NomeController e btn+Nomecontroller.
function lkpGenerico() {
    $(".lkpGen").bind("keydown click", function (e) {
        var controllerPesquisa = this.id;
        controllerPesquisa = controllerPesquisa.replace("btn", "").replace("lkp", "");
        cwk_EventoConsultaUnico(e, this, "", controllerPesquisa, "", "");
    });
}

function setaFocoProximo(campo) {
    //$(".datepickerpt").datepicker('hide');
    var campoR = campo.replace('#', '');
    document.getElementById(campoR).focus();// Coloca  e tira o foco no campo para retirar erro de validação caso tenha dado algum antes.
    document.getElementById(campoR).blur();
    var inputs = $(campo).closest('form').find(':input:visible');
    inputs.eq(inputs.index($(campo)) + 1).focus();
    //indice = $('input:text').index($(campo));/* pega o indice do elemento*/
    //$('input:text')[indice + 1].focus();/* Seta o foco no proximo elemento*/
}

function ajax_CarregarConsultaEventoTabComFiltro(acao, idFiltro, controller, consulta, campo, carregaDetalhes) {
    var vCampo = consulta;
    if (consulta.indexOf('|') !== -1) {
        vCampo = consulta.split("|");
        consulta = vCampo[0];
    }
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        data: { 'consulta': consulta, 'id': idFiltro },
        crossDomain: true,
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
            $('#divLoadModalLkp').html(ret);
            if (oTbLkpModal.fnGetNodes().length === 1) {
                var row = oTbLkpModal.fnGetNodes();
                var id = $(row[0]).find("td:eq(0)").text().replace('undefined', '');
                var nome = $(row[0]).find("td:eq(1)").text().replace('undefined', '');
                $(campo).val(id + ' | ' + nome);
                if (carregaDetalhes) {
                    ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                }
            } else if (oTbLkpModal.fnGetNodes().length === 0) {
                cwkErro("Registro não encontrado. ");
                $(campo).val("");
                $(campo).focus();
            } else {
                $('#divLoadModalLkp').appendTo('Body').modal();
                $('#btSelec').on('click', function () {
                    var id = '';
                    var nome = '';
                    var ids = cwk_GetIdSelecionado(oTbLkpModal);
                    if (ids > 0) {
                        oTbLkpModal.$("tr").filter(".selected").each(function (index, row) {
                            id = $(row).find("td:eq(0)").text().replace('undefined', '');
                            nome = $(row).find("td:eq(1)").text().replace('undefined', '');
                            $(campo).val(id + ' | ' + nome);
                            $("#divLoadModalLkp").modal('hide');
                            if (carregaDetalhes) {
                                ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                            }
                        })
                    } else {
                        cwkErro('Selecione um registro.');
                    }
                });
                $('#tbSel tbody tr').dblclick(function () {
                    var id = '';
                    var nome = '';

                    var ids = cwk_GetIdSelecionado(oTbLkpModal);
                    if (ids > 0) {
                        oTbLkpModal.$("tr").filter(".selected").each(function (index, row) {
                            id = $(row).find("td:eq(0)").text().replace('undefined', '');
                            nome = $(row).find("td:eq(1)").text().replace('undefined', '');

                            $(campo).val(id + ' | ' + nome);
                            $("#divLoadModalLkp").modal('hide');
                            if (carregaDetalhes) {
                                ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                            }
                        })
                    } else {
                        cwkErro('Selecione um registro.');
                    }

                });
            }
        }
    });
}

function ajax_CarregarConsultaEventoTabComFiltroMarcacao(acao, idFiltro, controller, consulta, campo, carregaDetalhes) {
    var vCampo = consulta;
    var dtInicio = $('#dtInicio').val();
    var dtFim = $('#dtFinal').val();

    if (consulta.indexOf('|') !== -1) {
        vCampo = consulta.split("|");
        consulta = vCampo[0];
    }
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        data: { 'consulta': consulta, 'id': idFiltro },
        crossDomain: true,
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText);
            }
        },
        success: function (ret) {
            $('#divLoadModalLkp').html(ret);
            if (oTbLkpModal.fnGetNodes().length === 1) {
                var row = oTbLkpModal.fnGetNodes();
                var id = $(row[0]).find("td:eq(0)").text().replace('undefined', '');
                var nome = $(row[0]).find("td:eq(1)").text().replace('undefined', '');
                $(campo).val(id + ' | ' + nome);
                if (carregaDetalhes) {
                    ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                }
                if (id > 0) {
                    $('#TabelaMarcacao').load('/MarcacaoView/TabelaMarcacao', { idFuncionario: id, dtInicio: dtInicio, dtFim: dtFim });
                }
            } else if (oTbLkpModal.fnGetNodes().length === 0) {
                cwkErro("Registro não encontrado. ");
                $(campo).val("");
                $(campo).focus();
            } else {
                $('#divLoadModalLkp').appendTo('Body').modal();
                $('#btSelec').on('click', function () {
                    var id = '';
                    var nome = '';
                    var ids = cwk_GetIdSelecionado(oTbLkpModal);
                    if (ids > 0) {
                        oTbLkpModal.$("tr").filter(".selected").each(function (index, row) {
                            id = $(row).find("td:eq(0)").text().replace('undefined', '');
                            nome = $(row).find("td:eq(1)").text().replace('undefined', '');
                            $(campo).val(id + ' | ' + nome);
                            $("#divLoadModalLkp").modal('hide');
                            if (carregaDetalhes) {
                                ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                            }
                            if (id > 0) {
                                $('#TabelaMarcacao').load('/MarcacaoView/TabelaMarcacao', { idFuncionario: id, dtInicio: dtInicio, dtFim: dtFim });
                            }
                        })
                    } else {
                        cwkErro('Selecione um registro.');
                    }
                });
                $('#tbSel tbody tr').dblclick(function () {
                    var id = '';
                    var nome = '';

                    var ids = cwk_GetIdSelecionado(oTbLkpModal);
                    if (ids > 0) {
                        oTbLkpModal.$("tr").filter(".selected").each(function (index, row) {
                            id = $(row).find("td:eq(0)").text().replace('undefined', '');
                            nome = $(row).find("td:eq(1)").text().replace('undefined', '');

                            $(campo).val(id + ' | ' + nome);
                            $("#divLoadModalLkp").modal('hide');
                            if (carregaDetalhes) {
                                ajax_CarregarDetalhes(acao + 'Detalhe', controller, id);
                            }
                            if (id > 0) {
                                $('#TabelaMarcacao').load('/MarcacaoView/TabelaMarcacao', { idFuncionario: id, dtInicio: dtInicio, dtFim: dtFim });
                            }
                        })
                    } else {
                        cwkErro('Selecione um registro.');
                    }

                });
            }
        }
    });
}

function Ajax_CarregaGridParametro(acao, controller, id, div) {
    $.ajax({
        type: 'GET',
        url: '/' + controller + '/' + acao,
        dataType: 'html',
        cache: false,
        async: true,
        data: { 'id': id },
        error: function (ex) {
            if (ex.status !== 200) {
                cwkErro("Erro!!\n\n" + ex.status + ': ' + ex.statusText + eval(error));
            }
        },
        success: function (data) {
            $(div).html(data);
        }
    });
}

function Ajax_CarregaTelaParametro(acao, controller, id, idFunc, div) {
    var parametros;
    if ((idFunc !== "") && (idFunc !== 0) && (typeof (idFunc) !== "undefined")) {
        parametros = {
            'id': id,
            'idfunc': idFunc
        };
        cwkErro("Ajax_CarregaTelaParametro");
    }
    else {
        parametros = {
            'id': id
        };
    }
    $.ajax({
        url: '/' + controller + '/' + acao,
        type: 'GET',
        dataType: "html",
        crossDomain: true,
        data: parametros,
        beforeSend: function () {
            $("#loading").modal();
        },
        error: function (ex) {
            $("#loading").modal('hide');
            if (ex.status !== 200) {
                alert("Erro!!\n\n" + ex.status + ': ' + ex.statusText + eval(error));
            }
        },
        success: function (ret) {

            $("#loading").modal('hide');
            $('#divLoadModalLkp').html(ret);
            $('#divLoadModalLkp').appendTo('Body').modal();

        },
        complete: function () {
        }
    });
}

function Ajax_SalvarModal(Objeto, acao, controller, id, divReturn, divModal) {

    $.ajax({
        url: Objeto.action,
        type: Objeto.method,
        data: $(Objeto).serialize(),
        beforeSend: function () {
            $.blockUI({ message: '<h2>Salvando<img src="../../Content/img/circulosLoading.GIF"></h1>' });
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
            }
        },
        success: function (result) {
            if (result.Success === true) {
                $(divModal).modal('hide');
                Ajax_CarregaGridParametro(acao, controller, id, divReturn);
            } else {
                $(divModal).html(result);
            }
        },
        complete: function () {
            $.unblockUI();
        }
    });

}

//Coloca os dados retornados de um controller em uma div na página exibindo um gif de carregando enquanto esses dados são carregados. Exemplo de uma na tela de Marcação
//acao = Método do controller que será executado
// controller = controller a ser chamado
//parametros = parametros a ser parassado para o método do controller (acao)
//div = Div onde os dados serão carregados.
//campo = campo que chamou a acao
function CarregaDadosAjax(acao, controller, parametros, div, campo, callback){
    var url = '/' + controller + '/' + acao;
    $.ajax({
        url: url,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: "html",
        data: parametros,
        cache: false,
        beforeSend: function () {
            $(".desabilitar-ao-carregar").attr("disabled", "disabled");
            $(div).html('<div class="col-md-12"> <div style="left:0;right:0;margin-left:auto;margin-right:auto;"><h1 style="text-align: center">Carregando<img src="../../Content/img/circulosLoading.GIF"></h1></div> </div>');
        },
        error: function (e, xhr, settings) {
            //alert(this.url);
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
        },
        success: function (result) {
            if (result.indexOf("Usuario/LogIn") > -1) {
                window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
            }
            else {
                if (result.erro) {
                    cwkErro(result.mensagemErro);
                    $(div).html("");
                }
                else {
                    $(div).html(result);
                    if (callback != null && callback != "" && callback != undefined) {
                        callback();
                    }
                }
            }
        },
        complete: function () {
            if (campo !== null && campo !== "" && campo !== undefined) {
                $(".datepickerpt").datepicker('hide');
                $(campo).focus();
            }
            $(".desabilitar-ao-carregar").removeAttr("disabled");
        },
    })
};

//Essa função chama um método do controller que necessita que a progress bar pradrão seja exibida e após ela terminar executa uma ação.
//acao = Método do controller que será executado
//controller = controller a ser chamado
//parametros = parametros a ser parassado para o método do controller (acao)
//fCallBack = Função javascript que será executada após o termino da progress bar.
//Exemplo de uso na TabelaMarcacao.
function ExecutaAjaxComProgress(acao, controller, parametros, fCallBack) {
    var url = '/' + controller + '/' + acao;
    $.ajax({
        url: url,
        method: 'POST',
        data: parametros,
        success: function (data) {
                if (data.JobId != "" && data != null && data.JobId != undefined) {
                    trackJobProgress(data, fCallBack);
                }
                else {
                    if (data.indexOf("Usuario/LogIn") > -1) {
                        window.location.href = '/Usuario/LogIn?ReturnUrl=' + window.location.href;
                    }
                    else {
                        ProgressLimpo();
                        $("#divModalProgress").modal('hide');
                        cwkErro(data.Erro);
                    }
                }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
            }
        },
    });
};

//Chama controller do job para verificar se tem algum processo ocorrendo para mostrar a progress
function verificaProgress() {
    $.ajax({
        url: '/job/GetJob',
        method: 'POST',
        success: function (data) {
            if (data.JobId != "" && data != null && data.JobId != undefined) {
                trackJobProgress(data);
            }
            else {
                if ($('#divModalProgress').hasClass('in')) {
                    ProgressLimpo();
                    $("#divModalProgress").modal('hide');
                    console.log("progress fechada forçada");
                }
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            $.unblockUI();
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
                $("#divModalProgress").modal('hide');
                console.log("progress fechada com erro");
            }
        },
    });
}

function ProgressPreenchido(msgProgress) {
    $("#progresssBar").addClass("progress-striped");
    $("#progresssBar").addClass("active");
    $("#caixaTexto").html(msgProgress);
    $("#progresssBarValue").css("width", 100 + "%");
}

function ProgressLimpo() {
    $("#progresssBar").removeClass("progress-striped");
    $("#progresssBar").removeClass("active");
    $("#caixaTexto").html('');
    $("#progresssBarValue").css("width", 0 + "%");
}
// Controla a progress bar
function trackJobProgress(job, fCallBack, fCallBackErro, abrirNovaAba) {
    
    $("#divModalProgress").modal();
    setProgressBarWidth(job.Progress);
    $("#completedDisplay").hide();
    var $focused = $(':focus');
    if ($focused.hasClass("datepickerpt")) {
        $($focused).datepicker('hide');
    }

    var hubProxy = $.connection.progressHub;

    hubProxy.client.progressChanged = function (jobId, progress, msgProgress) {
        setProgressBarWidth(progress, msgProgress);
    };

    hubProxy.client.jobCompleted = function (jobId, resultado, job) {
        $("#completedDisplay").show();
        $("#startButton").prop('disabled', false);
        $("#divModalProgress").modal('hide');
        var job = JSON.parse(job);
        if (!job.IsErro) {
            if (fCallBack && typeof (fCallBack) !== "undefined" && fCallBack !== "") {
                fCallBack(resultado);
            }
            if (job.msgAviso != '' && job.msgAviso != undefined) {
                cwkAlertaTit('Aviso!', job.msgAviso);
            }
            if (job.msgSucesso != '' && job.msgSucesso != undefined) {
                cwkSucessoTit('Operação realizada com sucesso.', job.msgSucesso);
            }
            if (resultado == true) {
                var url = '/job/ArquivoRetorno/' + job.Id;
                if (job.TipoArquivo != null && typeof job.TipoArquivo !== "undefined" && job.TipoArquivo == "HTML") {
                    $("#divLoadRelatorio").load(url);
                    $("#divRelatorio").modal();
                }
                else {
                    if (job.CompleteNovaAba) {
                        var wnd = window.open(url, '_blank');
                    }
                    else {
                        window.location = url;
                    }
                }
            }
        }
        else {
            cwkErro(job.msgErro);

            if (fCallBackErro && typeof (fCallBackErro) !== "undefined" && fCallBackErro !== "") {
                fCallBackErro();
            }
        }
        $.connection.hub.stop();
    };

    $.connection.hub.start().done(function () {
        hubProxy.server.trackJob(job.JobId);
    });
}

function setProgressBarWidth(progress, msgProgress) {
    // Quando estiver em um estatus sem progresso seta a progress animada.
    if ((progress == 0 && ((msgProgress != null && msgProgress != undefined && msgProgress.indexOf("Carregando") >= 0 && msgProgress.indexOf("dados") >= 0) || msgProgress == 'Salvando Marcações...')) || progress == -1) {
        ProgressPreenchido(msgProgress);
    }
    else {
        $("#progresssBar").removeClass("progress-striped");
        $("#progresssBar").removeClass("active");
        $("#caixaTexto").html(msgProgress);
        $("#progresssBarValue").css("width", progress + "%");
    }
}


// Componente para carregar a grid de funcionários por ajax
var EidDivPartial = null;
var EidCampoSelecionados = null;
function E_GridFunc(EidDivPartial, EidCampoSelecionados, FuncPosCarregamento) {
    EidDivPartial = $(EidDivPartial);
    EidCampoSelecionados = $(EidCampoSelecionados);
    if (!EidDivPartial.find('table').length > 0) {
        cwk_CarregarPartialAjaxPorPost('GridFuncionariosPost', 'FuncionariosRelatorio', parametros = { idsSelecionados: EidCampoSelecionados.val() }, '', AfterLoad);
    }

    function AfterLoad(fCallBack) {
        if (FuncPosCarregamento && typeof (FuncPosCarregamento) !== "undefined" && FuncPosCarregamento !== "") {
            FuncPosCarregamento();
        }
    }
}

function E_GridFuncGetSelecionados() {
    return GetSelecionados('tbFun');
}


//Função para carregar os dados do empregado ativo/inativo/todos.
//opção 0 - empregado inativo
//opção 1 - empregado ativo
//opção 2 - todos
function EventoFuncionarioFiltroRequest(opcao,url)
{
    let _parametros = { opcao: opcao };
    let _url = url;
    let _type = 'Post';
    let _contentType = 'application/json'
    var req = request(_parametros, _url, _type, _contentType, requestBeforeSend('Buscando dados'));

    req.done(function (data)
    {
        EventoFuncionarioFiltroSuccess(data);
        $.unblockUI();
    }).fail(function (xhr, textStatus, errorThrown)
    {
        EventoFuncionarioFiltroError(xhr, textStatus, errorThrown);
        $.unblockUI();
    });
}
function EventoFuncionarioFiltroError(xhr, textStatus, errorThrown) {
    if (xhr.responseText.indexOf('Usuário sem permissão') >= 0)
    {
        cwkErro("Acesso negado, Contate o administrador do sistema!");
    }
    else
    {
        var sErrMsg = "";
        sErrMsg += "Erro: ";
        sErrMsg += "\n\n" + " - Status :" + textStatus;
        sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
        sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
        sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
        cwkErro(sErrMsg);
    }
}
function EventoFuncionarioFiltroSuccess(dados)
{
    let funcDataTables = $('#tbFun').DataTable();
    funcDataTables.clear().draw();
    funcDataTables.rows.add(dados.data);
    funcDataTables.columns.adjust().draw();
}
function EventoFuncionarioRowCollor(row, data, index) {
    if (data.Ativo == 'Não') {
        $('td', row).css('color', 'red');
    }
};
//Metodo genérico para request ajax
//retorna uma requisição
function request(parametros, url, type, contentType, fnBeforeSend) {
    return $.ajax({
        url: url,
        type: type,
        ContentType: contentType,
        data: parametros,
        crossDomain: true,
        beforeSend: fnBeforeSend,
    });
}
function requestBeforeSend(texto) { return $.blockUI({ message: '<h2>' + texto + '<img src="../../Content/img/circulosLoading.GIF"></h1>' }); }

// FIM Componente para carregar a grid de funcionários por ajax


function PostFormJob(form, callBackSucesso, divAbrirJobs) {
    PostReturnJob(form.action, form.method, $(form).serialize(), divAbrirJobs, false, callBackSucesso);
}

function PostReturnJob(url, type, data, divAbrirJobs, LimparComponentes, callBackSucesso)
{
    if (isEmpty(LimparComponentes)) {
        LimparComponentes = true;
    }
    $.ajax({
        url: url,
        type: type,
        data: data,
        beforeSend: function () {
            $.blockUI({ message: '' });
            $("#loading").modal();
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                cwkErro("Acesso negado, Contate o administrador do sistema!");
            }
            else {
                var sErrMsg = "";
                sErrMsg += "Erro: ";
                sErrMsg += "\n\n" + " - Status :" + textStatus;
                sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                cwkErro(sErrMsg);
            }
        },
        success: function (result) {
            try {
                if (result.constructor === Object) {
                    if (result.success === true) {
                        if (LimparComponentes) {
                            $("input[type=text]:enabled").val('');
                        }
                        if (isEmpty(divAbrirJobs)) {
                            divAbrirJobs = window.location.pathname.replace(/\//g, "");
                        }
                        CriarOuReportarProgresso(divAbrirJobs, result.job);
                        var addFilaNotice = new PNotify({
                            title: 'Processo adicionado na fila com sucesso.',
                            text: 'Para acompanhar o progresso verifique o quadro de histórico.',
                            type: 'info',
                            delay: 3000,
                            buttons: {
                                closer: false,
                                sticker: false
                            }
                        });
                        addFilaNotice.get().click(function () {
                            addFilaNotice.remove();
                        });
                        if (callBackSucesso && typeof (callBackSucesso) !== "undefined" && callBackSucesso !== "") {
                            callBackSucesso(result);
                        }
                        try {
                            ScrollPageJobHistorico();
                        } catch (e) {
                            console.log('não foi possível rolar a página');
                        }
                        
                    } else {
                        for (var i = 0; i < result.errors.length; i++) {
                            var error = result.errors[i];
                            var fieldKey = error.Key;
                            var message = error.Message;
                            addErroInput(fieldKey, message);
                        }
                    }
                } else {
                    if(result.indexOf('<form') > 0) {
                        $("html").html(result);
                    }
                }
            }
            catch (e) {
                cwkErroTit('Erro', 'Erro genárico ao processar a solicitação');
            }
            
        },
        complete: function () {
            $.unblockUI();
            $("#loading").modal('hide');
        }
    });
}

function ScrollPageJobHistorico() {
    var divComScroll = GetDivScrollJob();
    if ($("#collapseJobsDNF").length) {
        var descerScroll = $("#" + divComScroll).scrollTop() - $("#" + divComScroll).offset().top - 40 + $("#collapseJobsDNF").offset().top;
        if (descerScroll > 0) {
            $('#' + divComScroll).animate({
                scrollTop: descerScroll
            }, 1000);
        }
    }
}
function ScrollPageJobIncio() {
    var divComScroll = GetDivScrollJob();
        $('#' + divComScroll).animate({
            scrollTop: 0
        }, 1000);
}

function GetDivScrollJob() {
    var divComScroll = "panelBodyFixo";
    if ($("#panelBodyFixodivCenter").length) {
        divComScroll = "panelBodyFixodivCenter";
    } else if ($("#myModal").length) {
        divComScroll = "myModal";
    }
    return divComScroll;
}

function EventoClickPostReturnJob(botao, acao, controller, objetoTabela) {
    $(botao).on('click', function () {
        var id = GetIdSelecionadoTable(objetoTabela);
        if (id > 0) {
            var url = '/' + controller + '/' + acao;
            var type = 'Post';
            var data = { 'id': id };

            PostReturnJob(url, type, data);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de executar a rotina é necessário selecionar um registro!');
        }
    });
}

function EventoClickDeletePostReturnJob(botao, acao, controller, nomeTabela, mensagem, callBackSucesso, mensagemConfirmacaoPersonalizada) {
    $(botao).on('click', function () {
        var id = GetIdSelecionadoTable(nomeTabela);
        if (id > 0) {
            var url = '/' + controller + '/' + acao;

            var mensagemConfirmacao = "Ao confirmar a ação será permanente!";
            if (mensagemConfirmacaoPersonalizada && typeof (mensagemConfirmacaoPersonalizada) !== "undefined" && mensagemConfirmacaoPersonalizada !== "") {
                mensagemConfirmacao = mensagemConfirmacaoPersonalizada;
            }
            bootbox.dialog({
                message: mensagemConfirmacao,
                title: "Deseja realmente excluir?",
                buttons: {
                    success: {
                        label: "Excluir!",
                        className: "btn-primary",
                        callback: function () {
                            $.ajax({
                                url: url,
                                type: 'POST',
                                dataType: 'json',
                                data: { 'id': id },
                                beforeSend: function () {
                                    $.blockUI({ message: '<h2>Excluindo<img src="../../Content/img/circulosLoading.GIF"></h2>' });
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    $.unblockUI();
                                    if (xhr.responseText.indexOf('Usuário sem permissão') >= 0) {
                                        cwkErro("Acesso negado, Contate o administrador do sistema!");
                                    }
                                    else {
                                        $.unblockUI();
                                        var sErrMsg = "";
                                        sErrMsg += "Erro: ";
                                        sErrMsg += "\n\n" + " - Status :" + textStatus;
                                        sErrMsg += "\n\n" + " - Status Erro :" + xhr.status;
                                        sErrMsg += "\n\n" + " - Tipo Erro :" + errorThrown;
                                        sErrMsg += "\n\n" + " - Mensagem Erro :" + xhr.responseText;
                                        cwkErro(sErrMsg);
                                    }
                                },
                                success: function (ret) {
                                    $.unblockUI();
                                    if ((ret.success === true)) {
                                        cwkSucessoTit('Registro Excluído!', mensagem);
                                        var objetoTabela = $(nomeTabela).closest('table').DataTable();
                                        cwk_RemoverLinhasSelecionadas(objetoTabela);
                                        if (callBackSucesso && typeof (callBackSucesso) !== "undefined" && callBackSucesso !== "") {
                                            callBackSucesso(ret);
                                        }
                                        var divAbrirJobs = window.location.pathname.replace(/\//g, "");
                                        CriarOuReportarProgresso(divAbrirJobs, ret.job);
                                    }
                                    else {
                                        if (ret.Aviso != undefined && ret.Aviso != "" && ret.Aviso == true) {
                                            cwkNotificacaoTit('Alerta!', ret.Erro);
                                        }
                                        else {
                                            cwkErroTit('Erro!', ret.Erro);
                                        }
                                    }
                                },
                            });
                        }
                    },
                    danger: {
                        label: "Cancelar!",
                        className: "btn-default",
                        callback: function () {

                        }
                    }
                }
            });
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de executar a rotina é necessário selecionar um registro!');
        }
    });
}