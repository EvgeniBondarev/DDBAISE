using System;
using System.Collections.Generic;

namespace Lab2;

public partial class SubscriptionView
{
    public int SubscriptionId { get; set; }

    public string RecipientName { get; set; } = null!;

    public string PublicationName { get; set; } = null!;

    public int SubscriptionDuration { get; set; }

    public string OfficeOwnerName { get; set; } = null!;

    public string SubscriptionStartDate { get; set; } = null!;
}
