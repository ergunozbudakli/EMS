using System;
using System.ComponentModel.DataAnnotations;


namespace EMS.Models.ViewModels
{
    public class EmployeeFilterViewModel
    {
        [Display(Name = "Departman")]
        public int? DepartmentId { get; set; }

        [Display(Name = "İşe Başlama Tarihi (Başlangıç)")]
        [DataType(DataType.Date)]
        public DateTime? HireDateStart { get; set; }

        [Display(Name = "İşe Başlama Tarihi (Bitiş)")]
        [DataType(DataType.Date)]
        public DateTime? HireDateEnd { get; set; }

        [Display(Name = "Minimum Maaş")]
        public decimal? MinSalary { get; set; }

        [Display(Name = "Maksimum Maaş")]
        public decimal? MaxSalary { get; set; }

        [Display(Name = "Sıralama")]
        public string? SortBy { get; set; }

        [Display(Name = "Sıralama Yönü")]
        public string? SortDirection { get; set; }
    }
}