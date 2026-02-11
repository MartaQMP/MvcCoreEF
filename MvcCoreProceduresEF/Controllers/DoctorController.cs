using Microsoft.AspNetCore.Mvc;
using MvcCoreEF.Models;
using MvcCoreProceduresEF.Repositories;
using System.Threading.Tasks;

namespace MvcCoreProceduresEF.Controllers
{
    public class DoctorController : Controller
    {
        RepositoryDoctor repo;

        public DoctorController (RepositoryDoctor repo)
        {
            this.repo = repo;
        }


        public async Task<IActionResult> Index()
        {
            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewBag.Especialidades = especialidades;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string especialidad, int salario, string accion)
        {

            List<string> especialidades = await this.repo.GetEspecialidadesAsync();
            ViewBag.Especialidades = especialidades;
            if (accion.Equals("buscar"))
            {
                List<Doctor> doctoresEspecialidad = await this.repo.GetDoctoresEspecialidadAsync(especialidad);
                return View(doctoresEspecialidad);

            } else if (accion.Equals("procedure"))
            {
                await this.repo.UpdateSalarioProcedureAsync(especialidad, salario);
            }else
            {
                await this.repo.UpdateSalarioAsync(especialidad, salario);
                
            }
            List<Doctor> doctores = await this.repo.GetDoctoresEspecialidadAsync(especialidad);
            return View(doctores);
        }

    }
}
