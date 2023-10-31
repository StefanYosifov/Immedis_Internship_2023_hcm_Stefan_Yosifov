using Microsoft.EntityFrameworkCore;

namespace HCM.Data
{
    using History_and_Audit;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Models;
    using System.Security.Claims;
    using System.Text;
    using Task = Models.Task;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HCM;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeRoles>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.RoleId,
                    k.EmployeeId
                });
            });


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
                    .HasConstraintName("FK__AuditLog__Employ__628FA481");
            });

            modelBuilder.Entity<Bonuse>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Bonuses)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Bonuses__Employe__4E88ABD4");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.Bonuses)
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
                entity.HasIndex(e => e.Name, "UQ__Countrie__737584F6F093C58F")
                    .IsUnique();

                entity.HasIndex(e => e.Iso, "UQ__Countrie__C4979A23F94C38A1")
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
                entity.HasIndex(e => e.Name, "UQ__Departme__737584F63994BC10")
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
                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E49A202DC1")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Employee__A9D10534B0647F87")
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

                entity.HasMany(d => d.EmployeeRoles)
                    .WithOne(er => er.Employee);
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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Deductions).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.NetPay).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Payrolls)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Payroll__Employe__49C3F6B7");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Position__737584F6E748174C")
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

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PriorityName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Roles__737584F666D3D7C5")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Salaries__7AD04FF1C6629CBB");

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

                entity.HasIndex(e => e.Name, "UQ__Seniorit__737584F65FD7EA16")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(450)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.PriorityId).HasColumnName("PriorityID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.TaskName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Tasks__EmployeeI__5FB337D6");

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

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            AuditSave();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        private void AuditSave()
        {
            var currentTime = DateTime.UtcNow;
            var userName = ClaimTypes.Name;


            var modifiedEntities = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToArray();


            foreach (var modifiedEntity in modifiedEntities)
            {
                var auditLog = new AuditLog
                {
                    EntityName = modifiedEntity.Entity.GetType().Name,
                    Action = modifiedEntity.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Changes = GetChanges(modifiedEntity)
                };

                var id = modifiedEntity.OriginalValues.Properties[0];


                this.AuditLogs.Add(auditLog);

                if (modifiedEntity.Entity is IEntity entity)
                {
                    if (modifiedEntity.State == EntityState.Added)
                    {
                        entity.CreatedOn = currentTime;
                        entity.CreatedBy = userName;
                    }
                    else if (modifiedEntity.State == EntityState.Modified)
                    {
                        entity.ModifiedOn = currentTime;
                        entity.ModifiedBy = userName!;
                    }
                }
            }
        }

        private static string GetChanges(EntityEntry entity)
        {
            var changes = new StringBuilder();
            foreach (var property in entity.OriginalValues.Properties)
            {
                var originalValue = entity.OriginalValues[property] ?? null;
                var currentValue = entity.CurrentValues[property] ?? "null";
                if (!Equals(originalValue, currentValue))
                {
                    changes.AppendLine($"{property.Name}: From '{originalValue}' to '{currentValue}'");
                }
            }
            return changes.ToString();
        }
    }
}
