using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Clinica.DAO
{
    public class DAORecetas
    {
        private ContextDb db { get; set; }

        public DAORecetas()
        {
            db = new ContextDb();
        }

        public bool agregar(Recetas receta, Citas cita)
        {
            
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    cita.status = 3;
                    //db.Entry(cita).State = EntityState.Modified;
                    db.Recetas.Add(receta);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    //hacemos rollback si hay excepción
                    dbContextTransaction.Rollback();

                }
            }
            return false;
        }
        public List<Recetas> getRecetasPaciente(int id)
        {
            string sql = "SELECT r.* FROM Recetas as r WHERE r.id_cita IN (SELECT c.id_cita FROM Citas as c WHERE c.id_paciente = @a)";
            return db.Database.SqlQuery<Recetas>(sql, new SqlParameter("@a", id)).ToList();
        }

        public Recetas getReceta(int id)
        {
            return db.Recetas.Find(id);
        }

    }
}