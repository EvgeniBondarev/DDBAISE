using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostSity.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] 
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Middlename { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Surname { get; set; }

        public int PositionId { get; set; }

        public int OfficeId { get; set; }

        [ForeignKey("OfficeId")] 
        public Office Office { get; set; }

        [ForeignKey("PositionId")] 
        public EmployeePosition Position { get; set; }
    }
}
