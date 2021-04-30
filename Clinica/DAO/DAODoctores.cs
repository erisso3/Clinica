using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.DAO
{
    public class DAODoctores
    {
        private ContextDb db { get; set; }

        public DAODoctores()
        {
            db = new ContextDb();
        }

        public List<Doctores> listarDoctores()
        {
            return db.Doctores.ToList();
        }
    }
}