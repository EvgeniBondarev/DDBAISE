using System;
using System.Collections.Generic;

namespace Lab2;

public partial class Publication
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual PublicationType Type { get; set; } = null!;
}
