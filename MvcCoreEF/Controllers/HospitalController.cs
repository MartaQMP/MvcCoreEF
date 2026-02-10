using Microsoft.AspNetCore.Mvc;
using MvcCoreEF.Models;
using MvcCoreEF.Repositories;
using System.Threading.Tasks;

namespace MvcCoreEF.Controllers
{
    public class HospitalController : Controller
    {
        private RepositoryHospital repo;

        public HospitalController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Hospital> hospitales = await this.repo.GetHospitalesAsync();
            if(hospitales == null)
            {
                ViewBag.Mensaje = "No hay Hospitales";
                return View();
            }
            return View(hospitales);
        }

        public async Task<IActionResult> Details(int idHospital)
        {
            Hospital hospital = await this.repo.FindHospitalByIdAsync(idHospital);
            return View(hospital);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Hospital hos)
        {
            await this.repo.CreateHospitalAsync(hos.IdHospital, hos.Nombre, hos.Direccion, hos.Telefono, hos.Camas);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int idHospital)
        {
            await this.repo.DeleteHospitalAsync(idHospital);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int idHospital)
        {
            Hospital hos = await this.repo.FindHospitalByIdAsync(idHospital);
            return View(hos);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Hospital hos)
        {
            await this.repo.UpdateHospitalAsync(hos.IdHospital, hos.Nombre, hos.Direccion, hos.Telefono, hos.Camas);
            return RedirectToAction("Index");
        }
    }
}
