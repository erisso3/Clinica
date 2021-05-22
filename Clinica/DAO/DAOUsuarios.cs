using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Clinica.DAO
{
    public class DAOUsuarios
    {
        private ContextDb db { get; set; }

        public DAOUsuarios()
        {
            db = new ContextDb();
        }

        public List<Usuarios> listarDoctores()
        {
            return db.Usuarios.Where(x => x.tipo == 0).ToList();
        }


        public static Usuarios getUsuario(string usu, string pas)
        {
            ContextDb db = new ContextDb();
            System.Diagnostics.Debug.WriteLine("Entre al DAO");
            try
            {
                string sql = "select * from Usuarios where usuario = @a and password = @b";
                Usuarios usuario = db.Database.SqlQuery<Usuarios>(sql, new SqlParameter("@a", usu), new SqlParameter("@b", pas)).First();
                return usuario.id_usuario > 0 ? usuario : null;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error en la conexión");
            }
            return null;

        }

        public List<Usuarios> listarUsuarios()
        {
            return db.Usuarios.ToList();
        }

        public bool agregarUsuario(Usuarios usuario)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Usuarios.Add(usuario);
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


        public bool editarUsuario(Usuarios usuario)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(usuario).State = EntityState.Modified;
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


        public bool eliminarUsuario(int id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Usuarios usuario = db.Usuarios.Find(id);
                    db.Usuarios.Remove(usuario);
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