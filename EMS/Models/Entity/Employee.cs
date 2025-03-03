using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.Models.Entity
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Display(Name = "Telefon Numarası")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Telefon numarası 11 haneli olmalıdır.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "İşe giriş tarihi zorunludur.")]
        [Display(Name = "İşe Giriş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Maaş alanı zorunludur.")]
        [Display(Name = "Maaş")]
        [Range(0, double.MaxValue, ErrorMessage = "Maaş 0'dan büyük olmalıdır.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Departman seçimi zorunludur.")]
        public int DepartmentId { get; set; }

        [Display(Name = "Departman")]
        public virtual Department? Department { get; set; }
    }
}