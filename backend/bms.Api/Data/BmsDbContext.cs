using Microsoft.EntityFrameworkCore;
using bms.Api.Models;

namespace bms.Api.Data;

public class BmsDbContext : DbContext 
{
    public BmsDbContext(DbContextOptions<BmsDbContext> options)
    :base(options)
    {

    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectStatus> ProjectStatuses { get; set; }
    public DbSet<bms.Api.Models.Task> Tasks { get; set; }
    public DbSet<bms.Api.Models.TaskStatus> TaskStatuses { get; set; }
    public DbSet<bms.Api.Models.TaskPriority> TaskPriorities { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity => 
        { entity.ToTable("roles");
          entity.HasKey(r => r.RoleId);

          entity.Property(r => r.RoleId)
            .HasColumnName("role_id");

          entity.Property(r => r.RoleName)
            .HasColumnName("role_name")
            .HasMaxLength(50)
            .IsRequired();
});

        modelBuilder.Entity<User>(entity =>
        {
          entity.ToTable("users"); entity.HasKey(u => u.UserId);
          entity.Property(u => u.UserId)
          .HasColumnName("user_id");

          entity.Property(u => u.UserName)
          .HasColumnName("user_name")
          .HasMaxLength(100)
          .IsRequired();

          entity.Property(u => u.UserEmail)
          .HasColumnName("user_email")
          .HasMaxLength(255)
          .IsRequired();

          entity.Property(u => u.PasswordHash)
          .HasColumnName("passward_hash")
          .HasMaxLength(255)
          .IsRequired();

          entity.Property(u => u.RoleId)
          .HasColumnName("role_id");

          entity.Property(u => u.IsActive)
          .HasColumnName("is_active");

          entity.Property(u => u.CreatedAt)
          .HasColumnName("created_at");

          entity.HasOne(u => u.Role)
          .WithMany()
          .HasForeignKey(u => u.RoleId);
        });

        modelBuilder.Entity<Department>(entity =>
        {
          entity.ToTable("departments");
          entity.HasKey(d => d.DepartmentId);

          entity.Property(d => d.DepartmentId)
            .HasColumnName("department_id");

          entity.Property(d => d.DepartmentName)
            .HasColumnName("department_name")
            .HasMaxLength(100)
            .IsRequired();

          entity.HasIndex(d => d.DepartmentName)
            .IsUnique();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
          entity.ToTable("employees");

          entity.HasKey(e => e.EmployeeId);

          entity.Property(e => e.EmployeeId)
            .HasColumnName("employee_id");

          entity.Property(e => e.UserId)
            .HasColumnName("user_id");

          entity.Property(e => e.DepartmentId)
            .HasColumnName("department_id");

          entity.Property(e => e.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(100)
            .IsRequired();

          entity.Property(e => e.Phone)
            .HasColumnName("phone")
            .HasMaxLength(30);

          entity.Property(e => e.HireDate)
            .HasColumnName("hire_date")
            .HasColumnType("date");

          entity.Property(e => e.EmployeePic)
            .HasColumnName("employee_pic")
            .HasMaxLength(500);

          entity.Property(e => e.IsActive)
            .HasColumnName("is_active");

          entity.HasIndex(e => e.UserId)
            .IsUnique();

          entity.HasOne<User>()
            .WithOne()
            .HasForeignKey<Employee>(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

          entity.HasOne<Department>()
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
});

modelBuilder.Entity<Customer>(entity =>
{
  entity.ToTable("customers");
  entity.HasKey(c => c.CustomerId);

  entity.Property(c => c.CustomerId)
      .HasColumnName("customer_id");

  entity.Property(c => c.UserId)
      .HasColumnName("user_id");

  entity.Property(c => c.CustomerName)
      .HasColumnName("customer_name")
      .HasMaxLength(100);

  entity.Property(c => c.CompanyName)
      .HasColumnName("company_name")
      .HasMaxLength(100);

  entity.Property(c => c.Email)
      .HasColumnName("email")
      .HasMaxLength(255);

  entity.Property(c => c.Phone)
      .HasColumnName("phone")
      .HasMaxLength(30);

  entity.Property(c => c.Address)
      .HasColumnName("address")
      .HasMaxLength(500);

  entity.Property(c => c.CreatedAt)
      .HasColumnName("created_at")
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

  entity.HasIndex(c => c.UserId).IsUnique();

  entity.HasOne<User>()
      .WithOne()
      .HasForeignKey<Customer>(c => c.UserId)
      .OnDelete(DeleteBehavior.SetNull);
  });

  modelBuilder.Entity<Project>(entity =>
  {
    entity.ToTable("projects");
  entity.HasKey(p => p.ProjectId);

  entity.Property(p => p.ProjectId)
      .HasColumnName("project_id");

  entity.Property(p => p.CustomerId)
      .HasColumnName("customer_id");

  entity.Property(p => p.ProjectName)
      .HasColumnName("project_name")
      .HasMaxLength(200)
    .IsRequired();

  entity.Property(p => p.ManagerId)
      .HasColumnName("manager_id");

  entity.Property(p => p.Description)
      .HasColumnName("description")
      .HasColumnType("text");

  entity.Property(p => p.StartDate)
      .HasColumnName("start_date")
      .HasColumnType("date");

  entity.Property(p => p.EndDate)
      .HasColumnName("end_date")
      .HasColumnType("date");

  entity.Property(p => p.CreatedAt)
      .HasColumnName("created_at")
      .HasDefaultValueSql("CURRENT_TIMESTAMP");

  entity.Property(p => p.StatusId)
      .HasColumnName("status_id");
  });

modelBuilder.Entity<ProjectStatus>(entity =>
{
  entity.ToTable("project_statuses");
  entity.HasKey(s => s.StatusId);

  entity.Property(s => s.StatusId)
      .HasColumnName("status_id")
      .ValueGeneratedOnAdd();

  entity.Property(s => s.StatusName)
      .HasColumnName("status_name")
      .HasMaxLength(50)
      .IsRequired();

  entity.HasIndex(s => s.StatusName)
      .IsUnique();
});

modelBuilder.Entity<bms.Api.Models.TaskStatus>(entity => { entity.ToTable("task_statuses");
entity.HasKey(s => s.StatusId);

entity.Property(s => s.StatusId)
    .HasColumnName("status_id")
    .ValueGeneratedOnAdd();

entity.Property(s => s.StatusName)
    .HasColumnName("status_name")
    .HasMaxLength(50)
    .IsRequired();

entity.HasIndex(s => s.StatusName)
    .IsUnique();
});

modelBuilder.Entity<TaskPriority>(entity =>
{
  entity.ToTable("task_priorities");
  entity.HasKey(p => p.PriorityId);

  entity.Property(p => p.PriorityId)
      .HasColumnName("priority_id")
      .ValueGeneratedOnAdd();

  entity.Property(p => p.PriorityName)
      .HasColumnName("priority_name")
      .HasMaxLength(50)
      .IsRequired();

  entity.HasIndex(p => p.PriorityName)
      .IsUnique();
});

  modelBuilder.Entity<bms.Api.Models.Task>(entity =>
  {
    entity.ToTable("tasks");
    entity.HasKey(t => t.TaskId);

    entity.Property(t => t.TaskId)
      .HasColumnName("task_id");

    entity.Property(t => t.ProjectId)
      .HasColumnName("project_id");

    entity.Property(t => t.EmployeeId)
      .HasColumnName("employee_id");

    entity.Property(t => t.Title)
      .HasColumnName("title")
      .HasMaxLength(200)
      .IsRequired();

    entity.Property(t => t.Description)
      .HasColumnName("description")
      .HasColumnType("text");

    entity.Property(t => t.StartDate)
      .HasColumnName("start_date")
      .HasColumnType("date");

    entity.Property(t => t.DueDate)
      .HasColumnName("due_date")
      .HasColumnType("date");

    entity.Property(t => t.StatusId)
      .HasColumnName("status_id");

    entity.Property(t => t.PriorityId)
      .HasColumnName("priority_id");

    entity.Property(t => t.CompletedAt)
      .HasColumnName("completed_at");

    entity.Property(t => t.CreatedAt)
      .HasColumnName("created_at")
      .HasDefaultValueSql("CURRENT_TIMESTAMP");
  });

  modelBuilder.Entity<ProjectMember>(entity =>
  {
    entity.ToTable("project_members");
    entity.HasKey(pm => pm.ProjectMemberId);

    entity.Property(pm => pm.ProjectMemberId)
        .HasColumnName("project_member_id");

    entity.Property(pm => pm.ProjectId)
        .HasColumnName("project_id")
        .IsRequired();

    entity.Property(pm => pm.EmployeeId)
        .HasColumnName("employee_id")
        .IsRequired();

    entity.Property(pm => pm.Responsibility)
        .HasColumnName("responsibility")
        .HasMaxLength(255);

    entity.Property(pm => pm.JoinedAt)
    .HasColumnName("joined_at")
    .HasColumnType("datetime")
    .HasDefaultValueSql("CURRENT_TIMESTAMP")
    .ValueGeneratedOnAdd();

    entity.Property(pm => pm.LeftAt)
        .HasColumnName("left_at");
});

modelBuilder.Entity<Payment>(entity =>
{
  entity.ToTable("payments");
  entity.HasKey(p => p.PaymentId);

  entity.Property(p => p.PaymentId)
      .HasColumnName("payment_id");

  entity.Property(p => p.ProjectId)
      .HasColumnName("project_id")
      .IsRequired();

  entity.Property(p => p.PaymentMethodId)
      .HasColumnName("payment_method_id");

  entity.Property(p => p.Amount)
      .HasColumnName("amount")
      .HasPrecision(12, 2)
      .IsRequired();

  entity.Property(p => p.PaymentDate)
      .HasColumnName("payment_date")
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();

  entity.Property(p => p.ReferenceNumber)
      .HasColumnName("reference_number")
      .HasMaxLength(100);

  entity.Property(p => p.Notes)
      .HasColumnName("notes");

  entity.Property(p => p.CreatedAt)
      .HasColumnName("created_at")
      .HasDefaultValueSql("CURRENT_TIMESTAMP")
      .ValueGeneratedOnAdd();
  });

  modelBuilder.Entity<PaymentMethod>(entity =>
  {
    entity.ToTable("payment_methods");
    entity.HasKey(pm => pm.PaymentMethodId);

    entity.Property(pm => pm.PaymentMethodId)
        .HasColumnName("payment_method_id")
        .ValueGeneratedOnAdd();

    entity.Property(pm => pm.MethodName)
        .HasColumnName("method_name")
        .HasMaxLength(50)
        .IsRequired();

    entity.HasIndex(pm => pm.MethodName)
        .IsUnique();
    });
}
}