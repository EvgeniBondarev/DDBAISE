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

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<OfficeView> OfficeViews { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<PublicationType> PublicationTypes { get; set; }

    public virtual DbSet<PublicationView> PublicationViews { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<RecipientAddress> RecipientAddresses { get; set; }

    public virtual DbSet<RecipientView> RecipientViews { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionView> SubscriptionViews { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FBA716D56");

            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Middlename)
                .HasMaxLength(20)
                .HasColumnName("middlename");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname");

            entity.HasOne(d => d.Office).WithMany(p => p.Employees)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__office__49C3F6B7");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__positi__4AB81AF0");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F167E0439");

            entity.ToTable("EmployeePosition");

            entity.HasIndex(e => e.Position, "UQ__Employee__75FE9D9930B8A4EF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Office__3213E83F15657E01");

            entity.ToTable("Office");

            entity.HasIndex(e => e.MobilePhone, "UQ__Office__3867605B3F8D03F9").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Office__AB6E6164EC42B120").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(20)
                .HasColumnName("mobile_phone");
            entity.Property(e => e.OnwnerSurname)
                .HasMaxLength(20)
                .HasColumnName("onwner_surname");
            entity.Property(e => e.OwnerMiddlename)
                .HasMaxLength(20)
                .HasColumnName("owner_middlename");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(20)
                .HasColumnName("owner_name");
            entity.Property(e => e.StreetName)
                .HasMaxLength(50)
                .HasColumnName("street_name");
        });

        modelBuilder.Entity<OfficeView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OfficeView");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MobilePhone).HasMaxLength(20);
            entity.Property(e => e.OfficeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("OfficeID");
            entity.Property(e => e.OwnerMiddlename).HasMaxLength(20);
            entity.Property(e => e.OwnerName).HasMaxLength(20);
            entity.Property(e => e.OwnerSurname).HasMaxLength(20);
            entity.Property(e => e.StreetName).HasMaxLength(50);
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publicat__3213E83FBFFED7F5");

            entity.ToTable("Publication");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(70)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Publications)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Publicati__type___3E52440B");
        });

        modelBuilder.Entity<PublicationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publicat__3213E83FF4660137");

            entity.ToTable("PublicationType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");
        });

        modelBuilder.Entity<PublicationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("PublicationView");

            entity.Property(e => e.PublicationId).HasColumnName("PublicationID");
            entity.Property(e => e.PublicationName).HasMaxLength(70);
            entity.Property(e => e.PublicationPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublicationType).HasMaxLength(20);
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipien__3213E83F8B16D66F");

            entity.ToTable("Recipient");

            entity.HasIndex(e => e.MobilePhone, "UQ__Recipien__3867605B8CDDE220").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Recipien__AB6E6164F25EBCDE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Middlename)
                .HasMaxLength(20)
                .HasColumnName("middlename");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(20)
                .HasColumnName("mobile_phone");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname");

            entity.HasOne(d => d.Address).WithMany(p => p.Recipients)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipient__addre__4316F928");
        });

        modelBuilder.Entity<RecipientAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipien__3213E83FEFE7DA13");

            entity.ToTable("RecipientAddress");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apartment).HasColumnName("apartment");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");
        });

        modelBuilder.Entity<RecipientView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RecipientView");

            entity.Property(e => e.RecipientEmail).HasMaxLength(255);
            entity.Property(e => e.RecipientId).HasColumnName("RecipientID");
            entity.Property(e => e.RecipientMiddlename).HasMaxLength(20);
            entity.Property(e => e.RecipientMobilePhone).HasMaxLength(20);
            entity.Property(e => e.RecipientName).HasMaxLength(20);
            entity.Property(e => e.RecipientStreet).HasMaxLength(50);
            entity.Property(e => e.RecipientSurname).HasMaxLength(20);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3213E83FEB906352");

            entity.ToTable("Subscription");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.OfficeId).HasColumnName("office_id");
            entity.Property(e => e.PublicationId).HasColumnName("publication_id");
            entity.Property(e => e.RecipientId).HasColumnName("recipient_id");
            entity.Property(e => e.SubscriptionStartDate)
                .HasMaxLength(7)
                .HasColumnName("subscription_start_date");

            entity.HasOne(d => d.Office).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__offic__4D94879B");

            entity.HasOne(d => d.Publication).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PublicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__publi__4E88ABD4");

            entity.HasOne(d => d.Recipient).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__recip__4F7CD00D");
        });

        modelBuilder.Entity<SubscriptionView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SubscriptionView");

            entity.Property(e => e.OfficeOwnerName).HasMaxLength(20);
            entity.Property(e => e.PublicationName).HasMaxLength(70);
            entity.Property(e => e.RecipientName).HasMaxLength(20);
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.SubscriptionStartDate).HasMaxLength(7);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
