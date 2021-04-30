using Clinica.Models;
using System;
using System.Collections.Generic;
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
    }
}