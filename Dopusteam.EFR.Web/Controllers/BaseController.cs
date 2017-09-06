using Dopusteam.EFR.EntityFramework;
using System.Web.Mvc;

namespace Dopusteam.EFR.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly EfrDbContext DbContext;

        public BaseController()
        {
            this.DbContext = new EfrDbContext();
        }
    }
}