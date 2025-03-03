using EMS.Models.Context;
using EMS.Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;

namespace EMS.Models.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly EMSContext _context;

        public EmployeeRepository(EMSContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id) ?? throw new InvalidOperationException($"Employee with ID {id} not found");
        }

        public async Task InsertAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
        }

        public Task UpdateAsync(Employee entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Employee entity)
        {
            _context.Employees.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetFilteredAsync(
            int? departmentId = null,
            DateTime? hireDateStart = null,
            DateTime? hireDateEnd = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            string? sortBy = null,
            string? sortDirection = null)
        {
            // LINQ 
            var query = from e in _context.Employees
                       join d in _context.Departments on e.DepartmentId equals d.Id
                       select new Employee
                       {
                           Id = e.Id,
                           FullName = e.FullName,
                           Email = e.Email,
                           PhoneNumber = e.PhoneNumber,
                           DepartmentId = e.DepartmentId,
                           Department = d,
                           HireDate = e.HireDate,
                           Salary = e.Salary
                       };

            // Filtreleri uygula
            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId);

            if (hireDateStart.HasValue)
                query = query.Where(e => e.HireDate >= hireDateStart.Value.Date);

            if (hireDateEnd.HasValue)
                query = query.Where(e => e.HireDate <= hireDateEnd.Value.Date);

            if (minSalary.HasValue)
                query = query.Where(e => e.Salary >= minSalary);

            if (maxSalary.HasValue)
                query = query.Where(e => e.Salary <= maxSalary);

            // Sıralama işlemini uygula
            query = ApplySorting(query, sortBy, sortDirection);

            // Sorguyu çalıştır ve sonuçları döndür
            return await query.ToListAsync();
        }

        private IQueryable<Employee> ApplySorting(IQueryable<Employee> query, string? sortBy, string? sortDirection)
        {
            var isAscending = string.IsNullOrEmpty(sortDirection) || 
                             sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);

            return (sortBy?.ToLower()) switch
            {
                "fullname" => isAscending ? 
                    query.OrderBy(e => e.FullName) : 
                    query.OrderByDescending(e => e.FullName),
                
                "email" => isAscending ? 
                    query.OrderBy(e => e.Email) : 
                    query.OrderByDescending(e => e.Email),
                
                "department" => isAscending ? 
                    query.OrderBy(e => e.Department!.Name) : 
                    query.OrderByDescending(e => e.Department!.Name),
                
                "hiredate" => isAscending ? 
                    query.OrderBy(e => e.HireDate) : 
                    query.OrderByDescending(e => e.HireDate),
                
                "salary" => isAscending ? 
                    query.OrderBy(e => e.Salary) : 
                    query.OrderByDescending(e => e.Salary),
                
                _ => query.OrderBy(e => e.Id)
            };
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}