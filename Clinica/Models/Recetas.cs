namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Recetas
    {
        [Key]
        public int id_receta { get; set; }

        public int id_cita { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string documento { get; set; }

        [Required]
        [StringLength(100)]
        public string ruta { get; set; }

        [Required]
        [StringLength(50)]
        public string ids_medicamentos { get; set; }

        [Column(TypeName = "date")]
        public DateTime fecha { get; set; }

        [Required]
        [StringLength(100)]
        public string observacion { get; set; }

        [Required]
        [StringLength(100)]
        public string instruccion { get; set; }
    }
}
