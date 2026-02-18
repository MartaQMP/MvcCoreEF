using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadoController : Controller
    {
        IRepositoryEmpleado repo; 

        public EmpleadoController(IRepositoryEmpleado repo)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string apellido, string oficio, int dir, int salario, int comision, string dept)
        {
            int id = await this.repo.CreateEmpleado(apellido, oficio, dir, salario, comision, dept);
            return RedirectToAction("Details", new {idEmpleado = id});
        }
    }
}
