using Microsoft.EntityFrameworkCore;
using MvcCoreEF.Models;
using MvcCoreProceduresEF.Models;

namespace MvcCoreProceduresEF.Data
{
    public class HospitalContext: DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options): base(options) {}

        public DbSet<Enfermo> Enfermos { get; set; }
        public DbSet<Doctor> Doctores { get; set; }
        public DbSet<VistaEmpleadoDepartamento> VistaEmpleados { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}
