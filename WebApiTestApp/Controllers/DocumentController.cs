using log4net;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiTestApp.Models;
using WebApiTestApp.Services;

namespace WebApiTestApp.Controllers
{
    public class DocumentController : Controller
    {
        private static readonly DocumentStore documentStore = new DocumentStore();
        private static readonly DocumentNotificationManager documentNotificationManager = new DocumentNotificationManager();

        private ILog _logger;

        public DocumentController()
        {
            _logger = LogManager.GetLogger(typeof(DocumentController));
        }

        // GET: Document
        public ActionResult Index()
        {
            _logger.Info("Request received");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase document)
        {
            _logger.Info("Ulpoad request received");
            if (document != null)
            {
                string fileName = Path.GetFileName(document.FileName);
                
                byte[] byteArray = new byte[document.ContentLength];
                document.InputStream.Read(byteArray, 0, document.ContentLength);

                Stream stream = new MemoryStream(byteArray);
                await documentStore.SaveDocumentAsync(stream, fileName).ConfigureAwait(false);

                await documentNotificationManager.AddAsync(fileName, documentStore.UriFor(fileName)).ConfigureAwait(false);

                return RedirectToAction("ShowAsync");
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ShowAsync()
        {
            var model = await documentNotificationManager.GetAsync();
            return View(model);
        }
    }
}