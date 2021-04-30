using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinica.DAO
{
    public class DAOLogin
    {
        private ContextDb db { get; set; }

        public DAOLogin()
        {
            db = new ContextDb();
        }

        public Pacientes getPaciente(string usuario, string password)
        {
            string sql = "SELECT * FROM Pacientes WHERE usuario = @a and password = @b";
            return db.Database.SqlQuery<Pacientes>(sql, new SqlParameter("@a", usuario), new SqlParameter("@b", password)).FirstOrDefault();
        }
    }
}