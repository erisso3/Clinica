using Clinica.DAO;
using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

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


        [HttpPost]
        public JsonResult LoginUsuario(string usuario, string password)
        {
            Usuarios usuarios = dao.getUsuario(usuario,password);
            if (usuarios!=null)
            {
                return Json(new { usuarios }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }

        //Movil
        // POST: Login
        [HttpPost]
        public JsonResult LoginMovil(string usuario, string password)
        {
            bool result = false;
            Pacientes paciente = dao.getPaciente(usuario, password);
            if (paciente !=null)
            {
                result = true;
                return Json(new {paciente,result}, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistroMovil([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password")] Pacientes paciente)
        {
            bool result = false;
            if (paciente != null)
            {
                result = daoPacientes.agregar(paciente);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditarMovil([Bind(Include = "id_paciente,nombre,ape_pat,ape_mat,usuario,password")] Pacientes paciente)
        {
            bool result = false;
            if (paciente != null)
            {
                result = daoPacientes.editar(paciente);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut()
        {
            Session["Usuario"] = null;
            Session.Abandon();
            return RedirectToAction("", "Login");
        }

        public static string EncodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }
    }
}