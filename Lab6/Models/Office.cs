using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Laba4.Models;

public partial class Office
{
    public int Id { get; set; }

    public string OwnerName { get; set; } = null!;

    public string OwnerMiddlename { get; set; } = null!;

    public string OwnerSurname { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string MobilePhone { get; set; } = null!;

    public string Email { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    [JsonIgnore]
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
