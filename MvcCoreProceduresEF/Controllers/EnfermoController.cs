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
    }
}
