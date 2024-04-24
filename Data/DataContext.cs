using Microsoft.EntityFrameworkCore;
using SimpleA.Models;

namespace SimpleA.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Applicant)
                .WithMany(a => a.Skills)
                .HasForeignKey(s => s.ApplicantId);
        }

        public void SeedData()
        {
            if (!Applicants.Any())
            {
                // Seed Applicant data
                var applicant = new Applicant
                {
                    Id = 1,
                    Name = "Bulelani Rwexane"
                };
                Applicants.Add(applicant);

                // Seed Skills data
                var skills = new List<Skill>
            {
                new Skill { Id = 1, Name = "C#", ApplicantId = 1 },
                new Skill { Id = 2, Name = "ASP.NET Core", ApplicantId = 1 },
                new Skill { Id = 3, Name = "API", ApplicantId = 1 }
            };
                Skills.AddRange(skills);

                SaveChanges();
            }
        }
    }
}
