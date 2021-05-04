using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Objects
{
    public class CitasDoctorObject
    {
        public int id_cita { get; set; }
        public int id_paciente { get; set; }
        public string nombrePaciente { get; set; }
        public DateTime fecha { get; set; }
        public TimeSpan hora { get; set; }
        public string observacion { get; set; }
    }
}