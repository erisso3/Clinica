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
            ViewBag.estadoCitas= "mm-active";
            return View();
        }
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