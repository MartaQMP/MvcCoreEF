using Microsoft.CodeAnalysis;
using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleado
    {
        Task<List<Empleado>> GetEmpleadosAsync();
        Task<Empleado> GetEmpleadoById(int idEmpleado);
        Task CreateEmpleado(string apellido, string oficio, int dir, int salario, int comision, string dept);
    }
}
