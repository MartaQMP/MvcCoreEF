using Microsoft.EntityFrameworkCore;
using MvcCoreDepartamentos.Models;

namespace MvcCoreDepartamentos.Data
{
    public class DepartamentoContext:DbContext
    {
        public DepartamentoContext(DbContextOptions<DepartamentoContext> options): base(options){}

        public DbSet<Departamento> Departamentos { get; set; }
    }
}
