using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApka.Models;

namespace WebApka.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult test()
    {
        return View();
    }

    public IActionResult ShowRecords()
    {   
        List<UserModel> users = new List<UserModel>();
        List<string> columns = new List<string>();

        using(var db = new DemoContext()){
            users = db.Users.ToList();

            var table = typeof(UserModel);

            foreach (var prop in typeof(UserModel).GetProperties())
            {
                columns.Add(prop.Name);
            }

        }
        TempData["users"] = users;
        TempData["columns"] = columns;

        return View();
    }

    public IActionResult SearchUser(string searchTerm){
        List<UserModel> users = new List<UserModel>();
        List<string> columns = new List<string>();

        using (var db = new DemoContext())
        {
            users = db.Users.Where(u => u.Name.Contains("Andrew")).ToList();
            var table = typeof(UserModel);

            foreach (var prop in typeof(UserModel).GetProperties())
            {
                columns.Add(prop.Name);
            }
        }

        TempData["users"] = users;
        TempData["columns"] = columns;

        return View("ShowRecords");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
