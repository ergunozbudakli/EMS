using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Models.Entity
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur.")]
        [Display(Name = "Departman Adı")]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}