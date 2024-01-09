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
    public DbSet<City> Cities => Set<City>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<PersonChange> PersonChanges => Set<PersonChange>();
    public DbSet<Substitution> Substitutions => Set<Substitution>();
    public DbSet<MeetingMember> MeetingMembers => Set<MeetingMember>();

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

        modelBuilder
            .Entity<Meeting>()
            .HasMany(c => c.Employees)
            .WithMany(s => s.Meetings)
            .UsingEntity<MeetingMember>(
                j => j
                    .HasOne(pt => pt.Employees)
                    .WithMany(t => t.MeetingMembers)
                    .HasForeignKey(pt => pt.EmployeesId),
                j => j
                    .HasOne(pt => pt.Meetings)
                    .WithMany(p => p.MeetingMembers)
                    .HasForeignKey(pt => pt.MeetingsId),
                j =>
                {
                    j.HasKey(t => new { t.EmployeesId, t.MeetingsId });
                    j.ToTable("MeetingMembers");
                });
        
        modelBuilder.Entity<Meeting>()
            .HasMany(c => c.Employees)
            .WithMany(s => s.Meetings)
            .UsingEntity(j => j.ToTable("MeetingMembers"));
        
        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.President) // Указываем, что у встречи есть один президент
            .WithMany() // Президент может участвовать во многих встречах, если это не так, уточните отношения
            .HasForeignKey(m => m.PresidentId) // Устанавливаем внешний ключ PresidentId
            .OnDelete(DeleteBehavior.Restrict); // При необходимости установите нужное поведение при удалении записи

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Secretary) // Указываем, что у встречи есть один секретарь
            .WithMany() // Секретарь может участвовать во многих встречах, если это не так, уточните отношения
            .HasForeignKey(m => m.SecretaryId) // Устанавливаем внешний ключ SecretaryId
            .OnDelete(DeleteBehavior.Restrict); // При необходимости установите нужное поведение при удалении записи
    }
}