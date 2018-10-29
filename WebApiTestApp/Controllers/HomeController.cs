using log4net;
using System.Configuration;
using System.Web.Mvc;

namespace WebApiTestApp.Controllers
{
    public class HomeController : Controller
    {
        private ILog _logger;

        public HomeController()
        {
            _logger = LogManager.GetLogger(typeof(HomeController));
        }

        public ActionResult Index()
        {
            _logger.Info("Request received");

            string greeting = ConfigurationManager.AppSettings["Greeting"];
            string ciGreating = " Now with CI!";
            return Content(greeting + ciGreating);
        }
    }
}
