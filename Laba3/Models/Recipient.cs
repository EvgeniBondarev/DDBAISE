﻿using System;
using System.Collections.Generic;

namespace Laba3;

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

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public override string ToString()
    {
        return $"{Surname} {Name} {Middlename}";
    }
}