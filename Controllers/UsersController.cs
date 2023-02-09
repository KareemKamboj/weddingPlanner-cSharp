using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

public class UsersController : Controller
{
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }
    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }
    private MyContext db;
    public UsersController(MyContext context)
    {
        db = context;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            if (db.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "is taken");
            }
        }

        if(ModelState.IsValid == false)
        {
            return Index();
        }

        PasswordHasher<User> hashBrowns = new PasswordHasher<User>();
        newUser.Password = hashBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        return RedirectToAction("All", "Weddings");
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (ModelState.IsValid == false)
        {
            return Index();
        }

        User? dbUser = db.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            ModelState.AddModelError("LoginEmail", "not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashBrowns.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "invalid password");
            return Index();
        } 

        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        return RedirectToAction("All", "Weddings");
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}