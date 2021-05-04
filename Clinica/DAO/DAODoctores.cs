using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


        public static Doctores getDoctor(string usuario, string password)
        {
            ContextDb db = new ContextDb();
            try
            {
                string sql = "select * from Doctores where usuario = @a and password = @b";
                Doctores doctor = db.Database.SqlQuery<Doctores>(sql, new SqlParameter("@a", usuario), new SqlParameter("@b", password)).First();
                return doctor.id_doctor > 0 ? doctor : null;
            }
            catch (Exception)
            {

            }
            return null;

        }
    }
}