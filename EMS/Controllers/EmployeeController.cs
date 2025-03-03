using System;
using Microsoft.AspNetCore.Mvc;
using EMS.Models.Entity;
using EMS.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Models.Repositories;

namespace EMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;

        public EmployeeController(
            IRepository<Employee> employeeRepository,
            IRepository<Department> departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(EmployeeFilterViewModel filter)
        {
            ViewBag.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");

            var employees = await ((EmployeeRepository)_employeeRepository).GetFilteredAsync(
                filter.DepartmentId,
                filter.HireDateStart,
                filter.HireDateEnd,
                filter.MinSalary,
                filter.MaxSalary,
                filter.SortBy,
                filter.SortDirection);

            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.DepartmentId = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _employeeRepository.InsertAsync(employee);
                    await _employeeRepository.SaveAsync();
                    return Json(new { success = true, message = "Çalışan başarıyla oluşturuldu." });
                }
                return Json(new { success = false, message = "Çalışan oluşturulurken bir hata oluştu." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Çalışan oluşturulurken bir hata oluştu." });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.DepartmentId = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _employeeRepository.UpdateAsync(employee);
                    await _employeeRepository.SaveAsync();
                    return Json(new { success = true, message = "Çalışan başarıyla güncellendi." });
                }
                return Json(new { success = false, message = "Çalışan güncellenirken bir hata oluştu." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Çalışan güncellenirken bir hata oluştu." });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            await _employeeRepository.DeleteAsync(employee);
            await _employeeRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _employeeRepository.Dispose();
                _departmentRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}