using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Ниже подключаем все сервисы(типа базы данных и т.п., как это сделать можно спросить у гпт)
builder.Services.AddControllersWithViews();

var app = builder.Build();

/*
Этот фрагмент кода в используется для настройки конвейера обработки HTTP-запросов. Всё что внутри будет работать если приложение будет находится не в режиме разработки(щас это скипается всё)
if (!app.Environment.IsDevelopment()) - Это условие проверяет, находится ли приложение в режиме разработки. (Смену режима можно сделать в настройках проекта, но эт пока не надо)
app.UseExceptionHandler("/Directum/Error") - Эта строка указывает, что при возникновении исключения (exception) в приложении, запрос должен быть перенаправлен на указанный путь (Кароч ошибку выводит)
app.UseHsts() - это механизм, который сообщает браузеру использовать только защищенное соединение (HTTPS) Здесь эта опция включается, когда приложение не находится в режиме разработки.
*/
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Directum/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.(эт юзлесс инфа, типа будет работать в течении 30 дней туда сюда)
    app.UseHsts();
}
// это для безопасности с хттп на хттпс перенаправляет
app.UseHttpsRedirection();
// Эта штука обрабатывает статик файлы по типу CSS FILES
app.UseStaticFiles();
// эт ЮРЛ АДРЕСА
app.UseRouting();
// эт возможность авторизации(хз правда как юзать это
app.UseAuthorization();
// тут указывается домашняя страница сайта контроллер директума юзается и иднекс из вью/директум/index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Directum}/{action=Index}/{id?}");

app.Run();
