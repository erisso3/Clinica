﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Clinica.Models;
using Clinica.Objects;

namespace Clinica.DAO
{
    public class DAOTicket
    {
        private ContextDb db { get; set; }

        public DAOTicket()
        {
            db = new ContextDb();
        }
        public List<TicketObject> getTickets()
        {
            string sql = "Select t.id_ticket, u.nombre+' '+u.ape_pat+' '+u.ape_mat as nombreDoctor,u.usuario as usuarioDoctor, p.nombre+' '+p.ape_pat+' '+p.ape_mat as nombrePaciente,CONVERT(VARCHAR(10), t.fecha,101) fecha ,t.total from Tickets t, Citas c, Pacientes p, Usuarios u where t.id_cita=c.id_cita and c.id_paciente=p.id_paciente and c.id_doctor=u.id_usuario ;";
            return db.Database.SqlQuery<TicketObject>(sql).ToList();
        }

        public bool agregar(Tickets ticket)
        {

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error en el DAOTickect");
                    //hacemos rollback si hay excepción
                    dbContextTransaction.Rollback();

                }
            }
            return false;
        }

        public TicketObject getTickect(int id)
        {
            string sql = "select id_ticket,id_cita,documento,ruta,CONVERT(VARCHAR(10),fecha,101) fecha , total from Tickets where id_ticket= @a ";
            return db.Database.SqlQuery<TicketObject>(sql, new SqlParameter("@a", id)).ToList().Last();
        }
    }
}