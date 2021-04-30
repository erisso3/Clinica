using Clinica.DAO;
using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : Controller
    {
        public DAOLogin dao = new DAOLogin();
        public DAOPacientes daoPacientes = new DAOPacientes();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public JsonResult LoginMovil(string usuario, string password)
        {
            Pacientes paciente = dao.getPaciente(usuario, password);
            if (paciente !=null)
            {
                return Json(new {paciente}, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistroMovil([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password")] Pacientes paciente)
        {

            if (paciente != null)
            {
                bool result = daoPacientes.agregar(paciente);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
    }
}