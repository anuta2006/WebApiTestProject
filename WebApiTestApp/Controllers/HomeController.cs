using System.Configuration;
using System.Web.Mvc;

namespace WebApiTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string greeting = ConfigurationManager.AppSettings["Greeting"];
            return Content(greeting);
        }
    }
}
