using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEF.Models;
using MvcCoreProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcCoreProceduresEF.Controllers
{
    public class TrabajadorController : Controller
    {
        RepositoryEmpleado repo;

        public TrabajadorController (RepositoryEmpleado repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            TrabajadoresDatos trabajadoresDatos = await this.repo.GetTrabajadoresDatosAsync();
            List<string> oficios = await this.repo.GetOficios();
            ViewBag.Oficios = oficios;
            return View(trabajadoresDatos);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            TrabajadoresDatos trabajadoresDatos = await this.repo.GetTrabajadoresDatosOficioAsync(oficio);
            List<string> oficios = await this.repo.GetOficios();
            ViewBag.Oficios = oficios;
            return View(trabajadoresDatos);
        }
    }
}
