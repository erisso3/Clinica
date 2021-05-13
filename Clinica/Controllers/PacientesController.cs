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
        public JsonResult ListarPacientes()
        {
            List<Pacientes> pacientes = dao.getPacientes();
            if ((pacientes != null))
            {
                return Json(new { pacientes }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Registrar([Bind(Include = "nombre,ape_pat,ape_mat,usuario,password")] Pacientes paciente)
        {
            bool result = dao.agregar(paciente);
            if (result)
            {
                return Json("exito", JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            bool result = dao.eliminar(id);
            if (result)
            {
                return Json("exito", JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }

    }
}