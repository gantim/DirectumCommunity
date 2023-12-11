using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DirectumCommunity.Models;

public sealed class ApplicationDbContext : IdentityDbContext<DirectumUser>
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Login> Logins => Set<Login>();
    public DbSet<PersonalPhoto> PersonalPhotos => Set<PersonalPhoto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DirectumDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
}