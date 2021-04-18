using Clinica.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class PacientesController : Controller
    {
        // GET: Pacientes
        public ActionResult Index()
        {
            List<Paciente> pacientes = new List<Paciente>();
            pacientes.Add(new Paciente("Victor","Vasquez","Poblete","victoe680@gmail.com"));
            pacientes.Add(new Paciente("Manuel","Iturbe","Flores","mpoblete501@gmail.com"));
            pacientes.Add(new Paciente("Juan","Lopez","Castro","juan@gmail.com"));
            ViewBag.estadoPacientes = "mm-active";
            return View(pacientes);
        }
    }
}