
// PRESTAR ATENÇÃO QUANDO OS MÉTODOS PEDEM NOME TABELA (ID - #ALGUMACOISA) E QUANDO PEDE O OBJETO DA TABELA
// ADICIONAR ESSA BIBLIOTECA DEPOIS DO DATATABLES, NO BUNDLE.
// AUTOR: MAYKON RISSI

// Monta um datatable e retorna o objeto para uma variável, que deve ser usado para os outros métodos. 
// O Método já aplica o estilo e as classes.
function cwk_MontarDataTable(nomeTabela, temBotoes, DestruirAoFechar, TemFiltro, TemPaginacao, TemInfos, TemUi, PrimeiroRegistroPagina, UltimoRegistroPagina, PercPagina) {

    $(nomeTabela).addClass('table table-condensed table-striped table-bordered display');
    $(nomeTabela).attr("cellpadding", 0);
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("border", 0);

    $.ajaxSetup({ cache: false });
    var tb;
    if (PercPagina == 0) {
        PercPagina = 50;
    }
    if (temBotoes) {
        tb = $(nomeTabela).dataTable({
            "sDom": "<'row'<'col-xs-6'T><'col-xs-6'f>r>t<'row'<'col-xs-6'i><'col-xs-6'p>>",
            "bJQueryUI": true,
            "sPaginationType": "full_numbers",
            bjQueryUI: TemUi,
            "sScrollY": (cwk_AlturaTela() - (cwk_AlturaTela() * PercPagina) / 100) + 'px',
            "sScrollX": true,
            "iDisplayStart": PrimeiroRegistroPagina,
            "iDisplayLength": UltimoRegistroPagina,
            "oTableTools": {
                "sSwfPath": '../scripts/swf/copy_csv_xls_pdf.swf',
                "aButtons": [
                {
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "sExtends": "copy",
                    "sButtonText": 'Copiar'
                },
                {
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "sExtends": "print",
                    "sButtonText": 'Imprimir'
                },
                {
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "sExtends": "collection",
                    "sButtonText": 'Salvar <span class="caret" />',
                    "aButtons": ["csv", "xls", "pdf"]
                }
                ]
            },
            "oLanguage": {
                'decimal': ',',
                'thousands': '.',
                'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
            },
            "bFilter": TemFiltro,
            "bInfo": TemInfos,
            "bPaginate": TemPaginacao,
            "bDestroy": DestruirAoFechar,
            "sScrollX": true
        });
    } else {
        tb = $(nomeTabela).dataTable({
            sPaginationType: "bootstrap",
            "sScrollY": (cwk_AlturaTela() - (cwk_AlturaTela() * PercPagina) / 100) + 'px',
            bjQueryUI: TemUi,
            "iDisplayStart": PrimeiroRegistroPagina,
            "iDisplayLength": UltimoRegistroPagina,
            "oLanguage": {
                'decimal': ',',
                'thousands': '.',
                'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
            },
            "bFilter": TemFiltro,
            "bInfo": TemInfos,
            "bPaginate": TemPaginacao,
            "bDestroy": DestruirAoFechar
        });
    }
    return tb;
}

function cwk_MontarDataTableTheme(nomeTabela, nomeTela, DiminuiGrid, altura, largura, ordem, paginar, qtdRegistroPagina) {

    var oTable;
    var retiraAltura = 0;
    var alturaNum = 0;
    if ((altura == '' || altura == null) || (altura.toString().indexOf('-') > -1)) {
        if (altura.indexOf('-') > -1) {
            retiraAltura = parseFloat(altura);
        }
        alturaNum = cwk_AlturaTela() - utilizadoGridGenerica();
        alturaNum = alturaNum - 160; // Grid Header 40 + Titulos Colunas 25 + Footer 40 + Padding 30 + Margem das "Divs Centers" = 35
        alturaNum = (alturaNum + retiraAltura)
        altura = alturaNum + 'px';
    }

    if (ordem == null) {
        ordem = 1;
    }

    if (paginar == null || paginar == "" || paginar == undefined) {
        paginar = false;
    } else {
        if (paginar) {
            if (qtdRegistroPagina == null || qtdRegistroPagina == "" || qtdRegistroPagina == undefined) {
                var qtd = alturaNum / 24; // 24 é o tamanho de cada linha da grid condensed
                qtd = Math.round(qtd);
                qtdRegistroPagina = qtd;
            }
        }
    }

    oTable = $(nomeTabela).DataTable({
        "scrollY": altura,
        "scrollX": true,
        "bScrollCollapse": DiminuiGrid,
      
        "bAutoWidth": true,
        "bSaveState": true,
        "sDom": '<"H"Tfr>t<"F"ip>',
        "pagingType": "full_numbers",
        "bPaginate": paginar,
        "iDisplayLength": qtdRegistroPagina,
        "aaSorting": [[ordem, "asc"]],
        "oLanguage": {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
        },
        "oTableTools": {
            "sSwfPath": '../../scripts/swf/copy_csv_xls_pdf.swf',
            "aButtons": [
                {
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "sExtends": "copy",
                    "sButtonText": "Copiar",
                    "fnComplete": function (nButton, oConfig, oFlash, sFlash) {
                        cwkAlerta('Dados copiados para área de transferencia!');
                    }
                },
                {
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "sExtends": "print",
                    "sButtonText": "Imprimir",
                    "sInfo": "Para sair do modo de impressão precione a teclas Esc (Canto superior esquerdo do seu Teclado)"
                },
                {
                    "sExtends": "collection",
                    "sButtonText": "Salvar",
                    "aButtons": [{
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "sExtends": "csv",
                        "sFileName": nomeTela + ".csv"
                    },
                                 {
                                     "oSelectorOpts": { filter: 'applied', order: 'current' },
                                     "sExtends": "xls",
                                     "sFileName": nomeTela + ".xls"
                                 },
                                 {
                                     "oSelectorOpts": {
                                     filter: 'applied', order: 'current' },
                                     "sExtends": "pdf",
                                     "sFileName": nomeTela + ".pdf"
                                 }, ]
                }
            ]
        }
    });
    $(nomeTabela).addClass('display compact');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).css("margin", 0);

    return oTable;
};

