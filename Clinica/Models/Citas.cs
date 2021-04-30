namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Citas
    {
        [Key]
        public int id_cita { get; set; }

        public int id_paciente { get; set; }

        public int id_doctor { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public TimeSpan hora { get; set; }

        public int status { get; set; }

        [Required]
        [StringLength(150)]
        public string observacion { get; set; }
    }
}
