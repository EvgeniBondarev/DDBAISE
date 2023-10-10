using System;
using System.Collections.Generic;

namespace Lab2;

public partial class Office
{
    public int Id { get; set; }

    public string OwnerName { get; set; } = null!;

    public string OwnerMiddlename { get; set; } = null!;

    public string OnwnerSurname { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string MobilePhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