function cwk_MicroTBTheme(nomeTabela, altura, bfiltro) {
    var oTable;

    var Alturapx = altura;

    if (Alturapx == 0) {
        Alturapx = 458;
    }

    oTable = $(nomeTabela).DataTable({
        "sScrollY": (cwk_AlturaTela() - Alturapx) + 'px',
        "sScrollX": true,

        "bSortClasses": false,
        "bPaginate": false,
        "aaSorting": [[1, "asc"]],
        "bFilter": bfiltro,
        "bDestroy": true,
        "sPaginationType": "scrolling",
        "sDom": '<"H"fr>t<"F"ip>',
        "oLanguage": {
            'decimal': ',',
            'thousands': '.',
            "sUrl": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json",
            buttons: {
                copyTitle: 'Copiado para área de transferência.',
                copySuccess: {
                    _: 'Copiados %d registros',
                    1: 'Copiado 1 registro'
                }
            },
            select: {
                rows: {
                    _: '(Selecionados %d registros)',
                    0: '(Click em um registro para selecioná-lo)',
                    1: '(Selecionado 1 registro)'
                }
            }
        }
    });

    $(nomeTabela).addClass('display compact');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("border", 0);

    return oTable;
}
//table totalizadores
function cwk_MicroTBTheme2(nomeTabela, altura, largura, bfiltro, sDom, colunaOrdenar) {
    var oTable;
    if (altura == '' || altura == null) {
        altura = "420px";
    }
    if (altura.toString().indexOf('px') == -1 || altura.toString().indexOf('%') == -1) {
        altura = altura + 'px';
    }
    if (sDom == "" || sDom == undefined) {
        sDom = '<"H"fr>t<"F"ip>';
    }

    if (colunaOrdenar === "" || colunaOrdenar === undefined || colunaOrdenar === null) {
        colunaOrdenar = 1;
    }
    oTable = $(nomeTabela).dataTable({
        orderCellsTop: true,
        dom: "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        cache: false,
        scrollX: true,
        scrollY: altura,
        scrollCollapse: false,
        paginate: false,
        select: { style: 'single' },
        language: {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
            select: {
                rows: {
                    _: '',
                    0: '',
                    1: ''
                }
            }
        },
    });
    $(nomeTabela).addClass('display compact');
    $(nomeTabela).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("border", 0);

    return oTable;
}

function cwk_MontarDataTableThemeMarcacao(nomeTabela, altura, colunasDinamicas) {
    var oTable;
    $.fn.dataTable.moment = function (format, locale) {
        var types = $.fn.dataTable.ext.type;

        // Add type detection
        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ?
                'moment-' + format :
                null;
        });

        // Add sorting method - use an integer for the sorting
        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    };

    $.fn.dataTable.moment('DD/MM/YYYY');
    var retiraAltura = 0;
    var alturaNum = 0;
    if ((altura == '' || altura == null) || (altura.toString().indexOf('-') > -1)) {
        if (altura.indexOf('-') > -1) {
            retiraAltura = parseFloat(altura);
        }
        alturaNum = cwk_AlturaTela() - utilizadoGridGenerica();
        alturaNum = alturaNum - 50 ; 
        alturaNum = (alturaNum + retiraAltura)
        altura = alturaNum + 'px';
    }

    var aoColumns = [
          { "bSortable": true },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false }
    ];

    for (var i = 0; i < colunasDinamicas; i++)
        aoColumns.push({ "bSortable": false });

    oTable = $(nomeTabela).dataTable({
        "orderCellsTop": true,
        "dom": "<'row'<'col-sm-3'l><'col-sm-3 text-center'f><'col-sm-6'>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-5'><'col-sm-7'>>",
        "rowId": "Id",
        "stateSave": true,
        "cache": true,
        "scrollX": true,
        "sScrollY": altura,
        "bPaginate": false,
        "bFilter": false,
        language: {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
        },
        "aoColumns": aoColumns
    });
    $(nomeTabela).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("width", "100%");
    return oTable;
}

function cwk_MontarDataTableThemeManutencaoDiaria(nomeTabela, altura, colunasDinamicas) {
    var oTable;
    $.fn.dataTable.moment = function (format, locale) {
        var types = $.fn.dataTable.ext.type;

        // Add type detection
        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ?
                'moment-' + format :
                null;
        });

        // Add sorting method - use an integer for the sorting
        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    };

    $.fn.dataTable.moment('DD/MM/YYYY');
    var retiraAltura = 0;
    var alturaNum = 0;
    if ((altura == '' || altura == null) || (altura.toString().indexOf('-') > -1)) {
        if (altura.indexOf('-') > -1) {
            retiraAltura = parseFloat(altura);
        }
        alturaNum = cwk_AlturaTela() - utilizadoGridGenerica();
        alturaNum = alturaNum - 50;
        alturaNum = (alturaNum + retiraAltura)
        altura = alturaNum + 'px';
    }

    var aoColumns = [
          { "bSortable": true },
          { "bSortable": true },
          { "bSortable": true },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false },
          { "bSortable": false } 
    ];

    for (var i = 0; i < colunasDinamicas; i++)
        aoColumns.push({ "bSortable": false });
    aoColumns.push({ "bSortable": false, "bVisible": false });


    oTable = $(nomeTabela).DataTable({
        "orderCellsTop": true,
        "dom": "<'row'<'col-sm-3'l><'col-sm-3'><'col-sm-6'f>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-5'><'col-sm-7'>>",
        "rowId": "Id",
        "scrollX": true,
        "sScrollY": altura,
        "bPaginate": false,

        language: {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
                                 },
        "aoColumns": aoColumns
    });
    $(nomeTabela).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("width", "100%");
    return oTable;
}

function cwk_MontarDataTableThemeHorariosFlexiveis(nomeTabela) {
    var oTable;
    $.fn.dataTable.moment = function (format, locale) {
        var types = $.fn.dataTable.ext.type;

        // Add type detection
        types.detect.unshift(function (d) {
            return moment(d, format, locale, true).isValid() ?
                'moment-' + format :
                null;
        });

        // Add sorting method - use an integer for the sorting
        types.order['moment-' + format + '-pre'] = function (d) {
            return moment(d, format, locale, true).unix();
        };
    };

    $.fn.dataTable.moment('DD/MM/YYYY');

    oTable = $(nomeTabela).DataTable({
        orderCellsTop: true,
        dom: "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        cache: true,
        scrollX: true,
        scrollY: '310px',
        paginate: false,
        select: { style: 'single' },
        scrollCollapse: true,
        language: { 
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
        }
    });

    $(nomeTabela).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("width", "100%");
    return oTable;
}

