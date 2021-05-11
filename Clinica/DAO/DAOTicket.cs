using System;
using System.Collections.Generic;
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
            string sql = "Select t.id_ticket, u.nombre+' '+u.ape_pat+' '+u.ape_mat as nombreDoctor,u.usuario as usuarioDoctor, p.nombre+' '+p.ape_pat+' '+p.ape_mat as nombrePaciente, t.fecha,t.total from Tickets t, Citas c, Pacientes p, Usuarios u where t.id_cita=c.id_cita and c.id_paciente=p.id_paciente and c.id_doctor=u.id_usuario ;";
            return db.Database.SqlQuery<TicketObject>(sql).ToList();
        }
    }
}