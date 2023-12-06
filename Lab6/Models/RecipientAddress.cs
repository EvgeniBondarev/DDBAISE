using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Laba4.Models;

public partial class RecipientAddress
{
    public int Id { get; set; }

    public string? Street { get; set; }

    public int? House { get; set; }

    public int? Apartment { get; set; }
    [JsonIgnore]
    public virtual ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
}
