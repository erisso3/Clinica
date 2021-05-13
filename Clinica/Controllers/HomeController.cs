using Clinica.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Clinica.Models;

namespace Clinica.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult Index()
        {
            return Json("HOLA", JsonRequestBehavior.AllowGet);
        }

    }
}