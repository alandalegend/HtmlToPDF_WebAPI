using PdfSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace HtmlToPDF.Controllers
{
    /// <summary>
    /// Controllador que permite convertir tu HTML a un PDF 
    /// </summary>
    public class HtmlToPDFController : ApiController
    {

        /// <summary>
        /// [POST] Método que generará tu HTML a un PDF, disfrútalo
        /// </summary>
        /// <param name="model">
        /// model.contenidohtml, aquí pones tu HTML, y eso es lo que convertirá a un PDF
        /// </param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]

        public IHttpActionResult GenerarPDF([FromBody] parametros model)
        {

            var arr = makePDF(model.contenidohtml);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(arr);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "test.pdf"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = ResponseMessage(result);

            return response;

        }

        /// <summary>
        /// Convierte el html a un arreglo de bytes, el cual es convertido por la dll de PdfSharp <3
        /// </summary>
        /// <param name="html">es tu html en sí</param>
        /// <returns></returns>
        public byte[] makePDF(string html)
        {

            //Te dejo un ejemplo de un "Diploma" para ser impreso, ya tu podrás poner algo en su lugar
            var example_html = @"   
      
   
      


<center>
    <style>
        .signature,
        .title {
            float: left;
            border-top: 1px solid #000;
            width: 200px;
            text-align: center;
        }
    </style>
    <div style='width:100%; height:100%; padding:20px; text-align:center; border: 10px solid #787878'>
        <div style='width:100%; height:100%; padding:20px; text-align:center; border: 5px solid #787878'>
            <span style='font-size:50px; font-weight:bold'>Certificate of Completion</span><br /><br />
                <span style='font-size:25px'><i>This is to certify that</i></span><br /><br />
                    <span style='font-size:30px'><b>$student.getFullName()</b></span><br /><br /><span
                            style='font-size:25px'><i>hascompleted the course</i></span> <br /><br /> <span
                            style='font-size:30px'>$course.getName()</span><br /><br /><span style='font-size:20px'>with
                            score of <b>$grade.getPoints()%</b></span> <br /><br /><br /><br />
                        <span style='font-size:25px'><i>Completed Date</i></span><br />
                            <span style='font-size:25px'><i>01-Sep-2018</i></span><br />
                                <table style='margin-top:40px;float:left'>
                                    <tr>
                                        <td><span><b>$student.getFullName()</b></span></td>
                                    </tr>
                                    <tr>
                                        <td style='width:200px;float:left;border:0;border-bottom:1px solid #000;'></td>
                                    </tr>
                                    <tr>
                                        <td style='text-align:center'><span><b>Signature</b></span></td>
                                    </tr>
                                </table>
                                <table style='margin-top:40px;float:right'>
                                    <tr>
                                        <td><span><b>$student.getFullName()</b></span></td>
                                    </tr>
                                    <tr>
                                        <td style='width:200px;float:right;border:0;border-bottom:1px solid #000;'></td>
                                    </tr>
                                    <tr>
                                        <td style='text-align:center'><span><b>Signature</b></span></td>
                                    </tr>
                                </table>
        </div>
    </div>
</center>
    
    ";

            example_html = !String.IsNullOrEmpty(html) ? html : example_html;

            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var config = new PdfGenerateConfig();
                //Pongo la hoja en horizontal
                config.PageOrientation = PageOrientation.Landscape;
                //tamaño carta
                config.PageSize = PdfSharp.PageSize.A4;
                //que esa dll haga la magia
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(example_html, config);

                pdf.Save(ms);
                res = ms.ToArray();

            }

            return res;



        }
    }
    /// <summary>
    /// Clase que solo tiene una propiedad, ahí pones tu html
    /// </summary>
    public class parametros
    {
        [AllowHtml]
        /// <summary>
        /// Agrega el contenido HTML para ser convertido a PDF
        /// </summary>
        public string contenidohtml { get; set; }

    }
}
