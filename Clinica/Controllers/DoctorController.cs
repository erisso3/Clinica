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
        public DAODoctores dao = new DAODoctores();
        // GET: Doctores
        public JsonResult ListarDoctores()
        {
            List<Doctores> lista = dao.listarDoctores();
            if (lista != null)
            {
                return Json(new { lista }, JsonRequestBehavior.AllowGet);
            }
            return Json("no", JsonRequestBehavior.AllowGet);
        }
    }
}