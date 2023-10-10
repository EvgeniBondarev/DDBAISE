using System;
using System.Collections.Generic;

namespace Lab2;

public partial class OfficeView
{
    public int OfficeId { get; set; }

    public string OwnerName { get; set; } = null!;

    public string OwnerMiddlename { get; set; } = null!;

    public string OwnerSurname { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public string MobilePhone { get; set; } = null!;

    public string Email { get; set; } = null!;
}
