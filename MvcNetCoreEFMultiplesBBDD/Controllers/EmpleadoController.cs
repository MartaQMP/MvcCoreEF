using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadoController : Controller
    {
        RepositoryEmpleado repo; 

        public EmpleadoController(RepositoryEmpleado repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int idEmpleado)
        {
            Empleado emp = await this.repo.GetEmpleadoById(idEmpleado);
            return View(emp);
        }
        
    }
}
