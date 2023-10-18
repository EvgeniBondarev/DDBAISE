using System;
using System.Collections.Generic;

namespace Laba3;

public partial class RecipientView
{
    public int RecipientId { get; set; }

    public string RecipientName { get; set; } = null!;

    public string RecipientMiddlename { get; set; } = null!;

    public string RecipientSurname { get; set; } = null!;

    public string? RecipientStreet { get; set; }

    public int? RecipientHouse { get; set; }

    public int? RecipientApartment { get; set; }

    public string RecipientMobilePhone { get; set; } = null!;

    public string RecipientEmail { get; set; } = null!;
}
