using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HCM.API.molelzz
{
    public partial class _HCMContext : DbContext
    {
        public _HCMContext()
        {
        }

        public _HCMContext(DbContextOptions<_HCMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
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
        public virtual DbSet<Priority> Priorities { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<Seniority> Seniorities { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=_HCM;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLog");

                entity.Property(e => e.Action)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Changes).IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.EntityName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.AuditLogs)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__AuditLog__Employ__6383C8BA");
            });

            modelBuilder.Entity<Bonuse>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Bonuses)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Bonuses__Employe__4E88ABD4");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.BonusesNavigation)
                    .HasForeignKey(d => d.PayrollId)
                    .HasConstraintName("FK__Bonuses__Payroll__5070F446");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Bonuses)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK__Bonuses__ReasonI__4F7CD00D");
            });

            modelBuilder.Entity<BonusesReason>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Countrie__737584F636E4A7EA")
                    .IsUnique();

                entity.HasIndex(e => e.Iso, "UQ__Countrie__C4979A23A6FD8200")
                    .IsUnique();

                entity.Property(e => e.Iso)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ISO");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxRate).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Deduction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Deductions)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Deduction__Emplo__5535A963");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.DeductionsNavigation)
                    .HasForeignKey(d => d.PayrollId)
                    .HasConstraintName("FK__Deduction__Payro__571DF1D5");

                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.Deductions)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK__Deduction__Reaso__5629CD9C");
            });

            modelBuilder.Entity<DeductionReason>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Departme__737584F687F532FD")
                    .IsUnique();

                entity.Property(e => e.ImageUrl)
                    .IsUnicode(false)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK__Departmen__Count__2E1BDC42");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E4CA9E51E4")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Employee__A9D1053463A81FA8")
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
                    .HasConstraintName("FK__Employees__Depar__3E52440B");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK__Employees__Gende__3D5E1FD2");

                entity.HasOne(d => d.Nationality)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.NationalityId)
                    .HasConstraintName("FK__Employees__Natio__3C69FB99");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__Employees__Posit__3F466844");

                entity.HasOne(d => d.Seniority)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SeniorityId)
                    .HasConstraintName("FK__Employees__Senio__403A8C7D");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__RoleI__440B1D61"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__EmployeeR__Emplo__4316F928"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "RoleId").HasName("PK__Employee__C27FE3F0BDF91DAC");

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

                entity.Property(e => e.Bonuses).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Deductions).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.GrossPay).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.NetPay).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Payrolls)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payroll__Employe__49C3F6B7");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Position__737584F6792F8FBE")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Positions__Depar__31EC6D26");
            });

            modelBuilder.Entity<PositionSeniority>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PositionSeniority");

                entity.HasOne(d => d.Position)
                    .WithMany()
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK__PositionS__Posit__36B12243");

                entity.HasOne(d => d.Seniority)
                    .WithMany()
                    .HasForeignKey(d => d.SeniorityId)
                    .HasConstraintName("FK__PositionS__Senio__37A5467C");
            });

            modelBuilder.Entity<Priority>(entity =>
            {
                entity.ToTable("Priority");

                entity.Property(e => e.PriorityName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Roles__737584F6668AE0F7")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Salaries__7AD04FF1A7011CDD");

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
                    .HasConstraintName("FK__Salaries__Employ__46E78A0C");
            });

            modelBuilder.Entity<Seniority>(entity =>
            {
                entity.ToTable("Seniority");

                entity.HasIndex(e => e.Name, "UQ__Seniorit__737584F6BF327FD8")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.IssuerId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TaskName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TaskEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Tasks__EmployeeI__5FB337D6");

                entity.HasOne(d => d.Issuer)
                    .WithMany(p => p.TaskIssuers)
                    .HasForeignKey(d => d.IssuerId)
                    .HasConstraintName("FK__Tasks__IssuerId__60A75C0F");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("FK__Tasks__PriorityI__5DCAEF64");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Tasks__StatusID__5EBF139D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
