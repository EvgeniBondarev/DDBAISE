using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Middlename { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int PositionId { get; set; }

    public int OfficeId { get; set; }

    public Office Office { get; set; } = null!;

    public EmployeePosition Position { get; set; } = null!;
}
