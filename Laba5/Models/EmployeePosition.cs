using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class EmployeePosition
{
    public int Id { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
