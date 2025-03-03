using System;

namespace EMS.Models.ViewModels
{
    public class DepartmentStatisticsViewModel
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public int EmployeeCount { get; set; }
        public decimal AverageSalary { get; set; }
    }
}