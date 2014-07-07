using System;
using System.IO;
using System.Web.Mvc;
using EvoPdf;

namespace EvoHtmlToPdfAzureTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BasicTest()
        {
            var html = "<h1>Basic HTML String to PDF Test</h2>";
            var htmlToPdfConverter = new HtmlToPdfConverter();
            var outPdf = htmlToPdfConverter.ConvertHtml(html, null);
            var fileResult = new FileContentResult(outPdf, "application/pdf");
            fileResult.FileDownloadName = "BasicTest.pdf";

            return fileResult;
        }

        public ActionResult MvcViewTest()
        {
            var stringWriter = new StringWriter();
            var baseUrl = string.Format("{0}://{1}{2}/", Request.Url.Scheme, Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'));
            var viewResult = ViewEngines.Engines.FindView(ControllerContext, "MvcViewTest", null);
            var viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                ControllerContext.Controller.ViewData,
                ControllerContext.Controller.TempData,
                stringWriter);
            
            viewResult.View.Render(viewContext, stringWriter);

            var htmlToConvert = stringWriter.ToString();
            var htmlToPdfConverter = new HtmlToPdfConverter();
            byte[] outPdfBuffer = htmlToPdfConverter.ConvertHtml(htmlToConvert, baseUrl);

            FileResult fileResult = new FileContentResult(outPdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "MvcViewTest.pdf";

            return fileResult;
        }
    }
}