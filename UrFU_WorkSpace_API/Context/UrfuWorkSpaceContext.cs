using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Context;

public partial class UrfuWorkSpaceContext : DbContext
{
    public UrfuWorkSpaceContext()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public UrfuWorkSpaceContext(DbContextOptions<UrfuWorkSpaceContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WorkspaceAmenity> WorkspaceAmenities { get; set; }
    public DbSet<WorkspaceObject> WorkspaceObjects { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceWeekday> OperationMode { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>()
            .HasDiscriminator(t => t.TypeOwner)
            .HasValue<Image>(-1)
            .HasValue<WorkspaceImage>((int)OwnerType.Workspace)
            .HasValue<TemplateImage>((int)OwnerType.Template);

        modelBuilder.Entity<WorkspaceObject>()
            .HasOne(wo => wo.Template)
            .WithMany()
            .HasForeignKey(wo => wo.IdTemplate);
        

        modelBuilder.Entity<Template>()
            .HasOne(e => e.Image)
            .WithOne()
            .HasForeignKey<TemplateImage>(e => e.IdOwner)
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
            .HasForeignKey(e => e.IdWorkspace)
            .IsRequired();

        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.OperationMode).WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.IdWorkspace)
            .IsRequired();

        modelBuilder.Entity<Workspace>()
            .HasMany(e => e.Objects).WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(e => e.IdWorkspace)
            .IsRequired();


        modelBuilder.Entity<Template>().Navigation(e => e.Image).AutoInclude();
        modelBuilder.Entity<WorkspaceAmenity>().Navigation(e => e.Template).AutoInclude();
        modelBuilder.Entity<WorkspaceObject>().Navigation(e => e.Template).AutoInclude();
        modelBuilder.Entity<Workspace>().Navigation(e => e.Images).AutoInclude();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}