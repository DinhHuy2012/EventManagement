using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Models;

public partial class Asm3EventManagementContext : DbContext
{
    public Asm3EventManagementContext()
    {
    }

    public Asm3EventManagementContext(DbContextOptions<Asm3EventManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendee> Attendees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			string ConnectionStr = config.GetConnectionString("DB");
			optionsBuilder.UseSqlServer(ConnectionStr);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.HasKey(e => e.AttendeeId).HasName("PK__Attendee__184401285E73425F");

            entity.Property(e => e.AttendeeId).HasColumnName("AttendeeID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.RegistrationTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event).WithMany(p => p.Attendees)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK__Attendees__Event__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.Attendees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Attendees__UserI__412EB0B6");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C8700FBA2950");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Image).HasColumnType("text");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Events__Category__3C69FB99");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .HasConstraintName("FK__Events__Organize__3D5E1FD2");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__EventCat__19093A2BE2C1A18F");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC7FCEBB0C");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4B91AEAA9").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053453A222CA").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Avatar).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fullname).HasMaxLength(255).HasDefaultValue("NoName");
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20).HasDefaultValue("NoNumber");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
