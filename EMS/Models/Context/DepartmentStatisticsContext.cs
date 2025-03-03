using Microsoft.EntityFrameworkCore;
using EMS.Models.Entity;
using EMS.Models.ViewModels;

namespace EMS.Models.Context
{
    public class DepartmentStatisticsContext : DbContext
    {
        public DepartmentStatisticsContext(DbContextOptions<DepartmentStatisticsContext> options) : base(options)
        {
        }

    }
}