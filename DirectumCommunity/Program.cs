using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ���� ���������� ��� �������(���� ���� ������ � �.�., ��� ��� ������� ����� �������� � ���)
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Directum}/{action=Index}/{id?}");

app.Run();
