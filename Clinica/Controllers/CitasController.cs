using Clinica.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class CitasController : Controller
    {
        // GET: Citas
        public ActionResult Index()
        {
            ViewBag.estadoCitas= "mm-active";
            return View();
        }

        /*public ActionResult DetallesCita()
        {
            return View();
        }
        */

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