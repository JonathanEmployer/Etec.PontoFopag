using System.Web;
using System.Web.Optimization;

namespace PontoWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.signalR-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/apputil").Include(
                      "~/Scripts/apputil.js"));

            bundles.Add(new ScriptBundle("~/bundles/homejs").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/globalize.js",
                      "~/Scripts/methods_pt.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/bootstrap-dialog.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-datepicker.pt-BR.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/jquery.mask.js",
                      "~/Scripts/cwkMascaras.js",
                      "~/Scripts/cwkFuncoesAuxiliares.js",
                      "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                      "~/Scripts/jNotify.jquery.min.js",
                      "~/Scripts/json2.js",
                      "~/Scripts/customvalidations.js",
                      "~/Scripts/pnotify.custom.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/cwkbootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/gauge-bar.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/homecss").Include(
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-dialog.css",
                      "~/Content/body.css",
                      "~/Content/dropdown.css",
                      "~/Content/Modal.css",
                      "~/Content/ImageArea.css",
                      "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                      "~/Content/jNotify.jquery.css",
                      "~/Content/pnotify.custom.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/loginjs").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery-ui.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/json2.js",
                      "~/Scripts/jstorage.js"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/styleLogin.css"));


            bundles.Add(new StyleBundle("~/Content/RegistraPontoCSS").Include(
            "~/Content/bootstrap.css",
            "~/Content/jquery-ui.css",
            "~/Content/font-awesome.css",
            "~/Content/bootstrap-datepicker3.css",
            "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
            "~/Content/jNotify.jquery.css",
            "~/Content/cwkbootstrap.css",
            "~/Content/site.css",
            "~/Content/styleLogin.css"));

            bundles.Add(new ScriptBundle("~/bundles/paginajs").Include(
                      "~/Scripts/cwkMetodosAjax.js",
                      "~/Scripts/cwkMetodosBotoesChamadas.js",
                      "~/Scripts/cwkMetodosTela.js",
                      "~/Scripts/jquery.blockUI.js",
                      "~/Scripts/simple-gauge-bar.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/paginacss").Include(

                      ));

            bundles.Add(new ScriptBundle("~/bundles/dataTablejs").Include(
                        "~/Scripts/DataTables/datatables.js",
                        "~/Scripts/DataTables/cwkMetodosDataTables.js",
                        "~/Scripts/DataTables/datetime-moment.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/dataTablecss").Include(
                      "~/Scripts/DataTables/datatables.min.css"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/uploadImagejs").Include(
            "~/Scripts/jquery.imgareaselect.js",
            "~/Scripts/jquery.fancyupload.js"));

            bundles.Add(new StyleBundle("~/Content/uploadimagecss").Include(
                      "~/Content/ImageArea.css"));

            bundles.Add(new ScriptBundle("~/bundles/RegistraPontojs").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/globalize.js",
                      "~/Scripts/methods_pt.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/moment-with-locales.min.js",
                      "~/Scripts/bootbox.min.js",
                      "~/Scripts/bootstrap-dialog.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-datepicker.pt-BR.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/jquery.mask.js",
                      "~/Scripts/cwkMascaras.js",
                      "~/Scripts/cwkFuncoesAuxiliares.js",
                      "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                      "~/Scripts/jNotify.jquery.min.js",
                      "~/Scripts/json2.js",
                      "~/Scripts/customvalidations.js",
                      "~/Scripts/cwkMetodosBotoesChamadas.js",
                      "~/Scripts/cwkMetodosTela.js",
                      "~/Scripts/html2canvas.js",
                      "~/Scripts/canvas2image.js",
                      "~/Scripts/FileSaver.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/partialjs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/methods_pt.js",
                "~/Scripts/globalize.js",
                "~/Scripts/globalize.culture.pt-BR.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-datepicker.pt-BR.js",
                "~/Scripts/jquery.mask.js",
                "~/Scripts/cwkMascaras.js",
                "~/Scripts/cwkMetodosBotoesChamadas.js"
            ));

            bundles.Add(new StyleBundle("~/Content/partialcss").Include(
                "~/Content/bootstrap.css",
                "~/Content/jquery-ui.css",
                "~/Content/cwkbootstrap.css",
                "~/Content/jquery.dataTables.css",
                "~/Content/dataTables.jqueryui.css",
                "~/Content/dataTables.fixedColumns.min.css",
                "~/Content/TableTools_JUI.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/nestedformjs").Include(
                "~/Scripts/cwkNestedForm.js"
            ));
        }
    }
}