function cwk_MicroTBRelatorios(nomeTabela, altura, bfiltro) {
    var oTable;

    if (altura == '' || altura == null) {
        altura = "458px";
    }

    oTable = $(nomeTabela).DataTable({
        orderCellsTop: true,
        dom: "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        cache: true,
        scrollX: true,
        scrollY: '310px',
        paginate: false,
        select: { style: 'single' },
        scrollCollapse: true,
        language: {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
            'select': {
                'rows': {
                    _: "Selecionado %d rows",
                    0: "Selec. um registro",
                    1: "Selec. 1 registro"
                }
            }
        }
    });

    $(nomeTabela).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("width", "100%");
    return oTable;
}

function fnGetSelected(oTableLocal) {
    return oTableLocal.$('tr.selected');
}

//Tabela pequena para exibição.
function cwk_MicroTB(nomeTabela) {

    $(nomeTabela).addClass('display compact');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("border", 0);

    $.ajaxSetup({ cache: false });
    var tb;

    tb = $(nomeTabela).dataTable({
        bjQueryUI: true,
        "iDisplayStart": 0,
        "iDisplayLength": 9,
        "bFilter": false,
        "bInfo": false,
        "bPaginate": false,
        "oLanguage": {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
        },
        "bDestroy": true
    });

    return tb;
}

function cwk_MicroTB2(nomeTabela, altura) {

    if (altura == '' || altura == null) {
        altura = "458px";
    }

    $(nomeTabela).addClass('display compact');
    $(nomeTabela).attr("cellspacing", 0);
    $(nomeTabela).attr("border", 0);

    $.ajaxSetup({ cache: false });
    var tb;

    tb = $(nomeTabela).dataTable({
        bjQueryUI: true,
        "sScrollY": altura,
        "iDisplayStart": 0,
        "iDisplayLength": 9,
        "bFilter": false,
        "bInfo": false,
        "bPaginate": false,
        "language": {
            'decimal': ',',
            'thousands': '.',
            'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
            buttons: {
                copyTitle: 'Copiado para área de transferência.',
                copySuccess: {
                    _: 'Copiados %d registros',
                    1: 'Copiado 1 registro'
                }
            },
            select: {
                rows: {
                    _: '(Selecionados %d registros)',
                    0: '(Click em um registro para selecioná-lo)',
                    1: '(Selecionado 1 registro)'
                }
            }
        },
        "bDestroy": true
    });

    return tb;
}



// Métodos Datatable Relatório
function cwk_DataTableRelatorio(idTabela, altura, paginar, qtdRegistroPagina, CampoOrdenar, dom, tituloGrid, diminuir, CallBackSelecao) {
    var retiraAltura = 0;
    var alturaNum = 0;
    if ((altura == '' || altura == null) || (altura.toString().indexOf('-') > -1)) {
        if (altura.indexOf('-') > -1) {
            retiraAltura = parseFloat(altura);
        }
        alturaNum = cwk_AlturaTela() - utilizadoGridGenerica();
        alturaNum = alturaNum - 50 ; 
        alturaNum = (alturaNum + retiraAltura)
        altura = alturaNum + 'px';
    }
        
        

    if (paginar == null || paginar == "" || paginar == undefined) {
        paginar = false;
    } else {
        if (paginar) {
            if (qtdRegistroPagina == null || qtdRegistroPagina == "" || qtdRegistroPagina == undefined) {
                var qtd = alturaNum / 24; // 24 é o tamanho de cada linha da grid condensed
                qtd = Math.round(qtd);
                qtdRegistroPagina = qtd;
            }
        }
    }

    if (CampoOrdenar == null) {
        CampoOrdenar = 1;
    }

    //Adiciona os textbox para filtro na tabela.
    $('#' + idTabela + ' thead tr#filterrow th').each(function () {
        var title = $('#' + idTabela + ' thead th').eq($(this).index()).text().replace('.', '') + idTabela;

        var inputbutton = '<div class="input-group  input-group-sm">' +
                               '<input type="text" class="form-control" placeholder="Pesquisar ' + title.replace(idTabela,'') + '" name="' + title + '" list="' + title + 'list"/> ';
        if ($(this).hasClass('addLista')) {
            inputbutton = inputbutton;
        }
        inputbutton = inputbutton + 
                            '</div>';

        $(this).html(inputbutton);


    });  

    if (dom == null || dom == "" || dom == undefined) {
        dom = '<"H"<"floatLeft"lr><"btnsSelect"><"centerDom"f>>t<"F"ip>';
        if (tituloGrid != null && tituloGrid != "" && tituloGrid != undefined) {
            dom = '<"H"<"floatLeft"lr><"btnsSelect"><"centerDom"<"titulo">>>t<"F"ip>';
        }
    }

    var msgInfo = "_TOTAL_ Registros";
    if (paginar) {
        msgInfo = "Mostrando de _START_ at&eacute; _END_ de _TOTAL_ reg.";
    }

    if (diminuir == undefined) {
        diminuir = true;
    }
    
    // Cria o Datatable
    var table = $('#' + idTabela).DataTable({
        "orderCellsTop": true,
        "dom":    "<'row'<'col-sm-2'f><'col-sm-3 text-center'l><'col-sm-7 text-right'B>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "rowId": "Id",
        "select": { style: 'multi' },
        "stateSave": false,
        "cache": true,
        "scrollY": "195px",
        "scrollX": true,
        "bPaginate": false,
        "pagingType": 'full_numbers',
        
        "language": {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": " _MENU_ por página",
            "zeroRecords": "N&atilde;o foram encontrados resultados",
            "info": msgInfo,
            "infoEmpty": "0 resultados",
            "infoFiltered": "(filtrado de _MAX_)",
            "search": "Pesquisar:",
            buttons: {
                copyTitle: 'Copiado para área de transferência.',
                copySuccess: {
                    _: 'Copiados %d registros',
                    1: 'Copiado 1 registro'
                }
            },
            select: {
                rows: {
                    _: '(Selecionados %d registros)',
                    1: '(Selecionado 1 registro)'
                }
            }
        },
        buttons: [
                        
           {
               text: '<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>',
               titleAttr: 'Filtrar ou Retirar filtro de selecionados',
               action: function () {                   
                   if (this.text() == '<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>') {
                       if (table.rows('.selected').count() > 0) {
                           table.columns(0).search('^(' + table.rows({ selected: true }).ids().toArray().join('|') + ')$', true, false).draw();
                           
                       }
                       this.text('<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-check-square-o"></i>');
                   }
                   else {
                       this.text('<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>');
                       table.columns(0).search('').draw();
                   }

               }
           },
                                        {
                                            text: '<i class="fa fa-check-square-o"></i>',
                                            titleAttr: 'Selecionar',
                                            action: function () {
                                                table.rows({ filter: 'applied' }).select();
                                                + table.NomeTabela + GetSelecionados();
                                                                }
                                        },
                                            {
                                                text: '<i class="fa fa-square-o"></i>',
                                                titleAttr: 'Desselecionar',
                                                action: function () {
                                                    table.rows({ filter: 'applied' }).deselect();
                                                    + table.NomeTabela + GetSelecionados();
                                                }
                                            },
                                            {
                                                text: '<i class="fa fa-retweet"></i>',
                                                titleAttr: 'Inverter Seleção',
                                                action: function (e, dt, button, config) {
                                                var rowsSelected = table.rows({ filter: 'applied', selected: true });
                                                var rowsDeselect = table.rows({ filter: 'applied', selected: false });
                                                rowsSelected.deselect();
                                                rowsDeselect.select();
                                                + table.NomeTabela + GetSelecionados();
                                                }
                                            }
                        ],
    });

    table.column(0).visible(false);
    
    $("div.dataTables_filter").find("input[type=search]").each(function (ev) {
        if (!$(this).val()) {
            $(this).attr("placeholder", "Pesquisar");
        }
    });

    // Corrige tamanho do panel onde fica o filtro geral e a quantidade de registros da Grid
    $(".dataTables_wrapper .dataTables_filter label").css({ "margin-bottom": "-5px" });

    // Marca/Desmarca registro quando clica na linha
    $('#'+idTabela+' tbody').on('click', 'tr', function () {
        //$(this).toggleClass('selected');
        //var sel = 'N';
        //if ($(this).hasClass('selected')) {
        //    sel = 'S';
        //}
        //table.api().rows($(this)).iterator('row', function (context, index) {
        //    var coluna = table.api().cell(index, '.Selecionar');
        //    if (coluna.data() != null) {
        //        coluna.data(sel);
        //    }
        //});

       
        if (CallBackSelecao != undefined) {
            CallBackSelecao();
        }
    });

     //Aplica o filtro
    $("#" + idTabela + "_wrapper thead tr#filterrow input").on('keyup change input', function () {
        table.column($(this).parent().parent().index() + ':visible')
            .search(this.value, false, false)
            .draw();
    });

$(table).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
$(table).attr("cellspacing", 0);
$(table).attr("width", "100%");
    return table;
}

