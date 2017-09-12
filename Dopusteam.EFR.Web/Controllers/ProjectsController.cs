using System.Linq;
using System.Web.Mvc;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class ProjectsController : BaseController
    {
        [HttpGet]
        public JsonResult All()
        {
            var projects = this.ProjectsService.GetAllProjects();

            var data = projects.Select(this.GetProjectModel);

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(ProjectFormInput projectInput)
        {
            var project = new Project
            {
                Name = projectInput.Name
            };

            this.ProjectsService.CreateProject(project);

            return this.Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Update(ProjectFormInput projectInput)
        {
            var project = new Project
            {
                Id = projectInput.Id,
                Name = projectInput.Name
            };

            this.ProjectsService.UpdateProject(project);

            return this.Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Delete(long projectId)
        {
            this.ProjectsService.RemoveProject(projectId);

            return this.Json(new { success = true });
        }

        [HttpGet]
        public JsonResult Get(long id)
        {
            var project = this.ProjectsService.GetProjectById(id);

            var data = this.GetProjectModel(project);

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        private ProjectInfoModel GetProjectModel(Project project)
        {
            return new ProjectInfoModel
            {
                Id = project.Id,
                Name = project.Name,
                StudentsAmount = this.StudentsService.GetProjectStudentsCount(project.Id)
            };
        }
    }
}