﻿using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class Subscription
{
    public int Id { get; set; }
    public int PublicationId { get; set; }
    public int Duration { get; set; }
    public string SubscriptionStartDate { get; set; } = null!;

    public Publication Publication { get; set; } = null!;

}
