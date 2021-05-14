using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Clinica.Objects;

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
                    db.Entry(cita).State = EntityState.Modified;
                    db.Recetas.Add(receta);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    //hacemos rollback si hay excepción
                    System.Diagnostics.Debug.WriteLine("Error en el DAORecetas");
                    dbContextTransaction.Rollback();

                }
            }
            return false;
        }
        public List<Receta> getRecetasPaciente(int id)
        {
            string sql = "SELECT r.id_receta,r.id_cita,r.documento,r.ruta,r.ids_medicamentos,CONVERT(VARCHAR(10), r.fecha,101) fecha,r.observacion,r.instruccion FROM Recetas as r WHERE r.id_cita IN (SELECT c.id_cita FROM Citas as c WHERE c.id_paciente = @a )";
            return db.Database.SqlQuery<Receta>(sql, new SqlParameter("@a", id)).ToList();
        }

        public Receta getReceta(int id)
        {
            string sql = "select r.id_receta,r.id_cita,r.documento,r.ruta,r.ids_medicamentos,CONVERT(VARCHAR(10), r.fecha,101) fecha,r.observacion,r.instruccion FROM Recetas as r where id_receta= @a ";
            return db.Database.SqlQuery<Receta>(sql, new SqlParameter("@a", id)).ToList().Last();
        }

    }
}