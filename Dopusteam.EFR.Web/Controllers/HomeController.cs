using System.Web.Mvc;

namespace Dopusteam.EFR.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Home/View.cshtml");
        }
    }
}