using Microsoft.EntityFrameworkCore;
using VetServer.Models;

namespace VetServer.Data
{
    public class VetCareDbContext : DbContext
    {
        public VetCareDbContext(DbContextOptions<VetCareDbContext> options) : base(options) { }

        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Owners> Owners { get; set; }
        public DbSet<Drugs> Drugs { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Cages> Cage { get; set; }



        public DbSet<Doctors> DoctorLoginModel { get; set; }
        public DbSet<Owners> OwnerLoginModel { get; set; }


    }
}
