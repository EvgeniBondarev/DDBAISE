using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laba4.Models;

public partial class SubsCityContext : IdentityDbContext
{
    public SubsCityContext()
    {
    }

    public SubsCityContext(DbContextOptions<SubsCityContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<PublicationType> PublicationTypes { get; set; }


    public virtual DbSet<Subscription> Subscriptions { get; set; }
}
