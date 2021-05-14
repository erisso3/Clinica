using Clinica.DAO;
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
        public JsonResult ListarCitas(int id_usuario)
        {
            List<CitasDoctorObject> listaCitasPendientes = daoCitas.getCitasPendientes(id_usuario);
            List<CitasDoctorObject> listaCitasAceptadas = daoCitas.getCitasAceptadas(id_usuario);
            List<CitasDoctorObject> listaCitasRechazadas = daoCitas.getCitasRechazadas(id_usuario);
            List<CitasDoctorObject> listaCitasRealizadas = daoCitas.getCitasRealizadas(id_usuario);
            List<Pacientes> listaPacientes = daoPacientes.listarPacientes();
            return Json(new { listaCitasPendientes, listaCitasAceptadas, listaCitasRechazadas, listaCitasRealizadas, listaPacientes }, JsonRequestBehavior.AllowGet);

        }
       
        [HttpPost]
        public JsonResult AgendarCitaDoctor([Bind(Include = "id_paciente,id_doctor,fecha,hora,observacion")] CitaObject cita)
        {
            cita.status = 1;
            bool result = daoCitas.agregar(cita);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult actualizarStatusCita(int id_cita, int status)
        {
            bool result = daoCitas.statusCita(id_cita, status);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult eliminarCita(int id_cita)
        {
            bool result = daoCitas.eliminarCita(id_cita);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetallesCita(int id_cita)
        {
            CitaDetallesObject citaDetalles = daoCitas.getCitaDetalles(id_cita);
            return Json(new { citaDetalles }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DetallesCita(int id_cita, string observacion, string indicaciones, string[] ids_medicamentos, string paciente, string nombreCompleto, float cobro, string documentoReceta, string documentoTicket)
        {
            bool banderaReceta = false; 
            bool banderaTicket = false;
            bool result = false;
            List<Medicamento> med = new List<Medicamento>();
            Medicamento aux;
            string ids = "";
            if ((id_cita>0)&& (observacion != null)&&(indicaciones != null)&& (ids_medicamentos != null)&& (paciente != null)&& (nombreCompleto != null)&& (cobro>0))
            {
                foreach (var item in ids_medicamentos)
                {
                    aux = JsonConvert.DeserializeObject<Medicamento>(item);
                    med.Add(aux);
                    ids = ids + "," + aux.id.ToString();
                }
                DateTime date = DateTime.Now;
                Recetas receta = new Recetas();
                Tickets ticket = new Tickets();
                ContextDb db = new ContextDb();
                Citas cita = db.Citas.ToList().Find(x => x.id_cita == id_cita);
                receta.id_cita = id_cita;
                receta.fecha = date;
                receta.ruta = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Receta.pdf";
                receta.ids_medicamentos = ids;
                receta.observacion = observacion;
                receta.instruccion = indicaciones;
                receta.documento = documentoReceta;

                ticket.id_cita = id_cita;
                ticket.documento = documentoTicket;
                ticket.ruta = date.Date.ToString("ddMMyyyy") + "-" + paciente + "-Recibo.pdf";
                ticket.fecha = date;
                ticket.total = cobro;
                banderaReceta = daoRecetas.agregar(receta, cita);
                banderaTicket = daoTicket.agregar(ticket);
                
            }
            if (banderaReceta && banderaTicket)
            {
                result = true;
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            
        }


        /*Esta función es para movil*/
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
        /*Esta función es para movil*/
        /*Esta función es para movil*/
        public JsonResult MisCitas(int id)
        {
            List<CitaObject> lista = daoCitas.getCitasPaciente(id);
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    item.fecha = item.fechag.ToString("dd/MM/yyyy");
                }
                return Json(new { lista }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
        /*Esta función es para movil*/


    }
}