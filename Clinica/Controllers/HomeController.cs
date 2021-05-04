using Clinica.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Clinica.Models;

namespace Clinica.Controllers
{
    public class HomeController : Controller
    {
        
        /*public async Task<ActionResult> Index()
        {
            List<Medicamento> medicamentos = new List<Medicamento>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3000");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage resp = await client.GetAsync("/");
                    if (resp.IsSuccessStatusCode)
                    {
                        var medResponde = resp.Content.ReadAsStringAsync().Result;
                        medicamentos = JsonConvert.DeserializeObject<List<Medicamento>>(medResponde);
                        System.Diagnostics.Debug.WriteLine("Exito");
                        foreach (var item in medicamentos)
                        {
                            System.Diagnostics.Debug.WriteLine(item);
                        }
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
            

            return View();
        }*/
        
        public ActionResult Index()
        {
            Doctores doctor = (Doctores)Session["Doctor"];
            if (doctor == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            ViewBag.estadoMenu = "mm-active";
            return View();
        }

    }
}