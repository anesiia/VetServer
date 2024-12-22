using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

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
        public DbSet<Kinds> Kinds { get; set; }
        public DbSet<Cages> Cages { get; set; }
        public DbSet<OwnersPatients> OwnersPatients { get; set; }
        public DbSet<AppointmentCount> AppointmentCount { get; set; }

        
    }
}
