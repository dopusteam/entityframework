using System.Linq;
using System.Web.Mvc;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class ProjectsController : BaseController
    {
        /// <summary>
        /// Получение списка всех проектов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult All()
        {
            var projects = this.ProjectsService.GetAllProjects();

            var data = projects.Select(this.GetProjectModel);

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectInput"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновление проекта
        /// </summary>
        /// <param name="projectInput"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(long projectId)
        {
            this.ProjectsService.RemoveProject(projectId);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Получение проекта по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get(long id)
        {
            var project = this.ProjectsService.GetProjectById(id);

            var data = this.GetProjectModel(project);

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
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