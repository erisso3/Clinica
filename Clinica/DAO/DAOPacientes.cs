using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinica.DAO
{
    public class DAOPacientes
    {
        private ContextDb db { get; set; }

        public DAOPacientes()
        {
            db = new ContextDb();
        }

        public bool agregar(Pacientes paciente)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Pacientes.Add(paciente);
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

        public bool eliminar(int id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Pacientes paciente = db.Pacientes.Find(id);
                    db.Pacientes.Remove(paciente);
                    //db.Entry(paciente).State = EntityState.Deleted;
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

        public List<Pacientes> getPacientes()
        {
            return db.Pacientes.ToList();
        }
    }
}