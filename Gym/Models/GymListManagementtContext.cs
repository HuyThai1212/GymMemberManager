using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Gym.Models;

public partial class GymListManagementtContext : DbContext
{
    public GymListManagementtContext()
    {
    }

    public GymListManagementtContext(DbContextOptions<GymListManagementtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MembershipPackage> MembershipPackages { get; set; }

    public virtual DbSet<PersonalTrainerPackage> PersonalTrainerPackages { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("value"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__0CF04B38399D2CD7");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.JoinDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MembershipPackageId).HasColumnName("MembershipPackageID");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PtpackageId).HasColumnName("PTPackageID");

            entity.HasOne(d => d.MembershipPackage).WithMany(p => p.Members)
                .HasForeignKey(d => d.MembershipPackageId)
                .HasConstraintName("FK__Members__Members__3E52440B");

            entity.HasOne(d => d.Ptpackage).WithMany(p => p.Members)
                .HasForeignKey(d => d.PtpackageId)
                .HasConstraintName("FK__Members__PTPacka__3F466844");
        });

        modelBuilder.Entity<MembershipPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Membersh__322035EC3AB5BFFB");

            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PackageName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PersonalTrainerPackage>(entity =>
        {
            entity.HasKey(e => e.PtpackageId).HasName("PK__Personal__20FF2154B783B919");

            entity.Property(e => e.PtpackageId).HasColumnName("PTPackageID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Ptname)
                .HasMaxLength(100)
                .HasColumnName("PTName");
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CC4CD569FA78");

            entity.ToTable("UserAccount");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
