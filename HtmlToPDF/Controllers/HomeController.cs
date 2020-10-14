using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace HtmlToPDF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConvertirHTMLToPDF()
        {
            var modelo = new parametros();
            return View(modelo);
        }

        [HttpPost]
        public FileStreamResult ConvertirHTMLToPDF(parametros modelo)
        {
            var arr = new HtmlToPDFController().makePDF(modelo.contenidohtml);
            System.Net.Http.HttpResponseMessage result = new System.Net.Http.HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new System.Net.Http.ByteArrayContent(arr);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "test.pdf"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            MemoryStream m = new MemoryStream(arr);
            m.Position = 0;

            return File(m, "application/pdf");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}