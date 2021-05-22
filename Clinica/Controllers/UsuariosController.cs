using Clinica.DAO;
using Clinica.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class UsuariosController : Controller
    {
        private DAOUsuarios dao = new DAOUsuarios();
        public JsonResult ListarPacientes()
        {
            List<Usuarios> pacientes = dao.listarUsuarios();
            return Json(new { pacientes }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Registrar([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password,tipo")] Usuarios usuario)
        {
            string password = EncodePassword(usuario.password);
            usuario.password = password;
            bool result = dao.agregarUsuario(usuario);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            bool result = dao.eliminarUsuario(id);
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public static string EncodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }


        [HttpPost]
        public JsonResult Editar([Bind(Include = "id_usuario,nombre,ape_pat,ape_mat,usuario,password,tipo")] Usuarios usuario)
        {
            bool result = false;

            if (usuario != null)
            {
                string password = EncodePassword(usuario.password);
                usuario.password = password;
                result = dao.editarUsuario(usuario);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


    }
}