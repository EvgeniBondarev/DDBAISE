using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Laba4.Models;

public partial class PublicationType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
