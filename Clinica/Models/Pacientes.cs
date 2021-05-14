namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Pacientes
    {
        [Key]
        public int id_paciente { get; set; }

        [Required]
        [StringLength(30)]
        public string nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string ape_pat { get; set; }

        [Required]
        [StringLength(30)]
        public string ape_mat { get; set; }

        [StringLength(80)]
        public string usuario { get; set; }

        [StringLength(64)]
        public string password { get; set; }
    }
}
