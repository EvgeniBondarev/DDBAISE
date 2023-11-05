using System.ComponentModel.DataAnnotations;

namespace PostSity.Models
{
    public class EmployeePosition
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Position { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
