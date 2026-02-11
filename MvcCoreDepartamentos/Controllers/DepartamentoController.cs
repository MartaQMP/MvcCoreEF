using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MvcCoreDepartamentos.Models;
using MvcCoreDepartamentos.Repositories;

namespace MvcCoreDepartamentos.Controllers
{
    public class DepartamentoController : Controller
    {
        RepositoryDepartamento repo;

        public DepartamentoController (RepositoryDepartamento repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Departamento> depts = await this.repo.GetDepartamentosAsync();
            if(depts == null)
            {
                ViewBag.Mensaje = "No hay departamentos";
                return View();
            }
            return View(depts);
        }

        /* DETALLES */
        public async Task<IActionResult> Details(int dept_no)
        {
            Departamento dept = await this.repo.FindDeptByIdAsync(dept_no);
            return View(dept);
        }

        /* GET POST CREATE */
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Departamento dept)
        {
            await this.repo.CreateDepartamentoAsync(dept.Dept_No, dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }
        
        /* GET POST UPDATE */
        public async Task<IActionResult> Update(int dept_no)
        {
            Departamento dept = await this.repo.FindDeptByIdAsync(dept_no);
            return View(dept);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Departamento dept)
        {
            await this.repo.UpdateDepartamentoAsync(dept.Dept_No, dept.Nombre, dept.Localidad);
            return RedirectToAction("Index");
        }

        /* DELETE */
        public async Task<IActionResult> Delete(int dept_no)
        {
            await this.repo.DeleteDepartamentoAsync(dept_no);
            return RedirectToAction("Index");
        }

    }
}
