using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure
{
    public class StudyBankContext : DbContext, IStudyBankContext
    {
        public DbSet<Student> Students => Set<Student>();

        public DbSet<Supervisor> Supervisors => Set<Supervisor>();

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<Tag> Tags => Set<Tag>();

        public StudyBankContext(DbContextOptions<StudyBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyBankUser>().HasIndex(e => e.Email).IsUnique();

        }
    }
}