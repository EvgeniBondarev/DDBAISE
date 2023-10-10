using System;
using System.Collections.Generic;

namespace Lab2;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Middlename { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int PositionId { get; set; }

    public int OfficeId { get; set; }

    public virtual Office Office { get; set; } = null!;

    public virtual EmployeePosition Position { get; set; } = null!;
}
