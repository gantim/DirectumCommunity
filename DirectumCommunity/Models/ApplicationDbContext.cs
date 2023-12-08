using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Models;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Employee> Employees => Set<Employee>();
    
    public ApplicationDbContext() => Database.EnsureCreated();
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DirectumDb"));
    }
}