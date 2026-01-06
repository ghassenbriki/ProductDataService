using System;
using System.Collections.Generic;
using Leoni.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Leoni.Persistence;

public partial class LeoniDbContext : DbContext
{
    public LeoniDbContext()
    {
    }

    public LeoniDbContext(DbContextOptions<LeoniDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tasks> Tasks { get; set; }

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ComponentName).HasName("PK__Componen__DB06D1C0374B2836");

            entity.ToTable("Component");

            entity.Property(e => e.ComponentName).HasMaxLength(150);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Employee__23DB122B8E84CB5A");

            entity.ToTable("Employee");

            entity.HasMany(e => e.RefreshTokens).WithOne(r => r.Employee)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);


            entity.Property(e => e.SessionId)
                .HasMaxLength(100)
                .HasColumnName("sessionId");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("lastName");

            entity.HasMany(d => d.Roles).WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EmployeeR__RoleI__36B12243"),
                    l => l.HasOne<Employee>().WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__EmployeeR__Emplo__35BCFE0A"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "RoleId").HasName("PK__Employee__C27FE3F06CCDFC70");
                        j.ToTable("EmployeeRole");
                        j.IndexerProperty<string>("EmployeeId").HasMaxLength(100);
                    });
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2F002B00F2");

            entity.ToTable("Permission");

            entity.Property(e => e.PermissionName).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A67E155C5");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(100);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                // in case there is no extra field in the resulting table (just composite key fields)
                // ef core does not create the model RolePermission explicitly as a class

                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__Permi__3A81B327"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolePermi__RoleI__398D8EEE"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PK__RolePerm__6400A1A808124B35");
                        j.ToTable("RolePermission");
                    });
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC074BFE197F");

            entity.ToTable("Status");

            entity.Property(e => e.Libelle).HasMaxLength(100);
        });

        modelBuilder.Entity<Tasks>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949B1B52E1B57");

            entity.Property(e => e.ActivePart).HasMaxLength(100);
            entity.Property(e => e.CompletedMinutesSmv).HasColumnName("CompletedMinutesSMV");
            entity.Property(e => e.ComponentGroup).HasMaxLength(100);
            entity.Property(e => e.CustomerPn)
                .HasMaxLength(100)
                .HasColumnName("CustomerPN");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DevExXmllink)
                .HasMaxLength(255)
                .HasColumnName("DevExXMLLink");
            entity.Property(e => e.DocumentComments).HasMaxLength(500);
            entity.Property(e => e.FailureCategory).HasMaxLength(100);
            entity.Property(e => e.FailureDetails).HasMaxLength(500);
            entity.Property(e => e.IBomxmllink)
                .HasMaxLength(255)
                .HasColumnName("iBOMXMLLink");
            entity.Property(e => e.InternalStatus).HasMaxLength(50);
            entity.Property(e => e.LeoniPn)
                .HasMaxLength(50)
                .HasColumnName("LeoniPN");
            entity.Property(e => e.PartnerDrawing).HasMaxLength(255);
            entity.Property(e => e.ReleasedPartNumber).HasMaxLength(100);
            entity.Property(e => e.ResponsibleComponentEngineerId).HasMaxLength(100);
            entity.Property(e => e.TerminalReelDirectionWireSizes).HasMaxLength(255);
            entity.Property(e => e.TimeSmv).HasColumnName("TimeSMV");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.ResponsibleComponentEngineer).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ResponsibleComponentEngineerId)
                .HasConstraintName("FK_Tasks_Employee");

            entity.HasOne(d => d.AssignedBy).WithMany(p => p.TasksAssigned)
            .HasForeignKey(d => d.AssignedByFk)
            .HasConstraintName("FK_Tasks_AssignedBy");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Status");

            entity.HasOne(d => d.Component).WithMany(c => c.Tasks)
            .HasForeignKey(d => d.ComponentName)
            .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
