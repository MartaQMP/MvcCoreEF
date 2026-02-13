using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEF.Models;
using MvcCoreProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcCoreProceduresEF.Controllers
{
    public class VistaEmpleadoDepartamentoController : Controller
    {
        RepositoryEmpleado repo; 

        public VistaEmpleadoDepartamentoController(RepositoryEmpleado repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleadoDepartamento> empleados = await this.repo.GetEmpleadoDepartamentosAsync();
            return View(empleados);
        }
    }
}
