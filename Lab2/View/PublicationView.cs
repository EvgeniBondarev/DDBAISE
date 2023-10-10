using System;
using System.Collections.Generic;

namespace Lab2;

public partial class PublicationView
{
    public int PublicationId { get; set; }

    public string PublicationType { get; set; } = null!;

    public string PublicationName { get; set; } = null!;

    public decimal PublicationPrice { get; set; }
}
