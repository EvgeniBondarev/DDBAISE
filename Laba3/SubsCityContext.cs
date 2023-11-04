using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Laba3;

public partial class SubsCityContext : DbContext
{
    public SubsCityContext()
    {
    }

    public SubsCityContext(DbContextOptions<SubsCityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePosition { get; set; }

    public virtual DbSet<Office> Office { get; set; }

    public virtual DbSet<OfficeView> OfficeView { get; set; }

    public virtual DbSet<Publication> Publication { get; set; }

    public virtual DbSet<PublicationType> PublicationType { get; set; }

    public virtual DbSet<PublicationView> PublicationView { get; set; }

    public virtual DbSet<Recipient> Recipient { get; set; }

    public virtual DbSet<RecipientAddress> RecipientAddress { get; set; }

    public virtual DbSet<RecipientView> RecipientView { get; set; }

    public virtual DbSet<Subscription> Subscription { get; set; }

    public virtual DbSet<SubscriptionView> SubscriptionView { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
