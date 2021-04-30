namespace Clinica.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ContextDb : DbContext
    {
        public ContextDb()
            : base("name=ContextDb")
        {
        }

        public virtual DbSet<Citas> Citas { get; set; }
        public virtual DbSet<Doctores> Doctores { get; set; }
        public virtual DbSet<Pacientes> Pacientes { get; set; }
        public virtual DbSet<Recetas> Recetas { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Citas>()
                .Property(e => e.observacion)
                .IsUnicode(false);

            modelBuilder.Entity<Doctores>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Doctores>()
                .Property(e => e.ape_pat)
                .IsUnicode(false);

            modelBuilder.Entity<Doctores>()
                .Property(e => e.ape_mat)
                .IsUnicode(false);

            modelBuilder.Entity<Doctores>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Doctores>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Pacientes>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Pacientes>()
                .Property(e => e.ape_pat)
                .IsUnicode(false);

            modelBuilder.Entity<Pacientes>()
                .Property(e => e.ape_mat)
                .IsUnicode(false);

            modelBuilder.Entity<Pacientes>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Pacientes>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Recetas>()
                .Property(e => e.documento)
                .IsUnicode(false);

            modelBuilder.Entity<Recetas>()
                .Property(e => e.ruta)
                .IsUnicode(false);

            modelBuilder.Entity<Recetas>()
                .Property(e => e.ids_medicamentos)
                .IsUnicode(false);

            modelBuilder.Entity<Recetas>()
                .Property(e => e.observacion)
                .IsUnicode(false);

            modelBuilder.Entity<Recetas>()
                .Property(e => e.instruccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tickets>()
                .Property(e => e.documento)
                .IsUnicode(false);

            modelBuilder.Entity<Tickets>()
                .Property(e => e.ruta)
                .IsUnicode(false);
        }
    }
}
