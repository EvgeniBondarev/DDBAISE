﻿using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public int RecipientId { get; set; }

    public int PublicationId { get; set; }

    public int Duration { get; set; }

    public int OfficeId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime SubscriptionStartDate { get; set; }

    //public Office Office { get; set; } = null!;

    public Publication? Publication { get; set; } = null!;

    //public Recipient Recipient { get; set; } = null!;

    public Employee? Employee { get; set; } = null;


}
