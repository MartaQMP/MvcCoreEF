using Microsoft.EntityFrameworkCore;
using MvcCoreDepartamentos.Data;
using MvcCoreDepartamentos.Models;

namespace MvcCoreDepartamentos.Repositories
{
    public class RepositoryDepartamento
    {
        DepartamentoContext context;

        public RepositoryDepartamento(DepartamentoContext context)
        {
            this.context = context;
        }


        public async Task<List<Departamento>> GetDepartametnosAsync()
        {
            var consulta = from datos in this.context.Departamentos
                           select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Departamento> FindDeptByIdAsync(int dept_no)
        {
            var consulta = from datos in this.context.Departamentos
                           where datos.Dept_No == dept_no
                           select datos;
            return await consulta.FirstOrDefaultAsync();

        }

        public async Task CreateDepartamentoAsync(int dept_no, string nombre, string localidad)
        {
            Departamento dept = new Departamento();
            dept.Dept_No = dept_no;
            dept.Nombre = nombre;
            dept.Localidad = localidad;
            await this.context.Departamentos.AddAsync(dept);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteDepartamentoAsync(int dept_no)
        {
            Departamento dept = await this.FindDeptByIdAsync(dept_no);
            this.context.Departamentos.Remove(dept);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateDepartamentoAsync(int dept_no, string nombre, string localidad)
        {
            Departamento dept = await this.FindDeptByIdAsync(dept_no);
            // MODIFICAMOS TODAS SUS PROPIEDADES EXCEPTO SU Key
            dept.Nombre = nombre;
            dept.Localidad = localidad;
            await this.context.SaveChangesAsync();
        }
    }
}
