using System.Net;
using Microsoft.AspNetCore.Mvc;
using EMS.Models.Entity;
using EMS.Models.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EMS.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IRepository<Department> _repository;

        public DepartmentController(IRepository<Department> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAsync());
        }

        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.InsertAsync(department);
                    await _repository.SaveAsync();
                    return Json(new { success = true, message = "Departman başarıyla oluşturuldu." });
                }
                return Json(new { success = false, message = "Departman oluşturulurken bir hata oluştu." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Departman oluşturulurken bir hata oluştu." });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = await _repository.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.UpdateAsync(department);
                    await _repository.SaveAsync();
                    return Json(new { success = true, message = "Departman başarıyla güncellendi." });
                }
                return Json(new { success = false, message = "Departman güncellenirken bir hata oluştu." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Departman güncellenirken bir hata oluştu." });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var department = await ((DepartmentRepository)_repository).GetWithEmployeesAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            ViewBag.HasEmployees = department.Employees.Any();
            ViewBag.EmployeeCount = department.Employees.Count;
            
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromBody] int id)
        {
            try
            {
                var department = await ((DepartmentRepository)_repository).GetWithEmployeesAsync(id);
                if (department == null)
                {
                    return Json(new { success = false, message = "Departman bulunamadı." });
                }

                if (department.Employees.Any())
                {
                    return Json(new { success = false, message = $"Bu departmanda {department.Employees.Count} çalışan bulunmaktadır. Önce çalışanları başka bir departmana taşıyın veya silin." });
                }

                await _repository.DeleteAsync(department);
                await _repository.SaveAsync();
                return Json(new { success = true, message = "Departman başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Departman silinirken bir hata oluştu: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}