using System;
using System.Collections.Generic;

namespace Laba3;

public partial class Subscription
{
    public int Id { get; set; }

    public int RecipientId { get; set; }

    public int PublicationId { get; set; }

    public int Duration { get; set; }

    public int OfficeId { get; set; }

    public string SubscriptionStartDate { get; set; } = null!;

    public virtual Office Office { get; set; } = null!;

    public virtual Publication Publication { get; set; } = null!;

    public virtual Recipient Recipient { get; set; } = null!;
}
