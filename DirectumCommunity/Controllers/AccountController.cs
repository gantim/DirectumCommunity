using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DirectumCommunity.Models;
using DirectumCommunity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace DirectumCommunity.Controllers;

public class AccountController : Controller
{
    private readonly IDirectumService _directumService;
    private readonly ILogger<AccountController> _logger;
    private readonly EmployeeService _employeeService;
    private readonly SignInManager<DirectumUser> _signInManager;
    private readonly UserManager<DirectumUser> _userManager;

    public AccountController(IDirectumService directumService,
        ILogger<AccountController> logger,
        EmployeeService employeeService,
        SignInManager<DirectumUser> signInManager,
        UserManager<DirectumUser> userManager)
    {
        _directumService = directumService;
        _logger = logger;
        _employeeService = employeeService;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _directumService.Login(model.Login, model.Password))
            {
                await using (var db = new ApplicationDbContext())
                {
                    if (!db.Users.Any(u => u.UserName == model.Login))
                    {
                        var employee = await _employeeService.GetByLogin(model.Login);

                        var user = new DirectumUser { UserName = model.Login, EmployeeId = employee.Id };
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Employees");
                        }
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false,
                            lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Employees");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                        }
                    }
                }
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.Title = "Авторизация";
        return View(new LoginViewModel());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}