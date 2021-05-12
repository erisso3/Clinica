using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Objects
{
    public class TicketObject
    {
        public int id_ticket { get; set; }
        public string nombreDoctor { get; set; }
        public string usuarioDoctor { get; set; }
        public string nombrePaciente { get; set; }
        public DateTime fecha { get; set; }
        public double total { get; set; }
    }
}