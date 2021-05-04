using Clinica.Models;
using Clinica.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Clinica.DAO
{
    public class DAOCitas
    {
        private ContextDb db { get; set; }

        public DAOCitas()
        {
            db = new ContextDb();
        }

        public bool agregar(CitaObject citaObject)
        {
            Citas cita = new Citas();
            cita.id_paciente = citaObject.id_paciente;
            cita.id_doctor = citaObject.id_doctor;
            cita.status = 0;
            System.Diagnostics.Debug.WriteLine("FECHA: "+citaObject.fecha);
            DateTime DateObject = DateTime.ParseExact(citaObject.fecha, "yyyy-MM-dd", null);
            TimeSpan time = new TimeSpan(citaObject.hora, 0, 0);
            cita.fecha = DateObject;
            cita.hora = time;
            cita.observacion = citaObject.observacion;

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Citas.Add(cita);
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

        public List<CitaObject> getCitasPaciente(int id)
        {
            string sql = "SELECT c.id_cita, c.id_paciente, c.id_doctor, c.status, c.observacion, c.fecha as fechag, c.hora as horag , (p.nombre + p.ape_pat+ p.ape_mat) as doctor FROM Citas as c INNER JOIN Pacientes as p ON c.id_paciente = p.id_paciente WHERE c.id_paciente = @a";
            return db.Database.SqlQuery<CitaObject>(sql, new SqlParameter("@a", id)).ToList();
        }


        public List<CitasDoctorObject> getCitasPendientes(int id_doctor)
        {
            string sql = "select c.id_cita,c.id_paciente, p.nombre+' '+p.ape_pat+' '+ape_mat as nombrePaciente, c.fecha, c.hora, c.observacion from citas c, Pacientes p where id_doctor= @a and c.id_paciente=p.id_paciente and c.status=0";
            return db.Database.SqlQuery<CitasDoctorObject>(sql, new SqlParameter("@a", id_doctor)).ToList();
        }

        public List<CitasDoctorObject> getCitasAceptadas(int id_doctor)
        {
            string sql = "select c.id_cita,c.id_paciente, p.nombre+' '+p.ape_pat+' '+ape_mat as nombrePaciente, c.fecha, c.hora, c.observacion from citas c, Pacientes p where id_doctor= @a and c.id_paciente=p.id_paciente and c.status=1";
            return db.Database.SqlQuery<CitasDoctorObject>(sql, new SqlParameter("@a", id_doctor)).ToList();
        }

        public List<CitasDoctorObject> getCitasRechazadas(int id_doctor)
        {
            string sql = "select c.id_cita,c.id_paciente, p.nombre+' '+p.ape_pat+' '+ape_mat as nombrePaciente, c.fecha, c.hora, c.observacion from citas c, Pacientes p where id_doctor= @a and c.id_paciente=p.id_paciente and c.status=2";
            return db.Database.SqlQuery<CitasDoctorObject>(sql, new SqlParameter("@a", id_doctor)).ToList();
        }

        public List<CitasDoctorObject> getCitasRealizadas(int id_doctor)
        {
            string sql = "select c.id_cita,c.id_paciente, p.nombre+' '+p.ape_pat+' '+ape_mat as nombrePaciente, c.fecha, c.hora, c.observacion from citas c, Pacientes p where id_doctor= @a and c.id_paciente=p.id_paciente and c.status=3";
            return db.Database.SqlQuery<CitasDoctorObject>(sql, new SqlParameter("@a", id_doctor)).ToList();
        }

        public Boolean statusCita(int id_cita, int status)
        {
            Boolean bandera = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Citas cita = db.Citas.Find(id_cita);
                    cita.status = status;
                    db.Entry(cita).State = EntityState.Modified;
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    bandera = true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
                return bandera;
        }

        public Boolean eliminarCita(int id_cita)
        {
            Boolean bandera = false;
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Citas cita = db.Citas.Find(id_cita);
                    db.Citas.Remove(cita);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    bandera = true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
            return bandera;
        }

    }
}