//gvGridEmpresa
// Métodos Datatable Relatório
function cwk_DataTableRelatorioGridEmp(idTabela, altura, paginar, qtdRegistroPagina, CampoOrdenar, dom, tituloGrid, diminuir, CallBackSelecao) {
    var retiraAltura = 0;
    var alturaNum = 0;
    if ((altura == '' || altura == null) || (altura.toString().indexOf('-') > -1)) {
        if (altura.indexOf('-') > -1) {
            retiraAltura = parseFloat(altura);
        }
        alturaNum = cwk_AlturaTela() - utilizadoGridGenerica();
        alturaNum = alturaNum - 50;
        alturaNum = (alturaNum + retiraAltura)
        altura = alturaNum + 'px';
    }



    if (paginar == null || paginar == "" || paginar == undefined) {
        paginar = false;
    } else {
        if (paginar) {
            if (qtdRegistroPagina == null || qtdRegistroPagina == "" || qtdRegistroPagina == undefined) {
                var qtd = alturaNum / 24; // 24 é o tamanho de cada linha da grid condensed
                qtd = Math.round(qtd);
                qtdRegistroPagina = qtd;
            }
        }
    }

    if (CampoOrdenar == null) {
        CampoOrdenar = 1;
    }

    //Adiciona os textbox para filtro na tabela.
    $('#' + idTabela + ' thead tr#filterrow th').each(function () {
        var title = $('#' + idTabela + ' thead th').eq($(this).index()).text().replace('.', '') + idTabela;

        var inputbutton = '<div class="input-group  input-group-sm">' +
                               '<input type="text" class="form-control" placeholder="Pesquisar ' + title.replace(idTabela, '') + '" name="' + title + '" list="' + title + 'list"/> ';
        if ($(this).hasClass('addLista')) {
            inputbutton = inputbutton;
        }
        inputbutton = inputbutton +
                            '</div>';

        $(this).html(inputbutton);


    });

    if (dom == null || dom == "" || dom == undefined) {
        dom = '<"H"<"floatLeft"lr><"btnsSelect"><"centerDom"f>>t<"F"ip>';
        if (tituloGrid != null && tituloGrid != "" && tituloGrid != undefined) {
            dom = '<"H"<"floatLeft"lr><"btnsSelect"><"centerDom"<"titulo">>>t<"F"ip>';
        }
    }

    var msgInfo = "_TOTAL_ Registros";
    if (paginar) {
        msgInfo = "Mostrando de _START_ at&eacute; _END_ de _TOTAL_ reg.";
    }

    if (diminuir == undefined) {
        diminuir = true;
    }

    // Cria o Datatable
    var table = $('#' + idTabela).DataTable({
        "orderCellsTop": true,
        "dom": "<'row'<'col-sm-2'f><'col-sm-3 text-center'l><'col-sm-7 text-right'B>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        "rowId": "Id",
        "select": { style: 'multi' },
        "stateSave": false,
        "cache": true,
        "scrollY": "195px",
        "scrollX": true,
        "bPaginate": false,
        "pagingType": 'full_numbers',

        "language": {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": " _MENU_ por página",
            "zeroRecords": "N&atilde;o foram encontrados resultados",
            "info": msgInfo,
            "infoEmpty": "0 resultados",
            "infoFiltered": "(filtrado de _MAX_)",
            "search": "Pesquisar:",
            buttons: {
                copyTitle: 'Copiado para área de transferência.',
                copySuccess: {
                    _: 'Copiados %d registros',
                    1: 'Copiado 1 registro'
                }
            },
            select: {
                rows: {
                    _: '(Selecionados %d registros)',
                    1: '(Selecionado 1 registro)'
                }
            }
        },
        buttons: [

           {
               text: '<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>',
               titleAttr: 'Filtrar ou Retirar filtro de selecionados',
               action: function () {
                   if (this.text() == '<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>') {
                       if (table.rows('.selected').count() > 0) {
                           table.columns(0).search('^(' + table.rows({ selected: true }).ids().toArray().join('|') + ')$', true, false).draw();

                       }
                       this.text('<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-check-square-o"></i>');
                   }
                   else {
                       this.text('<i class="fa fa-filter" aria-hidden="true"></i> Selecionados <i class="fa fa-square-o"></i>');
                       table.columns(0).search('').draw();
                   }

               }
           },
                                        {
                                            text: '<i class="fa fa-check-square-o"></i>',
                                            titleAttr: 'Selecionar',
                                            action: function () {
                                                table.rows({ filter: 'applied' }).select();
                                                +table.NomeTabela + GetSelecionados();
                                                $("#idsFuncionariosSelecionados").val(GetSelecionados('tbFun'));
                                                var parametros = {
                                                    IdsEmpresas: GetSelecionados('tbEmp')
                                                };
                                                CarregaDadosAjax('CarregarGridFuncionario', 'EnvioDadosRep', parametros, '#tbGridFuncionario', null);
                                            }
                                        },
                                            {
                                                text: '<i class="fa fa-square-o"></i>',
                                                titleAttr: 'Desselecionar',
                                                action: function () {
                                                    table.rows({ filter: 'applied' }).deselect();
                                                    +table.NomeTabela + GetSelecionados();
                                                    $("#idsFuncionariosSelecionados").val(GetSelecionados('tbFun'));
                                                    var parametros = {
                                                        IdsEmpresas: GetSelecionados('tbEmp')
                                                    };
                                                    CarregaDadosAjax('CarregarGridFuncionario', 'EnvioDadosRep', parametros, '#tbGridFuncionario', null);
                                                }
                                            },
                                            {
                                                text: '<i class="fa fa-retweet"></i>',
                                                titleAttr: 'Inverter Seleção',
                                                action: function (e, dt, button, config) {
                                                    var rowsSelected = table.rows({ filter: 'applied', selected: true });
                                                    var rowsDeselect = table.rows({ filter: 'applied', selected: false });
                                                    rowsSelected.deselect();
                                                    rowsDeselect.select();
                                                    +table.NomeTabela + GetSelecionados();
                                                    $("#idsFuncionariosSelecionados").val(GetSelecionados('tbFun'));
                                                    var parametros = {
                                                        IdsEmpresas: GetSelecionados('tbEmp')
                                                    };
                                                    CarregaDadosAjax('CarregarGridFuncionario', 'EnvioDadosRep', parametros, '#tbGridFuncionario', null);

                                                }
                                            }
        ],
    });

    table.column(0).visible(false);

    $("div.dataTables_filter").find("input[type=search]").each(function (ev) {
        if (!$(this).val()) {
            $(this).attr("placeholder", "Pesquisar");
        }
    });

    // Corrige tamanho do panel onde fica o filtro geral e a quantidade de registros da Grid
    $(".dataTables_wrapper .dataTables_filter label").css({ "margin-bottom": "-5px" });

    // Marca/Desmarca registro quando clica na linha
    $('#' + idTabela + ' tbody').on('click', 'tr', function () {
        //$(this).toggleClass('selected');
        //var sel = 'N';
        //if ($(this).hasClass('selected')) {
        //    sel = 'S';
        //}
        //table.api().rows($(this)).iterator('row', function (context, index) {
        //    var coluna = table.api().cell(index, '.Selecionar');
        //    if (coluna.data() != null) {
        //        coluna.data(sel);
        //    }
        //});


        if (CallBackSelecao != undefined) {
            CallBackSelecao();
        }
    });

     //Aplica o filtro
    $("#" + idTabela + "_wrapper thead tr#filterrow input").on('keyup change input', function () {
        table.column($(this).parent().parent().index() + ':visible')
            .search(this.value, false, false)
            .draw();
    });

    $(table).addClass('table table-striped table-bordered table-hover table-condensed nowrap');
    $(table).attr("cellspacing", 0);
    $(table).attr("width", "100%");
    return table;
}

