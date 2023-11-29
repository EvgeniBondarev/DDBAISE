using Laba4.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostCity.Models
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

        [ForeignKey("OfficeId")]
        public Office? Office { get; set; }

        [ForeignKey("PositionId")]
        public EmployeePosition? Position { get; set; }

        [InverseProperty("Employee")]
        [CascadeDelete]
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

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
