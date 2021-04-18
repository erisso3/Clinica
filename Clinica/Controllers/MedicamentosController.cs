using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class MedicamentosController : Controller
    {
        // GET: Medicamentos
        public ActionResult Index()
        {
            ViewBag.estadoMedicamentos = "mm-active";
            return View();
        }
    }
}