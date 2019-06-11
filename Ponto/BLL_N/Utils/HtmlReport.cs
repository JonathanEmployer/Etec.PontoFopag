using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BLL_N.IntegracaoTerceiro
{
    public class HtmlReport
    {
        public string nomeRel { get; set; }
        public float HorizontalMargin { get; set; }
        public float VerticalMargin { get; set; }
        public Rectangle pagina { get; set; }

        public HtmlReport()
            : this ("", 15.0f, 15.0f)
        {
        }

        public HtmlReport(string nomeRel)
            : this(nomeRel, 15.0f, 15.0f)
        {
        }

        public HtmlReport(string nomeRel, float horizontalMargin, float verticalMargin)
        {
            this.nomeRel = nomeRel;
            this.HorizontalMargin = horizontalMargin;
            this.VerticalMargin = verticalMargin;
            this.pagina = PageSize.A4;
        }

        public byte[] RenderPDF(string htmlText, bool CabecaoRodape, bool exibeEmissaoPagina)
        {
            byte[] renderedBuffer;

            try
            {
                using (MemoryStream outputMemoryStream = new MemoryStream())
                {
                    using (Document pdfDocument = new Document(pagina, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                    {
                        PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                        pdfWriter.CloseStream = false;
                        if (CabecaoRodape)
                        {
                            PageEventHelper pageEventHelper = new PageEventHelper(nomeRel, exibeEmissaoPagina, false);
                            pdfWriter.PageEvent = pageEventHelper;
                        }
                        pdfDocument.Open();
                        using (StringReader htmlViewReader = new StringReader(htmlText))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, pdfDocument, htmlViewReader);
                        }
                    }

                    renderedBuffer = new byte[outputMemoryStream.Position];
                    outputMemoryStream.Position = 0;
                    outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "The document has no pages.")
	            {
                    throw new Exception("Não há paginas para imprimir.");
	            }
                else
                {
                    throw ex;
                }
            }

            return renderedBuffer;
        }

        public string Render(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public String RenderViewToString(ControllerContext context, String viewPath, object model = null)
        {
            try
            {
                context.Controller.ViewData.Model = model;
                using (var sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindView(context, viewPath, null);
                    var viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(context, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            { 
                
                throw ex;
            }
            
        }

        public byte[] MergeFiles(List<byte[]> sourceFiles, bool CabecaoRodape, bool exibeEmissaoPagina)
        {
            Document document = new Document(pagina, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin);
            MemoryStream output = new MemoryStream();

            try
            {
                // Initialize pdf writer
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                if (CabecaoRodape)
                {
                    PageEventHelper pageEventHelper = new PageEventHelper(nomeRel, exibeEmissaoPagina, true);
                    writer.PageEvent = pageEventHelper;
                }

                // Open document to write
                document.Open();
                PdfContentByte content = writer.DirectContent;

                // Iterate through all pdf documents
                for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                {
                    // Create pdf reader
                    PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;

                    // Iterate through all pages
                    for (int currentPageIndex = 1; currentPageIndex <=
                                        numberOfPages; currentPageIndex++)
                    {
                        // Determine page size for the current page
                        document.SetPageSize(
                            reader.GetPageSizeWithRotation(currentPageIndex));

                        // Create page
                        document.NewPage();
                        PdfImportedPage importedPage =
                            writer.GetImportedPage(reader, currentPageIndex);


                        // Determine page orientation
                        int pageOrientation = reader.GetPageRotation(currentPageIndex);
                        if ((pageOrientation == 90) || (pageOrientation == 270))
                        {
                            content.AddTemplate(importedPage, 0, -1f, 1f, 0, 0,
                                reader.GetPageSizeWithRotation(currentPageIndex).Height);
                        }
                        else
                        {
                            content.AddTemplate(importedPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception("There has an unexpected exception" +
                        " occured during the pdf merging process.", exception);
            }
            finally
            {
                try
                {
                    document.Close();
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("The document has no pages."))
                    {
                        throw new Exception("Não há dados para gerar o relatório");
                    }
                    throw e;
                }
                
            }
            return output.GetBuffer();
        }

        public class FakeView : IView
        {
            #region IView Members

            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }

    public class PageEventHelper : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate headerTemplate;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        private DateTime dataImpressao = DateTime.Now;

        private string nomeRel = "";
        private bool exibeEmissaoPagina = false;
        private bool merge = false;
        public PageEventHelper(string nomeRel, bool exibeEmissaoPagina, bool merge)
        {
            this.nomeRel = nomeRel;
            this.exibeEmissaoPagina = exibeEmissaoPagina;
            this.merge = merge;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                dataImpressao = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                headerTemplate = cb.CreateTemplate(100, 100);
            }
            catch (DocumentException)
            {
                //handle exception here
            }
            catch (System.IO.IOException ioe)
            {
                //handle exception here
            }
        }

        public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);

            String text = "";
            if (exibeEmissaoPagina)
            {
                text = "Data emissão: " + dataImpressao.ToString("dd/MM/yyyy HH:mm") + "   -   Página " + writer.PageNumber + " de ";
            }

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.PageSize.GetRight(220), document.PageSize.GetTop(20));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 8);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(220) + len, document.PageSize.GetTop(20));

                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, nomeRel, document.LeftMargin, document.PageSize.GetTop(20), 0);
                cb.EndText();
            }
           
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            headerTemplate.BeginText();
            headerTemplate.SetFontAndSize(bf, 8);
            headerTemplate.SetTextMatrix(0, 0);

            if (exibeEmissaoPagina)
            {
                headerTemplate.ShowText((writer.PageNumber).ToString());
            }
            headerTemplate.EndText();
        }
    }
}