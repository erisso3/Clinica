using Clinica.DAO;
using Clinica.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuariosController : Controller
    {

        private DAOUsuarios dao = new DAOUsuarios();
        public JsonResult ListarUsuarios()
        {
            List<Usuarios> listaUsuarios = dao.listarUsuarios();
            return Json(new { listaUsuarios }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Registrar([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password,tipo")] Usuarios usuarios)
        {
            System.Diagnostics.Debug.WriteLine(usuarios.nombre);
            System.Diagnostics.Debug.WriteLine(usuarios.ape_pat);
            System.Diagnostics.Debug.WriteLine(usuarios.ape_mat);
            string password = EncodePassword(usuarios.password);
            usuarios.password = password;
            bool result = dao.agregarUsuario(usuarios);
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
        public JsonResult Editar([Bind(Include = "id_usuario,nombre,ape_pat,ape_mat,usuario,password,tipo")] Usuarios usuarios)
        {
            bool result = false;

            if (usuarios != null)
            {
                string password = EncodePassword(usuarios.password);
                usuarios.password = password;
                result = dao.editarUsuario(usuarios);
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }


    }
}