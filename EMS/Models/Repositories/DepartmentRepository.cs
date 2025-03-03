using EMS.Models.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMS.Models.Entity;

namespace EMS.Models.Repositories
{
    public class DepartmentRepository : IRepository<Department>
    {
        private readonly EMSContext _context;

        public DepartmentRepository(EMSContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id) ?? throw new InvalidOperationException($"Department with ID {id} not found");
        }

        public async Task InsertAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
        }

        public Task UpdateAsync(Department entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Department entity)
        {
            _context.Departments.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Department> GetWithEmployeesAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id) ?? throw new InvalidOperationException($"Department with ID {id} not found");
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