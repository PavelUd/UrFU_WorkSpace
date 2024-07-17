using System;
using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Context;

public partial class UrfuWorkSpaceContext : DbContext
{
    public UrfuWorkSpaceContext()
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public UrfuWorkSpaceContext(DbContextOptions<UrfuWorkSpaceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>()
            .HasDiscriminator(t => t.TypeOwner)
            .HasValue<Image>(-1)
            .HasValue<WorkspaceImage>((int)OwnerType.Workspace)
            .HasValue<ObjectImage>((int)OwnerType.Object)
            .HasValue<AmenityImage>((int)OwnerType.Amenity);
        
        modelBuilder.Entity<WorkspaceObject>()
            .HasOne(wo => wo.Template)
            .WithMany()
            .HasForeignKey(wo => wo.IdTemplate);

        modelBuilder.Entity<ObjectTemplate>()
            .HasOne(e => e.Image)
            .WithOne()
            .HasForeignKey<ObjectImage>(e => e.IdOwner)
            .IsRequired(); 
        
        modelBuilder.Entity<AmenityTemplate>()
            .HasOne(e => e.Image)
            .WithOne()
            .HasForeignKey<AmenityImage>(e => e.IdOwner)
            .IsRequired();
            
        
        modelBuilder.Entity<WorkspaceAmenity>()
            .HasOne(wo => wo.Template)
            .WithMany()
            .HasForeignKey(wo => wo.IdTemplate);
       
        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.Images).WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.IdOwner)
            .IsRequired();
         
        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.Amenities).WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e =>  e.IdWorkspace)
            .IsRequired();
        
        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.OperationMode).WithOne()
            .HasPrincipalKey(e =>  e.Id)
            .HasForeignKey(e => e.IdWorkspace)
            .IsRequired();
        
        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.Objects).WithOne()
            .HasPrincipalKey(e =>  e.Id)
            .HasForeignKey(e => e.IdWorkspace)
            .IsRequired();
        
        
        modelBuilder.Entity<AmenityTemplate>().Navigation(e=> e.Image).AutoInclude();
        modelBuilder.Entity<ObjectTemplate>().Navigation(e=> e.Image).AutoInclude();
        modelBuilder.Entity<WorkspaceAmenity>().Navigation(e => e.Template).AutoInclude();
        modelBuilder.Entity<WorkspaceObject>().Navigation(e => e.Template).AutoInclude();
        modelBuilder.Entity<Workspace>().Navigation(e => e.Objects).AutoInclude();

        modelBuilder.Entity<Workspace>().Navigation(e => e.Amenities).AutoInclude();
        modelBuilder.Entity<Workspace>().Navigation(e => e.OperationMode).AutoInclude();
        modelBuilder.Entity<Workspace>().Navigation(e => e.Images).AutoInclude();
        modelBuilder.Entity<VerificationCode>();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AmenityTemplate> AmenityTemplates { get; set; }
    public DbSet<WorkspaceAmenity> WorkspaceAmenities { get; set; }
    public DbSet<WorkspaceObject> WorkspaceObjects { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceWeekday> OperationMode { get; set; }
    public DbSet<ObjectTemplate> ObjectTemplates { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
}
