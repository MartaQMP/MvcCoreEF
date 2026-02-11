using Microsoft.AspNetCore.Mvc;
using MvcCoreProceduresEF.Models;
using MvcCoreProceduresEF.Repositories;

namespace MvcCoreProceduresEF.Controllers
{
    public class EnfermoController : Controller
    {
        RepositoryEnfermo repo;

        public EnfermoController (RepositoryEnfermo repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermosAsync();
            if(enfermos == null)
            {
                ViewBag.Mensaje = "No hay enfermos";
                return View();
            }
            return View(enfermos);
        }

        public async Task<IActionResult> Details(string inscripcion)
        {
            Enfermo enfermo = await this.repo.GetEnfermoById(inscripcion);
            if(enfermo == null)
            {
                ViewBag.Mensaje = "No se ha encontrado ningun enfermo con esa inscripcion";
                return View();
            }
            return View(enfermo);
        }

        public async Task<IActionResult> Delete(string inscripcion)
        {
            await this.repo.DeleteEnfermoAsync(inscripcion);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteRaw(string inscripcion)
        {
            await this.repo.DeleteEnfermoRawAsync(inscripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enfermo enf)
        {
            await this.repo.CreateEnfermo(enf.Apellido, enf.Direccion, enf.FechaNac, enf.Genero, enf.NSS);
            return RedirectToAction("Index");
        }
    }
}
