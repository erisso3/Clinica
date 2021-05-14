namespace Clinica.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }

        [Required]
        [StringLength(30)]
        public string nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string ape_pat { get; set; }

        [Required]
        [StringLength(30)]
        public string ape_mat { get; set; }

        [Required]
        [StringLength(80)]
        public string usuario { get; set; }

        [Required]
        [StringLength(64)]
        public string password { get; set; }

        public int tipo { get; set; }
    }
}