// Função que pega todos os registros selecionados em uma grid e retorna uma lista de ids
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
function GetSelecionados(idTable) {
    var selecionados = '';
    var table = $('#' + idTable.replace("#", "")).dataTable();
    table.api().rows('.selected').iterator('row', function (context, index) {
        var idSel = table.api().cell(index, 0).data();
        if (selecionados === '') {
            selecionados = idSel;
        }
        else {
            selecionados = selecionados + ',' + idSel;
        }
    });
    return selecionados;
}

// Função que recebe uma lista de ids e seleciona os registros no datatable
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
// Parâmetro selecionados = Ids dos registros a serem selecionados (Ex.: 123,124,125) ***Passar os ids sem espaços entre os números e as virgulas
function SetSelecionados(idTable, selecionados) {    
    var ids = selecionados.split(',');
    var table = $('#' + idTable).DataTable();
    if (ids != null && selecionados != "") {
        table.rows(['#' + ids.join(', #')]).select();
    }
    return selecionados;
}

// Função que recebe uma lista de ids e seleciona os registros no datatable
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
// Parâmetro selecionados = Ids dos registros a serem selecionados (Ex.: 123,124,125) ***Passar os ids sem espaços entre os números e as virgulas
function SetSelecionadosRow(idTable) {
    var table = $('#' + idTable).dataTable();
    table.api().rows().iterator('row', function (context, index) {
        var selecionado = table.api().cell(index, '.Selecionar').data();
        if (selecionado == 'S') {
            $(this.row(index).node()).addClass('selected');
        }
    });
}

// Função para Selecionar todos os registros de uma tabela
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
function SelecionarTodos(idTable) {
    var table = $('#' + idTable).dataTable();
    var rows = table.$('tr', { "filter": "applied" });
    $(rows).each(function (index, el) {
        $(this).addClass('selected');
        table.api().rows($(this)).iterator('row', function (context, index) {
            var ColunaSelecionado = table.api().cell(index, '.Selecionar');
            if (ColunaSelecionado.data() != undefined) {
                ColunaSelecionado.data("S")
            }
        });
    })

}

// Função para Deselecionar todos os registros de uma tabela
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
function DeselecionarTodos(idTable) {
    var table = $('#' + idTable).dataTable();
    var rows = table.$('tr', { "filter": "applied" });
    $(rows).each(function (index, el) {
        $(this).removeClass('selected');
        table.api().rows($(this)).iterator('row', function (context, index) {
            var ColunaSelecionado = table.api().cell(index, '.Selecionar');
            if (ColunaSelecionado.data() != undefined) {
                ColunaSelecionado.data("N")
            }
        });
    })
}

