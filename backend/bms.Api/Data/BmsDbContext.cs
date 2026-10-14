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
}
}