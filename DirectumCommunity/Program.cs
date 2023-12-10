using DirectumCommunity.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ���� ���������� ��� �������(���� ���� ������ � �.�., ��� ��� ������� ����� �������� � ���)
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

var app = builder.Build();

/*
���� �������� ���� � ������������ ��� ��������� ��������� ��������� HTTP-��������. �� ��� ������ ����� �������� ���� ���������� ����� ��������� �� � ������ ����������(��� ��� ��������� ��)
if (!app.Environment.IsDevelopment()) - ��� ������� ���������, ��������� �� ���������� � ������ ����������. (����� ������ ����� ������� � ���������� �������, �� �� ���� �� ����)
app.UseExceptionHandler("/Directum/Error") - ��� ������ ���������, ��� ��� ������������� ���������� (exception) � ����������, ������ ������ ���� ������������� �� ��������� ���� (����� ������ �������)
app.UseHsts() - ��� ��������, ������� �������� �������� ������������ ������ ���������� ���������� (HTTPS) ����� ��� ����� ����������, ����� ���������� �� ��������� � ������ ����������.
*/
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Directum/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.(�� ������ ����, ���� ����� �������� � ������� 30 ���� ���� ����)
    app.UseHsts();
}
// ��� ��� ������������ � ���� �� ����� ��������������
app.UseHttpsRedirection();
// ��� ����� ������������ ������ ����� �� ���� CSS FILES
app.UseStaticFiles();
// �� ��� ������
app.UseRouting();
// �� ����������� �����������(�� ������ ��� ����� ���
app.UseAuthorization();
// ��� ����������� �������� �������� ����� ���������� ��������� ������� � ������ �� ���/��������/index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employees}/{action=Index}/{id?}");

app.Run();
