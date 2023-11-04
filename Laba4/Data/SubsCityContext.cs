using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Laba4.Models;

public partial class SubsCityContext : DbContext
{
    public SubsCityContext()
    {
    }

    public SubsCityContext(DbContextOptions<SubsCityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<PublicationType> PublicationTypes { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<RecipientAddress> RecipientAddresses { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }
}
