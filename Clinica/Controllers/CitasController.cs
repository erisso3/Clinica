using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;
using iTextSharp.text.pdf.security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        // GET: Citas
        public ActionResult Index()
        {
            Doctores doctor = (Doctores)Session["Doctor"];
            if (doctor == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            ViewBag.estadoCitas= "mm-active";
            DAOCitas daoCitas = new DAOCitas();
            List <CitasDoctorObject> listaCitasPendientes=daoCitas.getCitasPendientes(doctor.id_doctor);
            TempData["CitasPendiantes"] = listaCitasPendientes;
            List<CitasDoctorObject> listaCitasAceptadas = daoCitas.getCitasAceptadas(doctor.id_doctor);
            TempData["CitasAceptadas"] = listaCitasAceptadas;
            List<CitasDoctorObject> listaCitasRechazadas = daoCitas.getCitasRechazadas(doctor.id_doctor);
            TempData["CitasRechazadas"] = listaCitasRechazadas;
            List<CitasDoctorObject> listaCitasRealizadas = daoCitas.getCitasRealizadas(doctor.id_doctor);
            TempData["CitasRealizadas"] = listaCitasRealizadas;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult actualizarStatusCita(int id_cita, int status)
        {
            Doctores doctor = (Doctores)Session["Doctor"];
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
            Doctores doctor = (Doctores)Session["Doctor"];
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


        public async Task<ActionResult> DetallesCita()
        {
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
    }
}