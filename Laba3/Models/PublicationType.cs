﻿using System;
using System.Collections.Generic;

namespace Laba3;

public partial class PublicationType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
