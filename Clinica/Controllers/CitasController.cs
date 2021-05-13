﻿using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CitasController : Controller
    {
        private DAOCitas daoCitas = new DAOCitas();
        private DAORecetas daoRecetas = new DAORecetas();
        private DAOPacientes daoPacientes = new DAOPacientes();
        private DAOTicket daoTicket = new DAOTicket();
        // GET: Citas
        public ActionResult Index()
        {
            Usuarios usuario = (Usuarios)Session["Usuario"];
            if (usuario == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            ViewBag.estadoCitas= "mm-active";
            DAOCitas daoCitas = new DAOCitas();
            List <CitasDoctorObject> listaCitasPendientes=daoCitas.getCitasPendientes(usuario.id_usuario);
            TempData["CitasPendiantes"] = listaCitasPendientes;
            List<CitasDoctorObject> listaCitasAceptadas = daoCitas.getCitasAceptadas(usuario.id_usuario);
            TempData["CitasAceptadas"] = listaCitasAceptadas;
            List<CitasDoctorObject> listaCitasRechazadas = daoCitas.getCitasRechazadas(usuario.id_usuario);
            TempData["CitasRechazadas"] = listaCitasRechazadas;
            List<CitasDoctorObject> listaCitasRealizadas = daoCitas.getCitasRealizadas(usuario.id_usuario);
            TempData["CitasRealizadas"] = listaCitasRealizadas;
            List<Pacientes> listaPacientes = daoPacientes.listarPacientes();
            TempData["listaPacientes"] = listaPacientes;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult registrarCita(int id_paciente, DateTime fecha, int hora, String observacion)
        {
            Usuarios usuario = (Usuarios)Session["Usuario"];
            if (usuario == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            System.Diagnostics.Debug.WriteLine(fecha);
            System.Diagnostics.Debug.WriteLine(hora);
            CitaObject cita = new CitaObject();
            cita.id_doctor = usuario.id_usuario;
            cita.id_paciente = id_paciente;
            cita.status = 1;
            cita.fecha = fecha.ToString("yyyy-MM-dd");
            cita.hora = hora;
            cita.observacion = observacion;
            bool bandera = daoCitas.agregar(cita);
            if (bandera)
            {
                System.Diagnostics.Debug.WriteLine("Todo chido");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No chido");
            }
            return RedirectToAction("Index");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult actualizarStatusCita(int id_cita, int status)
        {
            Usuarios doctor = (Usuarios)Session["Usuario"];
            if (doctor == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            bool bandera = daoCitas.statusCita(id_cita,status);
            if (bandera)
            {
                System.Diagnostics.Debug.WriteLine("Todo chido");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No chido");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult eliminarCita(int id_cita)
        {
            Usuarios doctor = (Usuarios)Session["Usuario"];
            if (doctor == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            bool bandera = daoCitas.eliminarCita(id_cita);
            if (bandera)
            {
                System.Diagnostics.Debug.WriteLine("Todo chido");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No chido");
            }
            return RedirectToAction("Index");

        }


        //Movil
        [HttpPost]
        public JsonResult AgendarCita([Bind(Include = "id_paciente,id_doctor,fecha,hora,observacion")] CitaObject cita)
        {
            cita.status = 0;
            bool result = daoCitas.agregar(cita);
            if (result)
            {
                return Json("exito", JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }

        public JsonResult MisCitas(int id)
        {
            List<CitaObject> lista = daoCitas.getCitasPaciente(id);
            if (lista !=null)
            {
                foreach (var item in lista)
                {
                    item.fecha = item.fechag.ToString("dd/MM/yyyy");
                }
                return Json(new { lista }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> DetallesCita(int id_cita)
        {
            Usuarios usuario = (Usuarios)Session["Usuario"];
            if (usuario == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            CitaDetallesObject citaDetalles = daoCitas.getCitaDetalles(id_cita);
            System.Diagnostics.Debug.WriteLine("Exito: "+citaDetalles);
            Session["citaDetalles"] = citaDetalles;
            List<Medicamento> medicamentos = new List<Medicamento>();
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:3000");
                    client.BaseAddress = new Uri("https://apimedicamentos.herokuapp.com/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage resp = await client.GetAsync("/");
                    if (resp.IsSuccessStatusCode)
                    {
                        var medResponde = resp.Content.ReadAsStringAsync().Result;
                        medicamentos = JsonConvert.DeserializeObject<List<Medicamento>>(medResponde);
                        System.Diagnostics.Debug.WriteLine("Exito");

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No Exito");
                    }
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("No Exito");
            }
            return View(medicamentos);
        }

        [HttpPost]
        public async Task<ActionResult> DetallesCita(int id_cita, string observacion, string indicaciones, string [] ids_medicamentos, string paciente, string nombreCompleto, float cobro, string correo)
        {
            List<Medicamento> medicamentos = new List<Medicamento>();
            System.Diagnostics.Debug.WriteLine("id_cita: " + id_cita);
            System.Diagnostics.Debug.WriteLine("observacion: " + observacion);
            System.Diagnostics.Debug.WriteLine("indicaciones: " + indicaciones);
            System.Diagnostics.Debug.WriteLine("Cobro: " + cobro);
            ViewBag.Error = "Entre al post detalles";
            string ids = "";
            List<Medicamento> med = new List<Medicamento>();
            Medicamento aux;
            if(ids_medicamentos != null)
            {
                foreach (var item in ids_medicamentos)
                {
                    aux = JsonConvert.DeserializeObject<Medicamento>(item);
                    med.Add(aux);
                    ids = ids + "," + aux.id.ToString();
                }
            }
            ViewBag.Error = "Antes de crear ticket";
            bool ticketResult = crearTicket(paciente, nombreCompleto, cobro);
            ViewBag.Error = "Despues de crear ticket";
            ViewBag.Error = "Antes de crear receta";
            bool result = crearReceta(med, observacion, indicaciones, paciente, nombreCompleto);
            ViewBag.Error = "Despues de crear receta";
            
            if (result && ticketResult)
            {
                ViewBag.Error = "Antes de generar archivo";
                DateTime date = DateTime.Now;

                Recetas receta = new Recetas();
                Tickets ticket = new Tickets();

                ContextDb db = new ContextDb();
                Citas cita = db.Citas.ToList().Find(x=>x.id_cita== id_cita);
                receta.id_cita = id_cita;
                receta.fecha = date;
                receta.ruta = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Receta.pdf";
                receta.ids_medicamentos = ids;
                receta.observacion = observacion;
                receta.instruccion = indicaciones;
                string path = Server.MapPath("~/Files/Recetas/" + receta.ruta);
                Byte[] bytes = System.IO.File.ReadAllBytes(path);
                string file = Convert.ToBase64String(bytes);
                //System.Diagnostics.Debug.WriteLine("Base64: "+file);
                receta.documento = file;

                string pathTicket = Server.MapPath("~/Files/Recibos/" + date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Recibo.pdf");
                Byte[] bytesTicket = System.IO.File.ReadAllBytes(pathTicket);
                ticket.id_cita = id_cita;
                ticket.documento = Convert.ToBase64String(bytesTicket);
                ticket.ruta = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Recibo.pdf";
                ticket.fecha = date;
                ticket.total = cobro;
                ViewBag.Error = "Antes de agregar receta a la base de datos";
                bool add = daoRecetas.agregar(receta,cita);
                ViewBag.Error = "Antes de generar ticket a la base de datos";
                bool addTicket = daoTicket.agregar(ticket);
                if (add && addTicket)
                {
                    SendEmailReceta(correo, receta.ruta);
                    return RedirectToAction("Index");
                }
            }
            
            
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:3000");
                    client.BaseAddress = new Uri("https://apimedicamentos.herokuapp.com/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage resp = await client.GetAsync("/");
                    if (resp.IsSuccessStatusCode)
                    {
                        var medResponde = resp.Content.ReadAsStringAsync().Result;
                        medicamentos = JsonConvert.DeserializeObject<List<Medicamento>>(medResponde);
                        System.Diagnostics.Debug.WriteLine("Exito");

                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No Exito");
                    }
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("No Exito");
            }
            

            return View(medicamentos);
        }


        public bool crearReceta(List<Medicamento> medicamentos, string observacion, string indicacion, string paciente, string nombreCompleto)
        {
            try
            {
                Document doc = new Document(PageSize.LETTER);
                // Indicamos donde vamos a guardar el documento
                DateTime date = DateTime.Now;
                string path = Server.MapPath("~/Files/Recetas/") + date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Receta.pdf";
                
                string nombreArchivo = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Receta.pdf";
                
                PdfWriter writer = PdfWriter.GetInstance(doc,
                                            new FileStream(path , FileMode.Create));
                System.Diagnostics.Debug.WriteLine("wroter: ");
                // Le colocamos el título y el autor
                // **Nota: Esto no será visible en el documento
                doc.AddTitle("CLINICA MITCHELL");
                doc.AddCreator("Dr. Erick Echeverría");

                // Abrimos el archivo
                doc.Open();

                string pathImagen = Server.MapPath("~/Imagenes/");
                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(pathImagen + "logo.png");
                //image1.ScalePercent(50f);
                float percentage = 0.0f;
                percentage = 150 / image1.Width;
                image1.ScalePercent(percentage * 50);
                //image1.Alignment = Element.ALIGN_LEFT;
                //image1.Top= 1;
                image1.Left = 0;
                doc.Add(image1);

                iTextSharp.text.Image imageFondo = iTextSharp.text.Image.GetInstance(pathImagen + "logoSalleFondo.png");
                imageFondo.Alignment = iTextSharp.text.Image.UNDERLYING;
                float percentage2 = 150 / imageFondo.Width;
                imageFondo.ScalePercent(percentage2 * 190);
                imageFondo.SetAbsolutePosition(170, 370);
                doc.Add(imageFondo);


                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                // Escribimos el encabezamiento en el documento
                iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
                iTextSharp.text.Font fontSub = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);
                Paragraph medico = new Paragraph("Dr. Erick Mitchell Echeverría Celaya", fontTitle);
                Paragraph clinica = new Paragraph("Clinica \"Mitchell\"    -    Cédula profesional. 901014585", fontSub);
                clinica.Font.SetStyle(Font.UNDERLINE);
                medico.Alignment = Element.ALIGN_CENTER;
                clinica.Alignment = Element.ALIGN_CENTER;

                doc.Add(medico);
                doc.Add(clinica);


                doc.Add(Chunk.NEWLINE);
                iTextSharp.text.Font fontPaciente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph datosPaciente = new Paragraph("Paciente: " + nombreCompleto + "                             Fecha: " + date.Date.ToString("dd/MM/yyyy"), fontPaciente);
                doc.Add(datosPaciente);
                doc.Add(Chunk.NEWLINE);

                iTextSharp.text.Font fontObs = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph observ = new Paragraph(observacion, fontObs);
                doc.Add(observ);
                doc.Add(Chunk.NEWLINE);


                Paragraph indic = new Paragraph(indicacion, fontObs);
                doc.Add(indic);
                doc.Add(Chunk.NEWLINE);

                // Creamos una tabla que contendrá el nombre, apellido y país
                // de nuestros visitante.
                PdfPTable tblPrueba = new PdfPTable(3);
                tblPrueba.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell clNombre = new PdfPCell(new Phrase("Medicamento", _standardFont));
                clNombre.BorderWidth = 0;
                clNombre.FixedHeight = 17;
                clNombre.BorderWidthBottom = 0.75f;

                PdfPCell clDosis = new PdfPCell(new Phrase("Dosis", _standardFont));
                clDosis.BorderWidth = 0;
                clDosis.BorderWidthBottom = 0.75f;
                clDosis.FixedHeight = 17;

                PdfPCell clIndic = new PdfPCell(new Phrase("Indicaciones", _standardFont));
                clIndic.BorderWidth = 0;
                clIndic.BorderWidthBottom = 0.75f;
                clIndic.FixedHeight = 17;

                // Añadimos las celdas a la tabla
                tblPrueba.AddCell(clNombre);
                tblPrueba.AddCell(clDosis);
                tblPrueba.AddCell(clIndic);

                // Llenamos la tabla con información
                foreach (var item in medicamentos)
                {
                    clNombre = new PdfPCell(new Phrase(item.nombre, _standardFont));
                    clNombre.BorderWidth = 0;
                    clNombre.PaddingTop = 8f;

                    clDosis = new PdfPCell(new Phrase(item.dosis, _standardFont));
                    clDosis.BorderWidth = 0;
                    clDosis.PaddingTop = 8f;

                    clIndic = new PdfPCell(new Phrase(item.indicaciones, _standardFont));
                    clIndic.BorderWidth = 0;
                    clIndic.PaddingTop = 8f;

                    // Añadimos las celdas a la tabla
                    tblPrueba.AddCell(clNombre);
                    tblPrueba.AddCell(clDosis);
                    tblPrueba.AddCell(clIndic);

                }

                doc.Add(tblPrueba);

                doc.Close();
                writer.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw;
                //return false;
            }
        }

        public bool crearTicket(string paciente, string nombreCompleto,float cobro)
        {
            try
            {
                Document doc = new Document(PageSize.LETTER);
                // Indicamos donde vamos a guardar el documento
                string path = Server.MapPath("~/Files/Recibos/");
                DateTime date = DateTime.Now;
                string nombreArchivo = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Recibo.pdf";
                System.Diagnostics.Debug.WriteLine("file " + nombreArchivo);

                System.Diagnostics.Debug.WriteLine("path: " + path);
                Session["Error"] = "Error en generar el tickect"+path;
                PdfWriter writer = PdfWriter.GetInstance(doc,
                                            new FileStream(path + nombreArchivo, FileMode.Create));
                Session["Error"] = "Error en generar el tickect : despues de crear archivo";
                // Le colocamos el título y el autor
                // **Nota: Esto no será visible en el documento
                doc.AddTitle("CLINICA MITCHELL");
                doc.AddCreator("Dr. Erick Echeverría");

                // Abrimos el archivo
                doc.Open();

                string pathImagen = Server.MapPath("~/Imagenes/");
                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(pathImagen + "logo.png");
                //image1.ScalePercent(50f);
                float percentage = 0.0f;
                percentage = 150 / image1.Width;
                image1.ScalePercent(percentage * 50);
                //image1.Alignment = Element.ALIGN_LEFT;
                //image1.Top= 1;
                image1.Left = 0;
                doc.Add(image1);

                iTextSharp.text.Image imageFondo = iTextSharp.text.Image.GetInstance(pathImagen + "logoSalleFondo.png");
                imageFondo.Alignment = iTextSharp.text.Image.UNDERLYING;
                float percentage2 = 150 / imageFondo.Width;
                imageFondo.ScalePercent(percentage2 * 190);
                imageFondo.SetAbsolutePosition(170, 370);
                doc.Add(imageFondo);


                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                // Escribimos el encabezamiento en el documento
                iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 14, iTextSharp.text.Font.NORMAL, BaseColor.BLUE);
                iTextSharp.text.Font fontSub = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY);
                Paragraph medico = new Paragraph("Dr. Erick Mitchell Echeverría Celaya", fontTitle);
                Paragraph clinica = new Paragraph("Clinica \"Mitchell\"    -    Cédula profesional. 901014585", fontSub);
                clinica.Font.SetStyle(Font.UNDERLINE);
                medico.Alignment = Element.ALIGN_CENTER;
                clinica.Alignment = Element.ALIGN_CENTER;

                doc.Add(medico);
                doc.Add(clinica);


                doc.Add(Chunk.NEWLINE);
                iTextSharp.text.Font fontPaciente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph datosPaciente = new Paragraph("Paciente: " + nombreCompleto + "                            Fecha: " + date.Date.ToString("dd/MM/yyyy"), fontPaciente);
                doc.Add(datosPaciente);
                doc.Add(Chunk.NEWLINE);

                iTextSharp.text.Font fontDescripcion = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Paragraph descripcion = new Paragraph("Cobro de los honorarios del doctor por la cita realizada", fontDescripcion);
                doc.Add(descripcion);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);
                // Creamos una tabla que contendrá el nombre, apellido y país
                // de nuestros visitante.
                PdfPTable tblPrueba = new PdfPTable(1);
                tblPrueba.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell clNombre = new PdfPCell(new Phrase("Total", _standardFont));
                clNombre.BorderWidth = 0;
                clNombre.FixedHeight = 17;
                clNombre.BorderWidthBottom = 0.75f;

                PdfPCell clDosis = new PdfPCell(new Phrase("$"+cobro.ToString()+".00 pesos", _standardFont));
                clDosis.BorderWidth = 0;
                clDosis.FixedHeight = 25;

                // Añadimos las celdas a la tabla
                tblPrueba.AddCell(clNombre);
                tblPrueba.AddCell(clDosis);

                doc.Add(tblPrueba);
                Session["Error"] = "Error en generar el tickect : antes de acabar de escribir";
                doc.Close();
                writer.Close();

                return true;
            }
            catch (Exception ex)
            {

                throw; 
                return false;
            }
        }

        public bool SendEmailReceta(string correoDestino, string nameArchivo)
        {
            string rutaCompartida = "\\\\127.0.0.1\\Users\\Public\\";
            try
            {
                var fromAddress = new MailAddress("vpoblete001@gmail.com", "CLINICA MITCHELL");
                const string fromPassword = "ladesiempre";
                const string subject = "Receta";

                string body = "<p style='width: 60%; margin:auto; text-align:justify;font-size:22px; line-height: 35px; color: #787675; font-family: monospace; margin-top:-50px'>Se le compratió por este medio la receta de su consulta</p>";


                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                var Mensaje = new MailMessage();

                Mensaje.To.Add(new MailAddress(correoDestino));

                string path = Server.MapPath("~/Files/Recetas/" + nameArchivo);
                //System.Diagnostics.Debug.WriteLine("path file " + path);

                //System.IO.File.Copy(path, rutaCompartida + nameArchivo);

                Mensaje.Attachments.Add(new Attachment(path));
                Mensaje.From = fromAddress;
                Mensaje.Subject = subject;
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);
                Mensaje.AlternateViews.Add(alternateView);
                Mensaje.Body = body;
                Mensaje.IsBodyHtml = true;
                smtp.Send(Mensaje);


                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }
    }
}