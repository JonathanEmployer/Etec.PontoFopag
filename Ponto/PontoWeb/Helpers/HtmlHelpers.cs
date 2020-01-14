using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PontoWeb.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString LinkToRemoveNestedForm(this HtmlHelper htmlHelper, string linkText, string container, string deleteElement)
        {
            var js = string.Format("javascript:removeNestedForm(this,'{0}','{1}');return false;", container, deleteElement);
            TagBuilder tb = new TagBuilder("a");
            tb.Attributes.Add("href", "#");
            tb.AddCssClass("btn btn-danger input-sm sm desabilitar");

            TagBuilder tbSpan = new TagBuilder("span");
            tbSpan.AddCssClass("glyphicon glyphicon-trash");
            
            tb.Attributes.Add("onclick", js);

            tb.InnerHtml = tbSpan.ToString(TagRenderMode.Normal) + (String.IsNullOrEmpty(linkText) ? "" : "&nbsp;" + linkText);
            
            var tag = tb.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        public static IHtmlString LinkToAddNestedForm<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, string containerElement, string counterElement, string collectionProperty, Type nestedType, string ColunaIndiceOpcionalListagem, object htmlAttributes)
        {
            IDictionary<string, object> tags = new Dictionary<string, object>();
            try
            {
                tags = ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            catch (Exception e)
            {
                tags = null;
            }

            var ticks = DateTime.UtcNow.Ticks;
            var nestedObject = Activator.CreateInstance(nestedType);
            var partial = htmlHelper.EditorFor(x => nestedObject).ToHtmlString().JsEncode();
            partial = partial.Replace("id=\\\"nestedObject", "id=\\\"" + collectionProperty + "_" + ticks + "_");
            partial = partial.Replace("name=\\\"nestedObject", "name=\\\"" + collectionProperty + "[" + ticks + "]");
            var js = string.Format("javascript:addNestedForm('{0}','{1}','{2}','{3}','{4}');return false;", containerElement, counterElement, ticks, partial, ColunaIndiceOpcionalListagem);
            TagBuilder tb = new TagBuilder("a");

            if (tags != null)
            {
                tb.MergeAttributes<string, object>(tags);
            }

            tb.Attributes.Add("href", "#");
            tb.AddCssClass("btn btn-success sm desabilitar");
            TagBuilder tbSpan = new TagBuilder("span");
            tbSpan.AddCssClass("glyphicon glyphicon-plus");
            tb.InnerHtml = tbSpan.ToString(TagRenderMode.Normal) + "&nbsp;" + linkText;
            tb.Attributes.Add("onclick", js);
            tb.InnerHtml = linkText;
            var tag = tb.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        public static IHtmlString LinkToAddNestedFormV2<TModel>(this HtmlHelper<TModel> htmlHelper, string linkText, string containerElement, string counterElement, string collectionProperty, Type nestedType, string divContainerElementMaster, string collectionPropertyMaster, object htmlAttributes)
        {
            long ticksMaster = -1;
            var ticks = DateTime.UtcNow.Ticks;
            string htmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix;
            string htmlFieldPrefixById = "";
            string htmlFieldPrefixNome = "";
            string collectionPropertyMasterById = "";
            string collectionPropertyMasterByName = "";
            
            if (!String.IsNullOrEmpty(htmlFieldPrefix))
            {
                collectionPropertyMasterById = htmlFieldPrefixById = htmlFieldPrefix.Replace('[', '_').Replace(']', '_') + "_";
                collectionPropertyMasterByName = htmlFieldPrefixNome = htmlFieldPrefix + ".";
                
                if (htmlFieldPrefix.Contains("nestedObject"))
                {
                    if (!String.IsNullOrEmpty(collectionPropertyMaster))
                    {
                        ticksMaster = DateTime.UtcNow.Ticks*10;
                        collectionPropertyMasterById = collectionPropertyMaster + "_" + ticksMaster + "__";
                        collectionPropertyMasterByName = collectionPropertyMaster + "[" + ticksMaster + "].";
                    }
                }
            }
            
            IDictionary<string, object> tags = new Dictionary<string, object>();
            try
            {
                tags = ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
            catch (Exception e)
            {
                tags = null;
            }

            
            var nestedObject = Activator.CreateInstance(nestedType);
            var partial = htmlHelper.EditorFor(x => nestedObject).ToHtmlString().JsEncode();
            int posClassCounterElement = partial.IndexOf(counterElement.Replace(".",""));
            int posFechaClassCounterElement = partial.IndexOf('>', posClassCounterElement);
            partial = partial.Insert(posFechaClassCounterElement, "idGrupo=\"" + ticks + "\"");
            partial = partial.Replace("id=\\\"" + htmlFieldPrefixById + "nestedObject", "id=\\\"" + collectionPropertyMasterById + collectionProperty + "_" + ticks + "_");
            partial = partial.Replace("for=\\\"" + htmlFieldPrefixById + "nestedObject", "for=\\\"" + collectionPropertyMasterById + collectionProperty + "_" + ticks + "_");
            partial = partial.Replace("name=\\\"" + htmlFieldPrefixNome + "nestedObject", "name=\\\"" + collectionPropertyMasterByName + collectionProperty + "[" + ticks + "]");
            partial = partial.Replace("data-valmsg-for=\\\"" + htmlFieldPrefixNome + "nestedObject", "data-valmsg-for=\\\"" + collectionPropertyMasterByName + collectionProperty + "[" + ticks + "]");

            var js = string.Format("javascript:addNestedFormV2(this,'{0}','{1}','{2}','{3}','{4}','{5}');return false;", containerElement, counterElement, ticks, partial, divContainerElementMaster, ticksMaster);
            TagBuilder tb = new TagBuilder("a");

            if (tags != null)
            {
                tb.MergeAttributes<string, object>(tags);
            }

            tb.Attributes.Add("href", "#");
            tb.Attributes.Add("id", ticks.ToString());
            tb.AddCssClass("btn btn-success sm desabilitar");
            TagBuilder tbSpan = new TagBuilder("span");
            tbSpan.AddCssClass("glyphicon glyphicon-plus");
            tb.InnerHtml = tbSpan.ToString(TagRenderMode.Normal) + "&nbsp;" + linkText;
            tb.Attributes.Add("onclick", js);
            tb.InnerHtml = linkText;
            var tag = tb.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        private static string JsEncode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            for (i = 0; i < len; i += 1)
            {
                char c = s[i];
                switch (c)
                {
                    case '>':
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        //sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        //sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ')
                        {
                            //t = "000" + Integer.toHexString(c); 
                            string tmp = new string(c, 1);
                            t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        public static MvcHtmlString IsDisabled(this MvcHtmlString htmlString, bool disabled)
        {
            string rawstring = htmlString.ToString();
            if (disabled)
            {
                int index = rawstring.IndexOf("/>");
                rawstring = rawstring.Insert(index, "disabled=\"disabled\"");
            }
            return new MvcHtmlString(rawstring);
        }

        public static MvcHtmlString IsReadonly(this MvcHtmlString htmlString, bool @readonly)
        {
            string rawstring = htmlString.ToString();
            if (@readonly)
            {
                int index = rawstring.IndexOf("/>");
                rawstring = rawstring.Insert(index, "readonly=\"readonly\"");
            }
            return new MvcHtmlString(rawstring);
        }

        public static MvcHtmlString CheckBoxDisabledFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> expression)
        {
            // get the name of the property
            string[] propertyNameParts = expression.Body.ToString().Split('.');
            propertyNameParts = propertyNameParts.Where(val => val != "ToString()").ToArray();
            string propertyName = propertyNameParts.Last();

            // get the value of the property
            Func<TModel, string> compiled = expression.Compile();
            string booleanStr = compiled(html.ViewData.Model);

            // convert it to a boolean
            bool isChecked = false;
            Boolean.TryParse(booleanStr, out isChecked);

            TagBuilder checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("id", propertyName+"Disabled");
            checkbox.MergeAttribute("name", propertyName + "Disabled");
            checkbox.MergeAttribute("type", "checkbox");
            checkbox.MergeAttribute("value", isChecked.ToString());
            checkbox.MergeAttribute("disabled", "disabled");
            if (isChecked)
                checkbox.MergeAttribute("checked", "checked");

            TagBuilder hidden = new TagBuilder("input");
            hidden.MergeAttribute("name", propertyName);
            hidden.MergeAttribute("type", "hidden");
            hidden.MergeAttribute("value", isChecked.ToString());

            return MvcHtmlString.Create(checkbox.ToString(TagRenderMode.SelfClosing) + hidden.ToString(TagRenderMode.SelfClosing));
        }

        static string ID = "1BAD3214-CCA6-4C15-B775-034371CAA7FE";
        public static MvcHtmlString CaptchaImage(this HtmlHelper htmlHelper, object htmlimgAtibute = null)
        {
            var id = Guid.NewGuid();
            var nameImage = string.Format("{0}_Image_{1}", ID, id);
            var nameHCode = string.Format("{0}_HCode_{1}", ID, id);
            string hCode;

            var urlCaptcha = Employer.Infra.Captcha.Captcha.GerarUrlCaptcha(out hCode);

            var html = htmlHelper.Hidden(nameHCode, hCode);
            html = html.Concat(htmlHelper.Image(urlCaptcha, nameImage, htmlimgAtibute));

            return html;
        }

        public static MvcHtmlString CaptchaText(this HtmlHelper htmlHelper, object htmlAtibuteTextBox = null)
        {
            var id = Guid.NewGuid();
            var nameTextBox = string.Format("{0}_TextBox_{1}", ID, id);
            var nameHCode = string.Format("{0}_HCode_{1}", ID, id);
            string hCode;

            var urlCaptcha = Employer.Infra.Captcha.Captcha.GerarUrlCaptcha(out hCode);

            var html = htmlHelper.TextBox(nameTextBox, null, htmlAtibuteTextBox);

            return html;
        }

        public static MvcHtmlString Concat(this MvcHtmlString first, params MvcHtmlString[] strings)
        {
            return MvcHtmlString.Create(first.ToString() + string.Concat(strings.Select(s => s.ToString())));
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src,
            string name,
            object htmlAttributes)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("id", name);
            builder.MergeAttribute("src", src);

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString GridFor<TModel>(this HtmlHelper<TModel> htmlHelper, string tableName, Type type, bool multSelecao, string controllerDados, string acaoDados)
        {
            return GridFor<TModel>(htmlHelper, tableName, type, multSelecao, controllerDados, acaoDados, "","", null,"");
        }

        public static IHtmlString GridFor<TModel>(this HtmlHelper<TModel> htmlHelper, string tableName, Type type, bool multSelecao, string controllerDados, string acaoDados, string rowCallback, string callBackLoad)
        {
            return GridFor<TModel>(htmlHelper, tableName, type, multSelecao, controllerDados, acaoDados, rowCallback, callBackLoad, null,"");
        }

        /// <summary>
        /// Cria um datatable apartir de um modelo e uma fonte de dados
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="tableName">Nome da tabela a ser criada</param>
        /// <param name="type">Modelo com as anotações dos campos a serem considerados</param>
        /// <param name="multSelecao">Indica se a grid permite multi selecão</param>
        /// <param name="controllerDados">Controller onde se encontra a fonte de dados</param>
        /// <param name="acaoDados">Ação que retorna o json com os dados</param>
        /// <param name="rowCallback"> Função a ser executada a cada linha</param>
        /// <param name="callBackLoad">Função a ser executada após o datatable ser carregado</param>
        /// <param name="selecionados">string com os ids separados com vingula que devem vir selecionados (caso não informado será selecionado o que estiver salvo em cache que foi selecionado anteriormente.)</param>
        /// <param name="tamanho">Tamanho corpo da grid (onde ficam as linhas) ex: "350px" ou "($(window).height() - 424)"</param>
        /// <returns></returns>
        public static IHtmlString GridFor<TModel>(this HtmlHelper<TModel> htmlHelper, string tableName, Type type, bool multSelecao, string controllerDados, string acaoDados, string rowCallback, string callBackLoad, string selecionados, string tamanho)
        {
            TableHTML table = Utils.GetPropertiesTableHTML.GetProperties(tableName, multSelecao, controllerDados, acaoDados, type);
            string html = GerarStrTabelaHTML(table);
            string js = JqueryDataTabela(table, rowCallback, callBackLoad, selecionados, tamanho);
            return MvcHtmlString.Create(html + js);
        }

        private static string JqueryDataTabela(TableHTML table, string rowCallback, string callBackLoad, string selecionados, string tamanho)
        {
            string multSelecao = "single";
            string filtroExportacao = "";
            if (table.MultipleSelect)
            {
                multSelecao = "multi";
                filtroExportacao = @"modifier: {
                                                    selected: true
                                                }";
            }
            #region Javascript jquery datatable
            string js = @"
            <script>";
            if (selecionados != null)
	        {
                js += @" var "+ table.NomeTabela+ "Selecionados = '" + selecionados + @"';";
	        }
            else
            {
                js += @" var "+ table.NomeTabela + "Selecionados; ";
            }
            
            js += @" 
            $(document).on('keydown', 'input', function (e) {
                if (e.which == 13) e.preventDefault();
            });
            var " + table.NomeTabela + @"selected = [];
            $(document).ready(function () {
        var nomeTabela = '#" + table.NomeTabela + @"';

        $(nomeTabela+' thead tr:eq(1) th').each(function () {
            var title = $(nomeTabela + ' thead tr:eq(0) th').eq($(this).index()).text();
            var placeH = title;
            title = title.replace(/\//g, '').replace(/\./g, '').replace(/\ /g, '').replace('(','').replace(')','');
            if ($(this).hasClass('text') || $(this).hasClass('select')) {
                var tipoCampo = 'text';
                if ($(this).hasClass('select')) {
                    tipoCampo = 'select'
                }
                
                $(this).html('<input class=""form-control input-sm"" type=""text"" tipoCampo=' + tipoCampo + ' placeholder=""Pesquisar ' + placeH + '"" id=""' + nomeTabela.replace('#','') + 'psq' + title + '"" />');
            }
        });

        $.extend( $.fn.dataTableExt.oSort, {
            'portugues-pre': function ( data ) {
                var a = 'a';
                    var e = 'e';
                    var i = 'i';
                    var o = 'o';
                    var u = 'u';
                    var c = 'c';
                    var special_letters = {
                    'Á': a, 'á': a, 'Ã': a, 'ã': a, 'À': a, 'à': a,
                    'É': e, 'é': e, 'Ê': e, 'ê': e,
                    'Í': i, 'í': i, 'Î': i, 'î': i,
                    'Ó': o, 'ó': o, 'Õ': o, 'õ': o, 'Ô': o, 'ô': o,
                    'Ú': u, 'ú': u, 'Ü': u, 'ü': u,
                    'ç': c, 'Ç': c
                };
                if (data !== null) {
                    for (var val in special_letters)
                        data = data.toString().split(val).join(special_letters[val]).toLowerCase();
                }
                return data;
            },
            'portugues-asc': function(a, b )
            {
                return ((a < b) ? -1 : ((a > b) ? 1 : 0));
            },
            'portugues-desc': function(a, b )
            {
                return ((a < b) ? 1 : ((a > b) ? -1 : 0));
            }
        } );

        $.fn.dataTable.moment('DD/MM/YYYY HH:mm');    //Formatação com Hora
        $.fn.dataTable.moment('DD/MM/YYYY');  

        $.fn.dataTable.moment = function ( format, locale ) {
            var types = $.fn.dataTable.ext.type;
 
            // Add type detection
            types.detect.unshift( function ( d ) {
                return moment( d, format, locale, true ).isValid() ?
                    'moment-'+format :
                    null;
            } );
 
            // Add sorting method - use an integer for the sorting
            types.order[ 'moment-'+format+'-pre' ] = function ( d ) {
                return moment( d, format, locale, true ).unix();
            };
        };

        var obj" + table.NomeTabela + @" = $(nomeTabela).DataTable({
 
            orderCellsTop: true,
            dom:    ""<'row'<'col-sm-3'l><'col-sm-3 text-center'f><'col-sm-6 text-right'B>>"" +
                    ""<'row'<'col-sm-12'tr>>"" +
                    ""<'row'<'col-sm-5'i><'col-sm-7'p>>"",
            rowId: 'Id',
            stateSave: true,
            cache: true,
            scrollX: true,
            processing: true,";
            if (!String.IsNullOrEmpty(tamanho))
            {
                js += @"scrollY: "+tamanho+",";
            }
            js += @"pagingType: 'full_numbers',
            select: { style: '" + multSelecao + @"' },
            buttons: [
                        {
                            extend: 'collection',
                            text: '<i class=""fa fa-bars"" aria-hidden=""true""></i> Exportar',
                            titleAttr: 'Exportar dados da tabela (apenas selecionados)',
                            buttons: [
                                        {
                                            extend: 'copyHtml5',
                                            text: '<i class=""fa fa-files-o""></i> Copiar',
                                            titleAttr: 'Copiar',
                                            exportOptions: {
                                                columns: ':visible',
                                                " + filtroExportacao + @"
                                            }
                                        },
                                        {
                                            extend: 'excelHtml5',
                                            text: '<i class=""fa fa-file-excel-o""></i> Excel',
                                            titleAttr: 'Excel',
                                            exportOptions: {
                                            columns: ':visible',
                                                " + filtroExportacao + @"
                                            }
                                        },
                                        {
                                            extend: 'csvHtml5',
                                            text: '<i class=""fa fa-file-text-o""></i> CSV',
                                            titleAttr: 'CSV',
                                            exportOptions: {
                                                columns: ':visible',
                                                " + filtroExportacao + @"
                                            }
                                        },
                                        {
                                            extend: 'pdfHtml5',
                                            orientation: 'landscape',
                                            pageSize: 'A4',
                                            text: '<i class=""fa fa-file-pdf-o""></i> PDF',
                                            titleAttr: 'PDF',
                                            exportOptions: {
                                                columns: ':visible',
                                                " + filtroExportacao + @"
                                            }
                                        },
                                        {
                                            text: '<i class=""fa fa-file-code-o""></i> Json',
                                            titleAttr: 'Json',
                                            action: function (e, dt, button, config) {
                                                var data = dt.buttons.exportData({
                                                    columns: ':visible',
                                                    " + filtroExportacao + @"
                                                });
                                                delete data.footer;

                                                $.fn.dataTable.fileSave(
                                                    new Blob([JSON.stringify(data)]),
                                                    'Export.json'
                                                );
                                            }
                                        },
                                        {
                                            extend: 'print',
                                            text: '<i class=""fa fa-print""></i> Imprimir',
                                            titleAttr: 'Imprimir',
                                            exportOptions: {
                                                columns: ':visible',
                                                " + filtroExportacao + @"
                                            },
                                            customize: function (win) {
                                                $(win.document.body)
                                                    .css('font-size', '10pt')
                                                    .css('background-image', 'none !important;')
                                                    .css('background-color', 'white')

                                                $(win.document.body).find('#" + table.NomeTabela + @"')
                                                    .addClass('compact')
                                                    .css('font-size', 'inherit');
                                            }
                                        }, 
                                        {
                                          extend: 'colvis',
                                          text: '<i class=""glyphicon glyphicon-eye-open""></i> Exibir/Esconder Colunas',
                                          columns: ':gt(0)'
                                        }
                            ]
                        },";

            if (table.MultipleSelect)
            {
                js += @"{
                                                text: '<i class=""fa fa-check-square-o""></i>',
                                                titleAttr: 'Selecionar',
                                                action: function () {
                                                    obj" + table.NomeTabela + @".rows({ filter: 'applied' }).select();
                                                    " + table.NomeTabela + @"GetSelecionados();
                                                }
                                            },
                                            {
                                                text: '<i class=""fa fa-square-o""></i>',
                                                titleAttr: 'Desselecionar',
                                                action: function () {
                                                    obj" + table.NomeTabela + @".rows({ filter: 'applied' }).deselect();
                                                    " + table.NomeTabela + @"GetSelecionados();
                                                }
                                            },
                                            {
                                                text: '<i class=""fa fa-retweet""></i>',
                                                titleAttr: 'Inverter Seleção',
                                                action: function (e, dt, button, config) {
                                                    " + table.NomeTabela + @"InverteSelecao();
                                                    " + table.NomeTabela + @"GetSelecionados();
                                                }
                                            }, ";
            }
            js += @"{
                            text: '<i class=""fa fa-recycle"" aria-hidden=""true""></i>',
                            titleAttr: 'Volta tabela ao estado original',
                            action: function () {
                                obj" + table.NomeTabela + @".search('').columns().search('').draw();
                                obj" + table.NomeTabela + @".rows().deselect();
                                " + table.NomeTabela + @"GetSelecionados();
                                obj" + table.NomeTabela + @".state.clear();
                                window.location.reload();
                            }
                        }
            ],
            ajax: {
                url: '/" + table.ControllerDados + @"/" + table.AcaoDados + @"',
                dataSrc: 'data',
                cache: false
            },
            'columns': [ ";
            foreach (ItemsForTable item in table.Columns)
            {
                js += "{ 'data': '" + item.PropertyName + "' },";
            }

            js += @" ],
            columnDefs: [ ";
            for (int i = 0; i < table.Columns.Count(); i++)
            {
                string dataFormat = "";
                if (table.Columns[i].ColumnType == ColumnType.data.ToString() || table.Columns[i].ColumnType == ColumnType.datatime.ToString())
                {
                    dataFormat = " , render:function(data){ return moment(data).format('DD/MM/YYYY" + (table.Columns[i].ColumnType == ColumnType.datatime.ToString() ? " HH:mm" : "") + "');}";
                }
                string columnDefs = @"{ 'targets': [" + i + "], 'visible': " + (table.Columns[i].Visible == true ? "true" : "false") + (String.IsNullOrEmpty(table.Columns[i].ColumnType) ? " " : ", 'type': '" + table.Columns[i].ColumnType + "'") + dataFormat + " },";
                js += columnDefs;
            }
            js += @"],
            order: [ ";
            for (int i = 0; i < table.Columns.Count(); i++)
            {
                if (table.Columns[i].Ordenacao != OrderType.none)
                {
                    js += "[ " + i + ", \"" + table.Columns[i].Ordenacao + "\"],";
                }
            }
            js += @"],
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
                        0: '(Click em um registro para selecioná-lo)',
                        1: '(Selecionado 1 registro)'
                    }
                }
            },
            lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'Todos']],";
            if (!String.IsNullOrEmpty(rowCallback))
            {
                js += @" rowCallback: " + rowCallback +","; 
            }

            js += @"
            initComplete: function (oSettings, json) {
                //Se passou os registros que devem ficar selecionados, sobrescreve o cache
                if ("+ table.NomeTabela + "Selecionados != null && "+ table.NomeTabela + @"Selecionados != undefined ) {
                    " + table.NomeTabela + @"selected = "+ table.NomeTabela + @"Selecionados.split(',');
                    sessionStorage.setItem(nomeTabela+'Selecionados_' + window.location.pathname, JSON.stringify(" + table.NomeTabela + @"selected.join()));
                }

                //Recupera Selecionados.
                var selecionadosCache = JSON.parse(sessionStorage.getItem(nomeTabela+'Selecionados_' + window.location.pathname));
                if (selecionadosCache != null && selecionadosCache != undefined && selecionadosCache != '') {
                    selecionadosCache.replace(/^, /, '');
                    " + table.NomeTabela + @"selected = selecionadosCache.split(',');
                }
                else
                {
                " + table.NomeTabela + @"selected = [];
                }
                //

                //Seta selecionados na tabela
                if (" + table.NomeTabela + @"selected != null &&" + table.NomeTabela + @"selected.length > 0) {
                    obj" + table.NomeTabela + @".rows(['#' + " + table.NomeTabela + @"selected.join(', #')]).select();
                }
                //

                //Adicionar botão Filtrar selecionados
                var textBtnSelecao = '<i class=""fa fa-filter"" aria-hidden=""true""></i> Selecionados <i class=""fa fa-square-o""></i>';
                var colsSearsh = oSettings.aoPreSearchCols;
                var valorPesquisa = colsSearsh[0].sSearch;
                if (valorPesquisa.length > 0) {
                    textBtnSelecao = '<i class=""fa fa-filter"" aria-hidden=""true""></i> Selecionados <i class=""fa fa-check-square-o""></i>';
                }
                obj" + table.NomeTabela + @".button().add(obj" + table.NomeTabela + @".button().length, {
                    text: textBtnSelecao,
                    titleAttr: 'Filtrar ou Retirar filtro de selecionados',
                    action: function () {
                        if (this.text() == '<i class=""fa fa-filter"" aria-hidden=""true""></i> Selecionados <i class=""fa fa-square-o""></i>') {
                            if (" + table.NomeTabela + @"selected.length > 0) {
                                obj" + table.NomeTabela + @".columns(0).search('^(' + " + table.NomeTabela + @"selected.join('|') + ')$', true, false).draw();
                            }
                            this.text('<i class=""fa fa-filter"" aria-hidden=""true""></i> Selecionados <i class=""fa fa-check-square-o""></i>');
                        }
                        else {
                            this.text('<i class=""fa fa-filter"" aria-hidden=""true""></i> Selecionados <i class=""fa fa-square-o""></i>');
                            obj" + table.NomeTabela + @".columns(0).search('').draw();
                        }

                    }
                });
                //

                //Adiciona funcionalidades no campo de pesquisa
                this.api().columns().every(function (index) {
                    var coluna = this;
                    var nomeColuna = $(coluna.header()).html();
                    nomeColuna = nomeColuna.replace(/\//g, '').replace(/\./g, '').replace(/\ /g, '').replace('(','').replace(')',''); 
                    var campoPesquisa = $(nomeTabela + 'psq' + nomeColuna);
                    //Recupera ultima pesquisa sava e adiciona valor no campo de pesquisa
                    var colsSearsh = oSettings.aoPreSearchCols;
                    var valorPesquisa = colsSearsh[index].sSearch;
                    if (valorPesquisa.length <= 0) {
                        valorPesquisa = '';
                    }
                    else {
                        var _re_escape_regex = new RegExp('(\\' + ['/', '.', '*', '+', '?', '|', '(', ')', '[', ']', '{', '}', '\\', '$', '^', '-'].join('|\\') + ')', 'g');
                        valorPesquisa = valorPesquisa.replace(_re_escape_regex, function myFunction(x) { return x.replace('\\', ''); });
                    }
                    if (valorPesquisa == '^s*$') {
                        campoPesquisa.val(' Sem valor');
                    } else
                    {
                        campoPesquisa.val(valorPesquisa);
                    }
                    //

                    // Funcionalidade da pesquisa
                    var filter = obj" + table.NomeTabela + @".state().columns[index].search;
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
                                if (this.value == ' Sem valor') {
                                    coluna.search('^\s*$', true,false).draw();
                                } else {
                                    coluna.search(val, true, false).draw();
                                }
                            }
                            return false;
                        }
                    });
                    //

                    // Se o tipo do campo for lista add lista
                    if (campoPesquisa.attr('tipoCampo') == 'select') {
                        campoPesquisa.attr('list', nomeTabela.replace('#', '') + 'lst' + nomeColuna);

                        var lista = $('<datalist id=""'+nomeTabela.replace('#', '') +'lst' + nomeColuna + '""></datalist>')

                        this.data().unique().sort().each(function (d, j) {
                            if (d == null) {
                                d = ' Sem valor'
                            }

                            if (valorPesquisa === d) {
                                lista.append('<option value=""' + d + '"" selected=""selected"">' + d + '</option>')
                            } else {
                                lista.append('<option value=""' + d + '"">' + d + '</option>')
                            }

                            });

                        campoPesquisa.append(lista);
                    }
                    //
                });
                //
                ";
            if (!String.IsNullOrEmpty(callBackLoad))
            {
                js += callBackLoad + "();";
            }
            
            js += @"
            }
        });";

            if (table.MultipleSelect)
            {
                js += @"
                // Scripts para seleção e salvar selecionados na sessão
                $(nomeTabela+' tbody').on('click', 'tr', function () {
            
                    var id = this.id;
                    var index = $.inArray(id, " + table.NomeTabela + @"selected);

                    if (index === -1) {
                        " + table.NomeTabela + @"selected.push(id);
                    } else {
                        " + table.NomeTabela + @"selected.splice(index, 1);
                    }
                    sessionStorage.setItem(nomeTabela+'Selecionados_' + window.location.pathname, JSON.stringify(" + table.NomeTabela + @"selected.join()));
                });
                ";
            }
            else
            {
            js += @"        
                    $(nomeTabela + ' tbody').on('click', 'tr', function () { 
                        " + table.NomeTabela + @"selected = [];
                        if ( $(this).hasClass('selected') ) {
                            $(this).removeClass('selected');
                        }
                        else
                        {
                            obj" + table.NomeTabela + @".$('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                        var id = this.id;
                            " + table.NomeTabela + @"selected.push(id);
                        }
                        sessionStorage.setItem(nomeTabela+'Selecionados_' + window.location.pathname, JSON.stringify(" + table.NomeTabela + @"selected.join()));
                    });

                    $(nomeTabela + ' tbody').on('dblclick', 'tr', function () {
                        var id = this.id;
                        obj" + table.NomeTabela + @".rows(['#'+id]).select();
                        " + table.NomeTabela + @"selected = [];
                        if ( $(this).hasClass('selected') ) {
                            var id = this.id;
                            " + table.NomeTabela + @"selected.push(id);
                        }
                        sessionStorage.setItem(nomeTabela+'Selecionados_' + window.location.pathname, JSON.stringify(" + table.NomeTabela + @"selected.join()));
                    }); 
            ";
            }

        js += @"
        function " + table.NomeTabela + @"GetSelecionados() {
            " + table.NomeTabela + @"selected = obj" + table.NomeTabela + @".rows({ selected: true }).ids().toArray();
            sessionStorage.setItem(nomeTabela+'Selecionados_' + window.location.pathname, JSON.stringify((" + table.NomeTabela + @"selected.join())));
        }

        function " + table.NomeTabela + @"InverteSelecao() {
            var rowsSelected = obj" + table.NomeTabela + @".rows({ filter: 'applied', selected: true });
            var rowsDeselect = obj" + table.NomeTabela + @".rows({ filter: 'applied', selected: false });
            rowsSelected.deselect();
            rowsDeselect.select();

            " + table.NomeTabela + @"GetSelecionados();
        }
        //
    });
";
            js += @"function " + table.NomeTabela + @"GetSelecionado() {
           return " + table.NomeTabela + @"selected.join(',');
        }

        function RemoveSelecionados(tabela) {
            tabela.rows({ selected: true }).remove().draw(false);
            var idTabela = $(tabela).context[0].sTableId;
            var selecionadosRestante = tabela.rows({ selected: true }).ids().toArray();
            sessionStorage.setItem(idTabela + 'Selecionados_' + window.location.pathname, JSON.stringify((selecionadosRestante.join())));
        }
