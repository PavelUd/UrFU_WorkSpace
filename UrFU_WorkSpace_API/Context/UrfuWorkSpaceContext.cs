using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API.Context;

public partial class UrfuWorkSpaceContext : DbContext
{
    public UrfuWorkSpaceContext()
    {
    }

    public UrfuWorkSpaceContext(DbContextOptions<UrfuWorkSpaceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<AmenityDetail> AmenityDetails { get; set; }
    public DbSet<WorkspaceAmenity> WorkspaceAmenities { get; set; }
    
    public DbSet<WorkspaceImage> WorkspaceImages { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<WorkspaceWeekday> OperationMode { get; set; }
    public DbSet<WorkspaceObject> WorkspaceObjects { get; set; }
}
