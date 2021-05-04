using Clinica.DAO;
using Clinica.Models;
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
        private DAOPacientes dao = new DAOPacientes();
        // GET: Pacientes
        public ActionResult Index()
        {
            List<Pacientes> pacientes = dao.getPacientes();
            ViewBag.estadoPacientes = "mm-active";
            return View(pacientes);
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            bool result = dao.eliminar(id);
            if (result)
            {
                TempData["resultado"] = "Paciente eliminado con éxito";
            }
            else
            {
                TempData["resultado"] = "Paciente no eliminado";
            }
            return RedirectToAction("Index");
        }
    }
}