// Função para Inverter Seleção de registros de uma tabela
// Parâmetro idTable = Id da Tabela (Não passar # no início.)
function InverterSelecao(idTable) {
    var table = $('#' + idTable).dataTable();
    var rows = table.$('tr', { "filter": "applied" });
    $(rows).each(function (index, el) {
        var sel = 'N';
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            sel = 'N';
        }
        else {
            $(this).addClass('selected');
            sel = 'S';
        }
        table.api().rows($(this)).iterator('row', function (context, index) {
            var ColunaSelecionado = table.api().cell(index, '.Selecionar');
            if (ColunaSelecionado.data() != undefined) {
                ColunaSelecionado.data(sel)
            }
        });
    })
}
// Fim métodos Datatable Relatório



// Cria o evento de seleção de uma única linha no evento click da tabela. 
function cwk_EventoClickSelecionarSingle(nomeTabela) {
    $(nomeTabela + ' tbody').on('click', 'tr', function () {
        var row = $(this);

        var rowIndex = row.index() + 1;

        if (row.parent().parent().hasClass("DTFC_Cloned")) {
            var rowSel = $(nomeTabela + " tr:eq(" + ($(this).index() + 2) + ")");
            if (rowSel.hasClass('selected')) {
                rowSel.removeClass("selected");
                $(this).removeClass("selected");
            } else {
                $(nomeTabela + " tr").removeClass("selected");
                $(".DTFC_Cloned tr").removeClass("selected");
                rowSel.addClass("selected");
                $(this).addClass("selected");
            }
        } else {
            var rowFixed = $(".DTFC_Cloned tr:eq(" + ($(this).index() + 4) + ")");
            if (rowFixed.hasClass('selected')) {
                rowFixed.removeClass("selected");
                $(this).removeClass("selected");
            } else {
                $(nomeTabela + " tr").removeClass("selected");
                $(".DTFC_Cloned tr").removeClass("selected");
                rowFixed.addClass("selected");
                $(this).addClass("selected");
            }
        }
    });
}

// Cria o evento de seleção de várias linhas no evento click da tabela. 
function cwk_EventoClickSelecionar(nomeTabela) {
    $(nomeTabela + ' tbody').on('click', 'tr', function () {
        var row = $(this);

        var rowIndex = row.index() + 1;

        if (row.parent().parent().hasClass("DTFC_Cloned")) {
            var rowSel = $(nomeTabela + " tr:eq(" + ($(this).index() + 2) + ")");
            if (rowSel.hasClass('selected')) {
                rowSel.removeClass("selected");
                $(this).removeClass("selected");
            } else {
                rowSel.addClass("selected");
                $(this).addClass("selected");
            }
        } else {
            var rowFixed = $(".DTFC_Cloned tr:eq(" + ($(this).index() + 4) + ")");
            if (rowFixed.hasClass('selected')) {
                rowFixed.removeClass("selected");
                $(this).removeClass("selected");
            } else {
                rowFixed.addClass("selected");
                $(this).addClass("selected");
            }
        }
    });
}

// Seleciona o checkbox de uma grid ao clicar na respectiva linha
function cwk_EventoClickTicarCheckBoxTabela(nomeTabela) {
    $(nomeTabela + ' tbody tr').click(function (event) {
        if (event.target.type !== 'checkbox') {
            $(':checkbox', this).trigger('click');
        }
    });
}




///Métodos para Inserção, Edição, Consulta e Exclusão de registro.
/// Pega o id selecionado no dataTable
function GetIdSelecionadoTable(nomeTabela) { 
    var id = 0;    
    try {
        id = parseInt(window[nomeTabela.replace("#", "") + 'GetSelecionado']());
        if (isNaN(id)) {            
            var table = $(nomeTabela).DataTable();
            id = table.rows(table.$("tr.selected")).ids().toArray()[0];
        }
    }
    catch (err) {
        var table = $(nomeTabela).DataTable();
        id = table.rows(table.$("tr.selected")).ids().toArray()[0];
    }
    return id;
}

/// Chama página de cadastro de acordo com um id
function ChamaCadastro(controller, acao, id) {
    if (id > 0) {
        $("#loading").modal();
        var url = "/" + controller + "/" + acao + "/" + id;
        window.location = url;
    }
    else {
        cwkErroTit('Selecione um Registro!', 'Antes de alterar um registro é necessário selecioná-lo!');
    }
}

// Executa o evento de alterar ao dar duplo click numa linha da tabela. Usa o método "CarregaTelaComLoading"
function cwk_EventoDbClickEditar(acao, controller, nomeTabela) {
    $(nomeTabela + ' tbody').on('dblclick', 'tr', function () {
        var id = GetIdSelecionadoTable(nomeTabela);
        ChamaCadastro(controller, acao, id);
    });
}

function cwk_EventoClickCadastroAdicionar(botao, acao, controller, objetoTabela) {
    $(botao).on("click", function () {
        var url = "/" + controller + "/" + acao + "/" + 0;
        window.location = url;
    });
}

function cwk_EventoClickCadastroAlterar(botao, acao, controller, nomeTabela) {
    $(botao).on("click", function () {
        var id = GetIdSelecionadoTable(nomeTabela);
        ChamaCadastro(controller, acao, id);
    });
}

// Adiciona o evento click que irá chamar a tela de excluir do objeto
// Deverá ser passado por parâmetro a mensagem de exclusão.
// Esse método irá usar as funções dentro da biblioteca de métodos ajax. Certifique-se que ela foi declarada.
function cwk_EventoClickCadastroExcluir(botao, acao, controller, nomeTabela, mensagem, callBackSucesso, mensagemConfirmacaoPersonalizada) {
    $(botao).on('click', function () {
        var id = GetIdSelecionadoTable(nomeTabela);
        if (id > 0) {
            var objetoTabela = $(nomeTabela).closest('table').DataTable();
            ajax_ExcluirRegistro(acao, controller, id, mensagem, objetoTabela, callBackSucesso, mensagemConfirmacaoPersonalizada);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de excluir um registro é necessário selecioná-lo!');
        }
    });
}
///Fim Métodos para Inserção, Edição, Consulta e Exclusão de registro.

// Executa o evento de alterar ao dar duplo click numa linha da tabela. Usa o método "CarregaTelaComLoading"
function cwk_DbClickCadastroEditarAjax(acao, controller, nomeTabela, objetoTabela) {
    $(nomeTabela + ' tbody tr').dblclick(function (event) {
        event.preventDefault();
        $(this).addClass('selected');
        var id = cwk_GetIdSelecionado(objetoTabela);
        if (id > 0) {
            Ajax_CarregaTelaParametro(acao, controller, id, 0);
        } else {
            cwkErroTit('Selecione um Registro!', 'Antes de alterar um registro é necessário selecioná-lo!');
        }
    });
}

