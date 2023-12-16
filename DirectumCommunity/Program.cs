using DirectumCommunity.Hubs;
using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddIdentity<DirectumUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IDirectumService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var host = configuration["DirectumRxConfig:Host"];
    var login = configuration["DirectumRxConfig:Login"];
    var password = configuration["DirectumRxConfig:Password"];
    return new DirectumService(host, login, password);
});

builder.Services.AddTransient<EmployeeService>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireDb"))
    .UseConsole());

builder.Services.AddHangfireServer();

builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Directum/Error");
    app.UseHsts();
}

app.UseHangfireDashboard("/jobs");

app.MapHub<BirthdayHub>("/birthday");

RecurringJob.AddOrUpdate<IDirectumService>("ImportDataFromDirectumRX", x => x.ImportData(null), Cron.Hourly,
    new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    "default",
    "{controller=Employees}/{action=Index}/{id?}");

app.Run();