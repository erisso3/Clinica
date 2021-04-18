using Clinica.Objects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class RecetasController : Controller
    {
        // GET: Recetas
        public ActionResult Index()
        {
            //crearReceta();
            List<Receta> recetas = new List<Receta>();
            recetas.Add(new Receta("Sintomas de tos seca", @"C:\Users\victo\Documents\8VO_SEMESTRE\SISTEMAS DISTRIBUIDOS\VisualStudio\Clinica\RecetasFiles\prueba.pdf", DateTime.Now));
            recetas.Add(new Receta("Sintomas de tos", @"C:\Users\victo\Documents\8VO_SEMESTRE\SISTEMAS DISTRIBUIDOS\VisualStudio\Clinica\RecetasFiles\prueba.pdf", DateTime.Now));
            recetas.Add(new Receta("Sintomas de gripa", @"C:\Users\victo\Documents\8VO_SEMESTRE\SISTEMAS DISTRIBUIDOS\VisualStudio\Clinica\RecetasFiles\prueba.pdf", DateTime.Now));

            return View(recetas);
        }

        public HttpResponseBase Visualizar()
        {
            System.Diagnostics.Debug.WriteLine("visualizar pdf");
            Response.Clear();
            Response.Redirect("~/RecetasFiles/prueba.pdf");

            Response.Close();
            return Response;
        }


        public void crearReceta()
        {
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            string path = Server.MapPath("~/RecetasFiles/");
            System.Diagnostics.Debug.WriteLine("path: "+path);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(path+"prueba.pdf", FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("Mi primer PDF");
            doc.AddCreator("Roberto Torres");

            // Abrimos el archivo
            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.Add(new Paragraph("Mi primer documento PDF"));
            doc.Add(Chunk.NEWLINE);

            string pathImagen = Server.MapPath("~/Imagenes/");
            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(pathImagen+"logo-inverse.png");
            //image1.ScalePercent(50f);
            float percentage = 0.0f;
            percentage = 150 / image1.Width;
            image1.ScalePercent(percentage * 50);
            image1.Alignment = Element.ALIGN_RIGHT;
            doc.Add(image1);

            doc.Add(new Paragraph("Descripcion"));
            doc.Add(new Paragraph("Parrafo 1"));
            doc.Add(Chunk.NEWLINE);
            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clApellido = new PdfPCell(new Phrase("Apellido", _standardFont));
            clApellido.BorderWidth = 0;
            clApellido.BorderWidthBottom = 0.75f;

            PdfPCell clPais = new PdfPCell(new Phrase("País", _standardFont));
            clPais.BorderWidth = 0;
            clPais.BorderWidthBottom = 0.75f;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            // Llenamos la tabla con información
            clNombre = new PdfPCell(new Phrase("Roberto", _standardFont));
            clNombre.BorderWidth = 0;

            clApellido = new PdfPCell(new Phrase("Torres", _standardFont));
            clApellido.BorderWidth = 0;

            clPais = new PdfPCell(new Phrase("Puerto Rico", _standardFont));
            clPais.BorderWidth = 0;

            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clApellido);
            tblPrueba.AddCell(clPais);

            doc.Add(tblPrueba);

            doc.Close();
            writer.Close();
        }
    }
}