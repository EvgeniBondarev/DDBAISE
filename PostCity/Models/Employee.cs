using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PostCity.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Name { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Middlename { get; set; }

        [Required]
        [MaxLength(50)] 
        public string Surname { get; set; }

        public int PositionId { get; set; }

        public int OfficeId { get; set; }

        [ForeignKey("OfficeId")] 
        public Office? Office { get; set; }

        [ForeignKey("PositionId")] 
        public EmployeePosition? Position { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name}";
        }
        public string FullName
        {
            get { return $"{Surname} {Name} {Middlename}"; }
        }

    }
}

