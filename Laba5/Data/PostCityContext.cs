using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PostCity.Models;

namespace Laba4.Models;

public partial class PostCityContext : IdentityDbContext<PostCityUser>
{
    public PostCityContext()
    {
    }

    public PostCityContext(DbContextOptions<PostCityContext> options)
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
    
    public virtual DbSet<PostCityUser> PostCityUsers { get; set; }

    public PostCityUser FindUserByUserId(int userId)
    {
        return Users.FirstOrDefault(u => u.UserId == userId);
    }
}