</script>";
            #endregion
            return js;
        }

        private static string GerarStrTabelaHTML(TableHTML table)
        {
            string html = "";
            if (table != null && !String.IsNullOrEmpty(table.NomeTabela))
            {
                html = @"
                        <div class=""col-md-12"">
                            <div class=""panel panel-default"" id=""pnl" + table.NomeTabela + @""" style='margin-bottom: 5px'>
                                <div class=""panel-body"">
                        <table id=""" + table.NomeTabela + @""" class=""table table-striped table-bordered table-hover table-condensed nowrap"" cellspacing=""0"" width=""100%"">
                            <thead>
                                <tr>";
                foreach (var item in table.Columns)
                {
                    html += @"<th>" + @item.Description + "</th>";
                }
                html += @"</tr>
                                <tr>";
                foreach (var item in table.Columns)
                {
                    html += @"<th class=""" + @item.Search + @""">" + @item.Description + @"</th>";
                }
                html += @"</tr>
                            </thead>
                        </table> </div> </div> </div>";
            }
            return html;
        }


        public static MvcHtmlString TextBoxForDateTime<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            MvcHtmlString label = LabelExtensions.LabelFor(helper, expression, new { @class = "control-label label-sm" });
            MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MaskDateTimeHelper" } });
            //MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MascDateTime", data_val = "false" } });
            MvcHtmlString validation = ValidationExtensions.ValidationMessageFor(helper, expression, null, new { @class = "text-danger" });

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var propName = metadata.PropertyName;

            StringBuilder html = new StringBuilder();
            html.Append(label);
            html.Append("<div class=\"form-group input-group-sm\">");
            html.Append("<div class='input-group input-group-sm date' id='" + propName + "'>");
            html.Append(editor);
            html.Append("<span class=\"input-group-addon\">");
            html.Append("<span class=\"glyphicon glyphicon-calendar\"></span>");
            html.Append("</span>");
            html.Append("</div>");
            html.Append("</div>");
            string script = @"<script type='text/javascript'>
                                $(function () {
                                    $('#"+propName+ @"').datetimepicker({
                                        locale: 'pt-BR',
                                        format : 'DD/MM/YYYY HH:mm',
                                        showTodayButton: true
                                    });
                                    $('.MaskDateTimeHelper').mask('00/00/0000 00:00', { 'translation': { 0: { pattern: /[0-9*]/ } } });
                                });
                            </script>";
            html.Append(script);
            html.Append(validation);
            string comp = html.ToString();
            return MvcHtmlString.Create(comp);
        }

        public static MvcHtmlString TextBoxForDate<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            MvcHtmlString label = LabelExtensions.LabelFor(helper, expression, new { @class = "control-label label-sm" });
            MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MaskDateHelper" } });
            //MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MascDateTime", data_val = "false" } });
            MvcHtmlString validation = ValidationExtensions.ValidationMessageFor(helper, expression, null, new { @class = "text-danger" });

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var propName = metadata.PropertyName;

            StringBuilder html = new StringBuilder();
            html.Append(label);
            html.Append("<div class=\"form-group input-group-sm\">");
            html.Append("<div class='input-group input-group-sm date' id='" + propName + "'>");
            html.Append(editor);
            html.Append("<span class=\"input-group-addon\">");
            html.Append("<span class=\"glyphicon glyphicon-calendar\"></span>");
            html.Append("</span>");
            html.Append("</div>");
            html.Append("</div>");
            string script = @"<script type='text/javascript'>
                                $(function () {
                                    $('#" + propName + @"').datetimepicker({
                                        locale: 'pt-BR',
                                        format : 'DD/MM/YYYY',
                                        showTodayButton: true
                                    });
                                    $('.MaskDateHelper').mask('00/00/0000', { 'translation': { 0: { pattern: /[0-9*]/ } } });
                                });
                            </script>";
            html.Append(script);
            html.Append(validation);
            string comp = html.ToString();
            return MvcHtmlString.Create(comp);
        }

        public static MvcHtmlString TextBoxForTime<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            MvcHtmlString label = LabelExtensions.LabelFor(helper, expression, new { @class = "control-label label-sm" });
            MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MaskTimeHelper" } });
            //MvcHtmlString editor = EditorExtensions.EditorFor(helper, expression, new { htmlAttributes = new { @class = "form-control input-sm MascDateTime", data_val = "false" } });
            MvcHtmlString validation = ValidationExtensions.ValidationMessageFor(helper, expression, null, new { @class = "text-danger" });

            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var propName = metadata.PropertyName;

            StringBuilder html = new StringBuilder();
            html.Append(label);
            html.Append("<div class=\"form-group input-group-sm\">");
            html.Append("<div class='input-group input-group-sm date' id='" + propName + "'>");
            html.Append(editor);
            html.Append("<span class=\"input-group-addon\">");
            html.Append("<span class=\"glyphicon glyphicon-time\"></span>");
            html.Append("</span>");
            html.Append("</div>");
            html.Append("</div>");
            string script = @"<script type='text/javascript'>
                                $(function () {
                                    $('#" + propName + @"').datetimepicker({
                                        locale: 'pt-BR',
                                        format : 'HH:mm'
                                    });
                                    $('.MaskTimeHelper').mask('00:00', { 'translation': { 0: { pattern: /[0-9*]/ } } });
                                });
                            </script>";
            html.Append(script);
            html.Append(validation);
            string comp = html.ToString();
            return MvcHtmlString.Create(comp);
        }
    }
}