using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class RecipientAddress
{
    public int Id { get; set; }

    public string? Street { get; set; }

    public int? House { get; set; }

    public int? Apartment { get; set; }

    public virtual ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
}
