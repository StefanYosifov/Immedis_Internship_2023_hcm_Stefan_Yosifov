using System;
using System.Collections.Generic;
using HCM.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCM.Data
{
    using System.Security.Claims;

    using History_and_Audit;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bonuse> Bonuses { get; set; } = null!;
        public virtual DbSet<BonusesReason> BonusesReasons { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Deduction> Deductions { get; set; } = null!;
        public virtual DbSet<DeductionReason> DeductionReasons { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Payroll> Payrolls { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<PositionSeniority> PositionSeniorities { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<Seniority> Seniorities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=_HCM;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bonuse>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Bonuses)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Bonuses__Employe__4CA06362");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Bonuses)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK__Bonuses__ReasonI__4D94879B");
            });

            modelBuilder.Entity<BonusesReason>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Countrie__737584F67BEBB877")
                    .IsUnique();

                entity.HasIndex(e => e.Iso, "UQ__Countrie__C4979A2326001EEB")
                    .IsUnique();

                entity.Property(e => e.Iso)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ISO");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Deduction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Deductions)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Deduction__Emplo__52593CB8");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Deductions)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK__Deduction__Reaso__534D60F1");
            });

            modelBuilder.Entity<DeductionReason>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Departme__737584F67BB7840D")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E412B8D60E")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Employee__A9D10534D67152A7")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Employees__Depar__3C69FB99");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK__Employees__Gende__3B75D760");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.NationalityId)
                    .HasConstraintName("FK__Employees__Natio__3A81B327");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__Employees__Posit__3D5E1FD2");

                entity.HasOne(d => d.Seniority)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SeniorityId)
                    .HasConstraintName("FK__Employees__Senio__3E52440B");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__RoleI__4222D4EF"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__Emplo__412EB0B6"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "RoleId").HasName("PK__Employee__C27FE3F02E3555AF");

                            j.ToTable("EmployeeRoles");

                            j.IndexerProperty<string>("EmployeeId").HasMaxLength(450).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.ToTable("Payroll");

                entity.Property(e => e.PayrollId)
                    .ValueGeneratedNever()
                    .HasColumnName("PayrollID");

                entity.Property(e => e.Bonuses).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Deductions).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.PayrollDate).HasColumnType("date");

                entity.Property(e => e.SalaryAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Payrolls)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Payroll__Employe__47DBAE45");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Position__737584F6779EDA1A")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Positions__Depar__30F848ED");
            });

            modelBuilder.Entity<PositionSeniority>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PositionSeniority");

                entity.HasOne(d => d.Position)
                    .WithMany()
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__PositionS__Posit__6D0D32F4");

                entity.HasOne(d => d.Seniority)
                    .WithMany()
                    .HasForeignKey(d => d.SeniorityId)
                    .HasConstraintName("FK__PositionS__Senio__6E01572D");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Roles__737584F616672FD9")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Salaries__7AD04FF1643206C2");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.SalaryAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.Salary)
                    .HasForeignKey<Salary>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Salaries__Employ__44FF419A");
            });

            modelBuilder.Entity<Seniority>(entity =>
            {
                entity.ToTable("Seniority");

                entity.HasIndex(e => e.Name, "UQ__Seniorit__737584F66551E8DE")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AuditLog();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void AuditLog()
        {
            var today = DateTime.UtcNow;
            var creatorUserName = ClaimTypes.Name;

            foreach (var item in ChangeTracker.Entries().Where(e => e.Entity is IEntity))
            {
                if (item.Entity is IEntity entity)
                {
                    if (item.State == EntityState.Added)
                    {
                        entity.CreatedOn = today;
                        entity.CreatedBy = creatorUserName;
                    }
                    else if (item.State == EntityState.Modified)
                    {
                        entity.ModifiedOn = today;
                        entity.ModifiedBy = creatorUserName;
                    }
                }
            }
        }
    }
}
