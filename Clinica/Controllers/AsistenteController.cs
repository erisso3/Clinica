using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;

namespace Clinica.Controllers
{
    public class AsistenteController : Controller
    {
        private DAOTicket dao = new DAOTicket();
        // GET: Asistente
        public ActionResult Index()
        {
            Usuarios usuario = (Usuarios)Session["Usuario"];
            if (usuario == null)
            {
                return RedirectToAction("LogOut", "Login");
            }
            ViewBag.estadoMenuAsistente = "mm-active";
            List<TicketObject> tickets = dao.getTickets();
            return View(tickets);
        }
    }
}