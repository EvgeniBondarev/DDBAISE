using System;
using System.Collections.Generic;

namespace Laba4.Models;

public partial class PublicationType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
