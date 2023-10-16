using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Human_Capital_Managment.Data.Models
{
    public partial class HumanCapitalManagementContext : DbContext
    {
        public HumanCapitalManagementContext()
        {
        }

        public HumanCapitalManagementContext(DbContextOptions<HumanCapitalManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<ContractAction> ContractActions { get; set; } = null!;
        public virtual DbSet<ContractHistory> ContractHistories { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; } = null!;
        public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; } = null!;
        public virtual DbSet<Seniority> Seniorities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HumanCapitalManagement;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Contracts_Employees");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Contract)
                    .HasPrincipalKey<Salary>(p => p.ContractId)
                    .HasForeignKey<Contract>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contracts_Salaries");
            });

            modelBuilder.Entity<ContractAction>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Contract__737584F6A9F1F9ED")
                    .IsUnique();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<ContractHistory>(entity =>
            {
                entity.ToTable("ContractHistory");

                entity.Property(e => e.ChangedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.ContractHistories)
                    .HasForeignKey(d => d.ActionId)
                    .HasConstraintName("FK__ContractH__Actio__1209AD79");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractHistories)
                    .HasForeignKey(d => d.ContractId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ContractHistory_Contracts");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Iso)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ISO")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.DepartmentName, "UQ__Departme__D949CC3442D74B08")
                    .IsUnique();

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__Departmen__Posit__5165187F");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Employee__A9D1053479DE1180")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeDetailsId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ManagerId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Employees__Depar__29572725");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Employee Details");

                entity.HasOne(d => d.Id1)
                    .WithOne(p => p.EmployeeId1)
                    .HasPrincipalKey<Project>(p => p.ManagerId)
                    .HasForeignKey<Employee>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Projects");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK_Employee_Manager");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Employees_Payments");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Employees_Positions");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.EmployeeProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Employees_Projects1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Employees__Statu__440B1D61");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__RoleI__3C69FB99"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__Emplo__3B75D760"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "RoleId").HasName("PK__Employee__C27FE3F0E2CB17A3");

                            j.ToTable("EmployeeRoles");

                            j.IndexerProperty<string>("EmployeeId").HasMaxLength(256).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<EmployeeDetail>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__3214EC07FDB28C97");

                entity.ToTable("Employee Details");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CountryOfBirth)
                    .WithMany(p => p.EmployeeDetailCountryOfBirths)
                    .HasForeignKey(d => d.CountryOfBirthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee Details_Countries");

                entity.HasOne(d => d.CountryOfResidence)
                    .WithMany(p => p.EmployeeDetailCountryOfResidences)
                    .HasForeignKey(d => d.CountryOfResidenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee Details_Countries1");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.EmployeeDetails)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Employee Details_Gender");
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.ToTable("Employee Status");

                entity.HasIndex(e => e.Name, "UQ__Status__737584F6C520FFB4")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(d => d.Seniorities)
                    .WithMany(p => p.Positions)
                    .UsingEntity<Dictionary<string, object>>(
                        "PositionSeniority",
                        l => l.HasOne<Seniority>().WithMany().HasForeignKey("SeniorityId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Position Seniority_Seniority"),
                        r => r.HasOne<Position>().WithMany().HasForeignKey("PositionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Position Seniority_Positions"),
                        j =>
                        {
                            j.HasKey("PositionId", "SeniorityId").HasName("PK__Position__3214EC272426F135");

                            j.ToTable("Position Seniority");

                            j.IndexerProperty<int>("PositionId").ValueGeneratedOnAdd();
                        });
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.ManagerId, "UQ__Projects__3BA2AAE0991F1BDD")
                    .IsUnique();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ManagerId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasIndex(e => e.ContractId, "FK_ContractId")
                    .IsUnique();

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Bonus).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ContractId)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalaryPayment>(entity =>
            {
                entity.ToTable("Salary Payments");

                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reason).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Seniority>(entity =>
            {
                entity.ToTable("Seniority");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
