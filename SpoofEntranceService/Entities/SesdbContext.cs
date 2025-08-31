using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpoofEntranceService.Entities;

public partial class SesdbContext : DbContext
{
    public SesdbContext()
    {
    }

    public SesdbContext(DbContextOptions<SesdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<UserEntry> UserEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=SESDB;TrustServerCertificate=True;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Token_Id");

            entity.ToTable("Token");

            entity.Property(e => e.Token1)
                .HasMaxLength(258)
                .HasColumnName("Token");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("UserEntry");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(512);
            entity.Property(e => e.Salt).HasMaxLength(512);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
