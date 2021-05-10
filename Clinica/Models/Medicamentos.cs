namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Medicamentos
    {
        [Key]
        public int id_medicamento { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre { get; set; }

        [Required]
        [StringLength(10)]
        public string dosis { get; set; }

        public double precio { get; set; }

        [Required]
        [StringLength(100)]
        public string indicaciones { get; set; }
    }
}
