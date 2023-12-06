using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Laba4.Models;

public partial class Recipient
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Middlename { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int AddressId { get; set; }

    public string MobilePhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual RecipientAddress Address { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