// Retorna o ID da linha selecionada num datatable.
function cwk_GetIdSelecionado(objetoTabela) {
    return objetoTabela.$('tr.selected').attr('id');
}

// Retorna o ID da linha selecionada num datatable.
function cwk_GetIdsSelecionado(objetoTabela) {
    var ret = new Array();
    objetoTabela.$('tr.selected').toArray().forEach(function (item, index, array) {
        ret.push($(item).attr('id'));
    });
    return ret;
}

// Remove a linha selecionada num datatable.
function cwk_RemoverLinhasSelecionadas(objetoTabela) {
    try {
        objetoTabela.row('.selected').remove().draw(false);
    }
    catch (err) {
        objetoTabela.api().row('.selected').remove().draw(false); // Tenta pela forma do datatable (Antigo)
    }

}

// Retorna uma string de 'ids' dos objetos selecionados num datatable. A primeira célula deve ser o id
function cwk_GerarStringIds(objetoTabela) {
    var rows = objetoTabela.fnGetNodes();
    var ids = '';
    for (var i = 0; i < rows.length; i++) {
        ids += $(rows[i]).find("td:eq(0)").text().replace('undefined', '') + ';';
    }
    return ids;
}

// Retorna uma string de atributos dos objetos selecionados num datatable. Para, por exemplo, 
// selecionar todos da 3 coluna -> var str = cwk_GerarStringsAtributos(objetoTabela, 2);
function cwk_GerarStringsAtributos(objetoTabela, pos) {
    var rows = objetoTabela.fnGetNodes();
    var str = '';
    for (var i = 0; i < rows.length; i++) {
        str += $(rows[i]).find("td:eq(" + pos + ")").text().replace('undefined', '') + ';';
    }
    return str;
}

/**
 *  Função que retorna um array de pesquisa, de acordo com as linhas selecionadas (por meio de 
 *  checkboxes) de uma tabela. Este array será utilizado para filtrar elementos de listas-filhas
 *  Exemplo: Filtrar uma lista de Departamentos de acordo com uma Empresa.
 *  Adicione uma classe aos checkboxes de seleção da tabela. Esta função irá verificar
 *  se cada um deles está checado e irá adicionar o conteúdo da célula indicada pelos índices no
 *  array de retorno.
 *  @param {string} classeBusca Classe de seleção para os checkboxes. 
 *  @param {int} linha Número que identifica o objeto de linha <tr> no container-pai em que o checkbox está. 
 *  Normalmente se utiliza 0 (zero), mas fica à critério do utilizador, identificar qual é o índice para este
 *  campo.
 *  @param {int} coluna Número que identifica a coluna que contém a informação que será utilizada e 
 *  adicionada ao array de retorno.
 *  @memberof cwkMedotosDataTables
 */

function getArrayBusca(classeBusca, linha, coluna) {
    if (classeBusca === null || classeBusca === undefined) {
        classeBusca = '';
        return new Array();
    }
    else {
        var arrayBusca = new Array();
        $(classeBusca).toArray().forEach(function (item, index, array) {
            if ($(item).is(':checked')) {
                arrayBusca.push($(item).parents('tr')[linha].cells[coluna].innerHTML);
            }
        });
        return arrayBusca;
    }
};

/**
 *  Função que realiza o filtro dos elementos de uma tabela, sem remover os objetos da mesma.
 *  Esta função se faz necessária para realizar filtros de acordo com checkboxes selecionados
 *  para enviar aos controllers do ASP.NET MVC, usando os EditorTemplates para listas de ViewModels
 *  (proxies) de relatórios, evitando a perda de informações e problemas no envio de listas filtradas
 *  para os Controllers (já que os métodos de Filter do DataTables removem de fato as linhas da 
 *  tabela, causando problemas com os índices dos objetos de lista).
 *  @param {string} nomeClassePai Nome da classe definida para os elementos de Checkboxes da 
 *  tabela-pai (de onde virão as informações para filtrar as tabelas-filhas)
 *  
 *  @param {int} indiceLinhaTblPai Número que identifica o objeto <tr> dentro da tabela-pai (Ver o
 *  método getArrayBusca, para mais informações)
 *  @param {int} indiceColunaTblPai Número que identifica a coluna da tabela-pai que contém a 
 *  informação de filtro (Ver o método getArrayBusca, para mais informações)
 *  @param {string} idTabelaFilha Identificação da tabela-filha que será filtrada de acordo com os
 *  as linhas checadas na tabela-pai.
 *  @memberof cwkMedotosDataTables
 */

function filtraTabela(nomeClassePai, indiceLinhaTblPai, indiceColunaTblPai, idTabelaFilha) {
    var tbl = $(idTabelaFilha).dataTable();
    $(idTabelaFilha + ' tbody tr').removeClass('hidden');
    var keywords = getArrayBusca(nomeClassePai, indiceLinhaTblPai, indiceColunaTblPai);
    var filter = '';
    keywords.forEach(function (item, index, array) {
        filter = (filter !== '') ? filter + ',' + ':contains("' + item + '")' : ':contains("' + item + '")';
    });
    $(idTabelaFilha + ' tbody tr:not(' + filter + ')').addClass('hidden');
    corrigeColoracaoTabela(idTabelaFilha);
    $(idTabelaFilha).css("width", "100%");
}

/**
 *  Função que realiza o filtro dos elementos de uma tabela, sem remover os objetos da mesma.
 *  Esta função se faz necessária para realizar filtros de acordo com checkboxes selecionados
 *  para enviar aos controllers do ASP.NET MVC, usando os EditorTemplates para listas de ViewModels
 *  (proxies) de relatórios, evitando a perda de informações e problemas no envio de listas filtradas
 *  para os Controllers (já que os métodos de Filter do DataTables removem de fato as linhas da 
 *  tabela, causando problemas com os índices dos objetos de lista).
 *  @param {array} keywords array de valores à serem pesquisados na lista 
 *  @param {string} idTabelaFilha Identificação da tabela-filha que será filtrada
 *  @memberof cwkMedotosDataTables
 */

function filtraTabelaPorTexto(keywords, idTabelaFilha) {
    $(idTabelaFilha).DataTable().search(
        keywords.toString(),
        false,
        true
    ).draw();
}

