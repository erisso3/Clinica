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

namespace Clinica.Controllers
{
    public class HomeController : Controller
    {
        /*
        public async Task<ActionResult> Index()
        {
            List<Medicamento> medicamentos = new List<Medicamento>();
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

            return View();
        }
        */
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

        private void GetItems()
        {
            var url = $"http://localhost:3000";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            System.Diagnostics.Debug.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle error
            }
        }

        
    }

    class Medicamento
    {
        public int id { set; get; }
        public string nombre { set; get; }
        public string dosis { set; get; }
        public string precio { set; get; }

        public override string ToString()
        {
            return String.Format("Id: {0}, nombre: {1}, dosis: {2}, precio: {3}",id,nombre,dosis,precio);
        }
    }
}