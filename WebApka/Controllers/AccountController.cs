using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApka.Models;

namespace WebApka.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AccountController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Register()
    {
        string username = HttpContext.Session.GetString("username");
        if( username != null ){
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public IActionResult Register(CreateUserDto user)
    {   
        string username = HttpContext.Session.GetString("username");
        if( username != null ){
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        if(ModelState.IsValid)
        {
            using (var db = new DemoContext())
            {   
                string emailError = "";
                string usernameError = "";

                UserModel emailExists = db.Users.FirstOrDefault(u => u.Email == user.Email);
                UserModel usernameExists = db.Users.FirstOrDefault(u => u.Username == user.Username);

                if(emailExists != null)
                {
                    emailError = "This email already exists";
                }

                if(usernameExists != null)
                {
                    usernameError = "This username already exists";
                }

                if(emailExists == null && usernameExists == null)
                {   
                    user.Password = PasswordHelper.EncryptPassword(password: user.Password);
                    UserModel newUser = new UserModel
                    {
                        Name = user.Name,
                        LastName = user.LastName,
                        Username = user.Username,
                        Password = user.Password,
                        Email = user.Email,
                        Age = user.Age

                    };

                    db.Add(newUser);
                    db.SaveChanges();

                    TempData["accountCreatedMessage"] = "Account has been created! Log in to your accoun";

                    return RedirectToAction("Login", "Account");
                }

                TempData["emailError"] = emailError;
                TempData["usernameError"] = usernameError;
            }
        }

        return View();
    }
    
    public IActionResult Login()
    {   
        string username = HttpContext.Session.GetString("username");
        Console.WriteLine(username);
        
        // if( username != null ){
        //     return RedirectToAction("Index", "Home");
        // }
        Console.WriteLine(Request.QueryString.ToString());
        TempData["RedirectUrl"] = Request.QueryString.ToString().Replace("?ReturnUrl=%2F", "").Replace("%2F", "/");
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {   
        // string user_name = HttpContext.Session.GetString("username");
        // if( user_name != null ){
        //     return RedirectToAction("Index", "Home");
        // }

        if(ModelState.IsValid)
        {   
             using (var db = new DemoContext()){
                
                var userExists = db.Users.SingleOrDefault(a => a.Username == username
                && a.Password == PasswordHelper.EncryptPassword(password));

                if(userExists != null)
                {   
                    HttpContext.Session.SetString("username", username);
                    TempData["accountStateMessage"] = "You are logged in!";

                    // return RedirectToAction("Index", "Home");
                    if (TempData["RedirectUrl"].ToString() == "")
                    {      
                        return RedirectToAction("Index", "Home");
                    }else
                    {   
                        string ReturnUrl = TempData["RedirectUrl"].ToString();
                        // Console.WriteLine(ReturnUrl);
                        var results = ReturnUrl.Split('/');
                        string controller = results[0];
                        string view = results[1];
                        // Console.WriteLine(view);


                        return RedirectToAction(view, controller);
                    }
                }
                else
                {
                    TempData["LoginErrorMessage"] = "Failed to login, check your password and username";
                }
             }
        }

        return View();
    }

    [Authorize]
    public IActionResult Logout()
    {   
        
        string username = HttpContext.Session.GetString("username");
        // if( username == null ){
        //     return RedirectToAction("Index", "Home", new { area = "" });
        // }

        HttpContext.Session.Clear();

        TempData["accountStateMessage"] = "You've been logged out";
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    [Authorize]
    public IActionResult AddTask()
    {   
        string username = HttpContext.Session.GetString("username");
        // if( username == null ){
        //     return RedirectToAction("Login", "Account");
        // }

        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddTask(CreateTaskDto task)
    {   
        string username = HttpContext.Session.GetString("username");
        // if( username == null ){
        //     return RedirectToAction("Login", "Account");
        // }

        if(ModelState.IsValid)
        {   
            using (var db = new DemoContext())
            {
                var userExists = db.Users.SingleOrDefault(a => a.Username == username);
                TaskModel newTask = new TaskModel
                {
                    Title = task.Title,
                    Content = task.Content,
                    EndTime = task.EndTime,
                    User = userExists,
                    UserId = userExists.id

                };

                db.Add(newTask);
                db.SaveChanges();

                return RedirectToAction("ViewTask", "Account", new { id = newTask.id });
            }
        }
        return View();
    }

    [Authorize]
    public IActionResult MyTasks()
    {   
        string username = HttpContext.Session.GetString("username");

        // if( username == null ){
        //     return RedirectToAction("Login", "Account");
        // }

        using (var db = new DemoContext())
        {   
            Dictionary<string, List<TaskModel>> TasksDict = new Dictionary<string, List<TaskModel>>();
            TasksDict["Created"] = new List<TaskModel>();
            TasksDict["In Progress"] = new List<TaskModel>();
            TasksDict["Done"] = new List<TaskModel>();
            TasksDict["Blocked"] = new List<TaskModel>();
            
            UserModel user = db.Users.SingleOrDefault(a => a.Username == username);
            Dictionary<string, List<TaskModel>> userTasks 
                = db.Tasks.Where(t => t.UserId == user.id).GroupBy(t => t.Status).
                ToDictionary(g => g.Key, g => g.ToList());

            foreach (KeyValuePair<string, List<TaskModel>> task in userTasks)
            {
                TasksDict[task.Key] = task.Value;
            }

            ViewData["Tasks"] = TasksDict;
        }
        
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult UpdateTaskStatus(int taskId, UpdateTaskStatusDto updatedTask)
    {   
        string referringUrl = HttpContext.Request.Headers["Referer"].ToString();
        string username = HttpContext.Session.GetString("username");
        
        using(var db = new DemoContext())
        {
            TaskModel task = db.Tasks.SingleOrDefault(t => t.id == taskId & t.User.Username == username);
            if(task == null)
            {
                 return NotFound(new { error = "Task not found." });
            }

            task.Status = updatedTask.Status;
            db.SaveChanges();
        }

        if(referringUrl.Contains("ViewTask"))
        {
            return RedirectToAction("ViewTask", "Account", new { id = taskId });
        }
        else
        {
             return RedirectToAction("MyTasks", "Account");
        }
        
    }

    [Authorize]
    public IActionResult ViewTask(int id)
    {   
        string username = HttpContext.Session.GetString("username");

        // if(username == null)
        // {
        //     return RedirectToAction("Login", "Account");
        // }

        using(var db = new DemoContext())
        {   
            TaskModel task = db.Tasks.SingleOrDefault(t => t.id == id & t.User.Username == username);

            if(task == null)
            {
                 return NotFound(new { error = "Task not found." });
            }

            ViewData["Task"] = task;
        }
        TempData["TaskId"] = id.ToString();
        
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult DeleteTask(int id)
    {   
        string username = HttpContext.Session.GetString("username");
        // if(username == null)
        // {
        //     return RedirectToAction("Login", "Account");
        // }
        
        using(var db = new DemoContext())
        {   
            TaskModel task = db.Tasks.SingleOrDefault(t => t.id == id & t.User.Username == username);
            db.Tasks.Remove(task);
            db.SaveChanges();
            
        }
        return RedirectToAction("MyTasks", "Account");
    }

    [Authorize]
    public IActionResult UpdateTask(int id)
    {
        string username = HttpContext.Session.GetString("username");
        // if(username == null)
        // {
        //     return RedirectToAction("Login", "Account");
        // }

        using(var db = new DemoContext())
        {   
            TaskModel task = db.Tasks.SingleOrDefault(t => t.id == id & t.User.Username == username);
            ViewData["Task"] = task;
        }

        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult UpdateTask(int id, UpdateTaskDto task)
    {   
        string username = HttpContext.Session.GetString("username");
        // if(username == null)
        // {
        //     return RedirectToAction("Login", "Account");
        // }

    
        using(var db = new DemoContext())
        {   
            TaskModel taskToUpdate = db.Tasks.SingleOrDefault(t => t.id == id & t.User.Username == username);
            ViewData["Task"] = taskToUpdate;
            if(ModelState.IsValid)
            {
                taskToUpdate.Title = task.Title;
                taskToUpdate.Content = task.Content;
                taskToUpdate.EndTime = task.EndTime;
                db.SaveChanges();

                return RedirectToAction("ViewTask", "Account", new { id = id });
            }
        }
        
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
