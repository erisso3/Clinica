﻿using Clinica.DAO;
using Clinica.Models;
using Clinica.Objects;
using Clinica.Objects;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace Clinica.Controllers
{
    public class RecetasController : Controller
    {
        private DAORecetas daoRecetas = new DAORecetas();

        public JsonResult ListarRecetasPacientes(int id)
        {
            List<Receta> recetas = daoRecetas.getRecetasPaciente(id);
            return Json(new { recetas }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Previsualizar(int id)
        {
            Receta receta = daoRecetas.getReceta(id);
            return Json(new { receta }, JsonRequestBehavior.AllowGet);
        }
    }
}