using System.ComponentModel.DataAnnotations;


namespace Domains.Models
{
    public class Employee : IUser
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
        public Office? Office { get; set; }
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
