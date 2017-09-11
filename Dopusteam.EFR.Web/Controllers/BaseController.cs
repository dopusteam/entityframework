using System.Web.Mvc;
using Application;

namespace Dopusteam.EFR.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly StudentsService StudentsService;
        protected readonly ProjectsService ProjectsService;
        protected readonly GroupsService GroupsService;

        public BaseController()
        {
            this.StudentsService = new StudentsService();
            this.ProjectsService = new ProjectsService();
            this.GroupsService = new GroupsService();
        }
    }
}