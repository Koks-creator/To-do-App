using Microsoft.EntityFrameworkCore;

namespace WebApka.Models;

public class DemoContext: DbContext
{
    public DbSet<UserModel> Users {get; set;}
    public DbSet<TaskModel> Tasks {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=C:\Users\table\OneDrive\Pulpit\SziSzarp\WebApka\test.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserModel>()
            .HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique");
        
        modelBuilder.Entity<UserModel>()
            .HasIndex(e => e.Username)
            .IsUnique()
            .HasDatabaseName("IX_Users_Username_Unique");

        // modelBuilder.Entity<UserModel>()
        // .HasMany(c => c.Tasks)
        // .WithOne(e => e.User);
    }

}
