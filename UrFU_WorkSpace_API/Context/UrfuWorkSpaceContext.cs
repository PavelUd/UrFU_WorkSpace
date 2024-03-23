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
    public DbSet<User> Admins { get; set; }
}
