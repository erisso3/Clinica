using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;

namespace Clinica.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AsistenteController : Controller
    {
        private DAOTicket dao = new DAOTicket();
        public JsonResult ListarTickets()
        {
            List<TicketObject> tickets = dao.getTickets();
            return Json(new { tickets }, JsonRequestBehavior.AllowGet);
        }

    }
}