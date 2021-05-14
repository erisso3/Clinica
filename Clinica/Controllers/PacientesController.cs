using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class PacientesController : Controller
    {
        private DAOPacientes dao = new DAOPacientes();
        public JsonResult ListarPacientes()
        {
            List<Pacientes> pacientes = dao.getPacientes();
            return Json(new { pacientes }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Registrar([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password")] Pacientes paciente)
        {
            string password=EncodePassword(paciente.password);
            paciente.password = password;
            bool result = dao.agregar(paciente);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            bool result = dao.eliminar(id);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
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