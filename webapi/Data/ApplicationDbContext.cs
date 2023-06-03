using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Models;

namespace Przychodnia.Webapi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Patient>(entity => { entity.ToTable("Patients").HasIndex(p => p.Pesel).IsUnique(); });
            builder.Entity<Employee>(entity => { entity.ToTable("Employees").HasIndex(e => e.Pesel).IsUnique(); });
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }
    }
}
