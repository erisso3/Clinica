﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class CitasController : Controller
    {
        // GET: Citas
        public ActionResult Index()
        {
            ViewBag.estadoCitas= "mm-active";
            return View();
        }

        public ActionResult DetallesCita()
        {
            return View();
        }
    }
}