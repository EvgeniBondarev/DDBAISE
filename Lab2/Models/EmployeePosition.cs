using System;
using System.Collections.Generic;

namespace Lab2;

public partial class EmployeePosition
{
    public int Id { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
