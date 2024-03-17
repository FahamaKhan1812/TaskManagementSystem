using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Categories;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.DAL.Data;
public class DataContext : IdentityDbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Domain.Tasks.Task> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure the Task-Category relationship
        builder.Entity<Domain.Tasks.Task>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade); // Choose the appropriate delete behavior

        builder.Entity<User>()
            .HasMany(t => t.Tasks)
            .WithOne(u => u.User)
            .HasForeignKey(t => t.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);
    }
}
