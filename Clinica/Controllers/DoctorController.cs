using Clinica.DAO;
using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace Clinica.Controllers
{
    [EnableCors (origins: "*", headers:"*",methods:"*" )]
    public class DoctorController : Controller
    {
        public DAOUsuarios dao = new DAOUsuarios();
        // GET: Doctores
        public JsonResult ListarDoctores()
        {
            List<Usuarios> lista = dao.listarDoctores();
            if (lista != null)
            {
                return Json(new { lista }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
    }
}