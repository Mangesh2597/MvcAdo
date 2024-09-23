using Microsoft.AspNetCore.Mvc;
namespace MvcDemo.Controllers
{
    public class StudentController:Controller
    {
            public IActionResult Index()
             {
                    return View();
            }
    }
    
}