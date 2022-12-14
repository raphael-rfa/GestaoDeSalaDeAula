using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestãoDeSalaDeAula.Models;

namespace GestãoDeSalaDeAula.Data
{
    public class GestãoDeSalaDeAulaContext : DbContext
    {
        public GestãoDeSalaDeAulaContext (DbContextOptions<GestãoDeSalaDeAulaContext> options)
            : base(options)
        {
        }

        public DbSet<GestãoDeSalaDeAula.Models.Alunos> Alunos { get; set; }
        public DbSet<GestãoDeSalaDeAula.Models.Materias> Materias { get; set; }
        public DbSet<GestãoDeSalaDeAula.Models.Provas> Provas { get; set; }
        public DbSet<GestãoDeSalaDeAula.Models.Professores> Professores { get; set; }
        public DbSet<GestãoDeSalaDeAula.Models.ProfessorMateria> ProfessorMateria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alunos>().ToTable("Alunos");
            modelBuilder.Entity<Materias>().ToTable("Materias");
            modelBuilder.Entity<Provas>().ToTable("Provas");
            modelBuilder.Entity<Professores>().ToTable("Professores");
            modelBuilder.Entity<ProfessorMateria>().ToTable("ProfessorMateria");

            modelBuilder.Entity<ProfessorMateria>()
                .HasKey(p => new { p.MateriasId, p.ProfessoresId});
        }
    }
}
