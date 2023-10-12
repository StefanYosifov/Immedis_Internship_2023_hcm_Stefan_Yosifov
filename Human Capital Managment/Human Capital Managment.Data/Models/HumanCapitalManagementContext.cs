using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Salary> Salaries { get; set; } = null!;
        public virtual DbSet<Seniority> Seniorities { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;

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

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

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
                entity.HasIndex(e => e.PhoneNumber, "UQ__Employee__85FB4E387E893FD2")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Employee__A9D1053479DE1180")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CountryOfBirth)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CountryOfResidence)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Employees__Depar__29572725");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK__Employees__Gende__46E78A0C");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Employees_Payments");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Employees_Positions");

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

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reason).HasColumnType("decimal(18, 2)");
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

            modelBuilder.Entity<Seniority>(entity =>
            {
                entity.ToTable("Seniority");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.HasIndex(e => e.Name, "UQ__Status__737584F6C520FFB4")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
