namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tickets
    {
        [Key]
        public int id_ticket { get; set; }

        public int id_cita { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string documento { get; set; }

        [Required]
        [StringLength(100)]
        public string ruta { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        public double total { get; set; }
    }
}