/**
 * Função que corrige a coloração de linhas pares e ímpares após a realização de um filtro
 *  @param {string} idTabela Identificação da tabela à ser corrigida.
 *  @memberof cwkMedotosDataTables
 */

function corrigeColoracaoTabela(idTabela) {
    
}

function adicionaTodosElementos(idTbOrigem, idTbDestino, nomeListaOrigem, nomeListaDestino, listaChaveValorInputs) {
    var tbOrigem = $(idTbOrigem).DataTable();
    var tbDestino = $(idTbDestino).DataTable();

    $(idTbOrigem).find('tbody').children('tr').addClass('selected');

    var linhas = cwk_GetIdsSelecionado(tbOrigem);

    linhas.forEach(function (item, index, array) {
        var linhaObj = $('#' + item);
        if (!linhaObj.hasClass('hidden')) {
            tbOrigem.row($(linhaObj)).remove().draw();
            tbDestino.row.add(linhaObj).draw();
        }
    });

    updateIndexes(idTbOrigem, nomeListaOrigem, listaChaveValorInputs);
    updateIndexes(idTbDestino, nomeListaDestino, listaChaveValorInputs);
    corrigeColoracaoTabela(idTbOrigem);
    corrigeColoracaoTabela(idTbDestino);
    $(idTbOrigem).find('tbody').children('tr').removeClass('selected');
    $(idTbDestino).find('tbody').children('tr').removeClass('selected');
}

function adicionaElemento(idTbOrigem, idTbDestino, nomeListaOrigem, nomeListaDestino, listaChaveValorInputs) {

    var tbOrigem = $(idTbOrigem).DataTable();
    var tbDestino = $(idTbDestino).DataTable();

    var linhas = cwk_GetIdsSelecionado(tbOrigem);

    linhas.forEach(function (item, index, array) {
        var linhaObj = $('#' + item);
        tbOrigem.row($(linhaObj)).remove().draw();
        tbDestino.row.add(linhaObj).draw();
    });

    updateIndexes(idTbOrigem, nomeListaOrigem, listaChaveValorInputs);
    updateIndexes(idTbDestino, nomeListaDestino, listaChaveValorInputs);
    corrigeColoracaoTabela(idTbOrigem);
    corrigeColoracaoTabela(idTbDestino);
    $(idTbOrigem).find('tbody').children('tr').removeClass('selected');
    $(idTbDestino).find('tbody').children('tr').removeClass('selected');
}

function updateIndexes(idTabela, nomeLista, listaChaveValorInputs) {
    $(idTabela).find("tbody").children("tr").each(function (i) {
        var prefixNome = nomeLista + "[" + i + "].";
        var prefixId = nomeLista + "_" + i + "__";
        var $tr = $(this);

        listaChaveValorInputs.forEach(function (item, index, array) {
            $.each(item, function (key, value) {
                $tr.find("input." + key).attr("name", prefixNome + value);
                $tr.find("input." + key).attr("id", prefixId + value);
            });
        });
    });
};



/// Datatable Nova

function cwk_tbEventroConsulta(nomeTabela, bfiltro) {
$(document).on('keydown', 'input', function (e) {
    if (e.which == 13) e.preventDefault();
});
        $(nomeTabela+' thead tr:eq(1) th').each(function () {
            var title = $(nomeTabela + ' thead tr:eq(0) th').eq($(this).index()).text();
            title = title.replace(/\ /g, '').replace(/\./g, '').replace(/\//g, '');
            if ($(this).hasClass('text') || $(this).hasClass('select')) {
                var tipoCampo = 'text';
                if ($(this).hasClass('select')) {
                    tipoCampo = 'select'
                }
                
                $(this).html('<input class="form-control input-sm" type="text" tipoCampo=' + tipoCampo + ' placeholder="Pesquisar ' + title + '" id="' + nomeTabela.replace('#','') + 'psq' + title + '" />');
            }
        });

        var oTable = $(nomeTabela).DataTable({
 
            orderCellsTop: true,
            dom:    "<'row'<'col-sm-6'l><'col-sm-6'f>>" +
                    "<'row'<'col-sm-12'tr>>" +
                    "<'row'<'col-sm-5'i><'col-sm-7'p>>",
            rowId: 'Codigo',
            scrollX: true,
            pagingType: 'full_numbers',
            select: { style: 'single' },
            order: [ [ 1, "asc"],],
            Filter: bfiltro,
            language: {
                'decimal': ',',
                'thousands': '.',
                'url': '//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Portuguese-Brasil.json',
                buttons: {
                    copyTitle: 'Copiado para área de transferência.',
                    copySuccess: {
                        _: 'Copiados %d registros',
                        1: 'Copiado 1 registro'
                    }
                },
                select: {
                    rows: {
                        _: '(Selecionados %d registros)',
                        0: '(Clique para selecionar)',
                        1: '(Selecionado 1 registro)'
                    }
                }
            },
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'Todos']],
            initComplete: function (oSettings, json) {
                //Adiciona funcionalidades no campo de pesquisa
                this.api().columns().every(function (index) {
                    var coluna = this;
                    var nomeColuna = $(coluna.header()).html();
                    nomeColuna = nomeColuna.replace(/\ /g, '').replace(/\./g, '').replace(/\//g, '');
                    var campoPesquisa = $(nomeTabela + 'psq' + nomeColuna);

                    // Funcionalidade da pesquisa
                    campoPesquisa.on('keyup input', function (event) {
                        event.preventDefault();
                        var val = $(this).val();
                        if (event.keyCode == 13 || val == '' ||
                            $(nomeTabela+'lst' + nomeColuna + ' option').filter(function () { return this.value === val; }).length)// vai disparar a pesquisa quando apertar enter, quando apagar os dados ou quando input preenchido com valor contendo na lista
                        {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                                );
                            if (coluna.search() !== this.value) {
                                coluna
                                        .search(val, true, false)
                                        .draw();
                            }
                            return false;
                        }
                    });
                    //

                    // Se o tipo do campo for lista add lista
                    if (campoPesquisa.attr('tipoCampo') == 'select') {
                        campoPesquisa.attr('list', nomeTabela.replace('#', '') + 'lst' + nomeColuna);

                        var lista = $('<datalist id="'+nomeTabela.replace('#', '') +'lst' + nomeColuna + '"></datalist>')

                        this.data().unique().sort().each(function (d, j) {
                                lista.append('<option value="' + d + '">' + d + '</option>')
                        });

                        campoPesquisa.append(lista);
                    }
                    //
                });
                //
            }
        });        
        
        return oTable;
}