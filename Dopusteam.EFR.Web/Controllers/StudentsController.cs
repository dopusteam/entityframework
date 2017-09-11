using System.Linq;
using System.Web.Mvc;
using Application.Enums;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class StudentsController : BaseController
    {
        [HttpGet]
        public JsonResult All(
            int limit,
            SortOrder sortOrder = SortOrder.Asc,
            SortField sortField = SortField.Id,
            bool showGroup = false,
            bool showProjects = false)
        {
            var students = this.StudentsService.GetStudents(
                limit,
                sortOrder,
                sortField,
                showGroup,
                showProjects);

            // это для отправки на фронт, чтоб выести можно было
            var data = students.Select(this.GetStudentModel);

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(StudentFormInput studentInput)
        {
            var student = new Student
            {
                Name = studentInput.Name,
                LastName = studentInput.LastName
            };

            var projectIds = studentInput.ProjectIds;
            var groupId = studentInput.GroupId;

            this.StudentsService.Create(student, projectIds, groupId);

            return this.Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetProjects()
        {
            var projects = this.ProjectsService.GetAllProjects();

            var data = projects.Select(this.GetProjectModel).ToList();

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGroups()
        {
            var groups = this.GroupsService.GetAllGroups();

            var data = groups.Select(group => this.GetGroupModel(@group)).ToList();

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get(long id)
        {
            var student = this.StudentsService.GetStudentInfoById(id);

            var projects = this.ProjectsService.GetAllProjects();

            var groups = this.GroupsService.GetAllGroups();

            var studentProjects = projects
                .Select(project => this.GetStudentProjectModel(project, student))
                .ToList();

            var studentGroups = groups
                .Select(group => this.GetStudentGroupModel(group, student))
                .ToList();

            var data = new StudentFormModel
            {
                Id = student.Id,
                LastName = student.LastName,
                Name = student.Name,
                Groups = studentGroups,
                Projects = studentProjects
            };

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(long studentId)
        {
            this.StudentsService.RemoveStudent(studentId);

            return this.Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Update(StudentFormInput studentInput)
        {
            var existedStudent = this.StudentsService.GetStudentInfoById(studentInput.Id);

            existedStudent.Name = studentInput.Name;
            existedStudent.LastName = studentInput.LastName;

            this.StudentsService.UpdateStudent(
                existedStudent,
                studentInput.ProjectIds,
                studentInput.GroupId);

            return this.Json(new { success = true });
        }

        private StudentModel GetStudentModel(Student student)
        {
            var studentGroup = student.Group != null
                ? new GroupModel
                {
                    Id = student.Group.Id,
                    Number = student.Group.Number
                }
                : null;

            var studentProjects = student.Projects.Select(project => new ProjectModel
            {
                Name = project.Name,
                Id = project.Id
            }).ToList();

            return new StudentModel
            {
                Id = student.Id,
                GroupId = student.GroupId ?? 0,
                Group = studentGroup,
                LastName = student.LastName,
                Name = student.Name,
                Projects = studentProjects
            };
        }

        private StudentProjectModel GetProjectModel(Project project)
        {
            return new StudentProjectModel
            {
                Name = project.Name,
                ProjectId = project.Id
            };
        }

        
        private StudentGroupModel GetGroupModel(Group @group)
        {
            return new StudentGroupModel
            {
                Number = @group.Number,
                GroupId = @group.Id,
                IsEnrolled = false
            };
        }

        

        private StudentGroupModel GetStudentGroupModel(Group @group, Student student)
        {
            return new StudentGroupModel
            {
                Number = @group.Number,
                GroupId = @group.Id,
                IsEnrolled = student.GroupId == @group.Id
            };
        }

        private StudentProjectModel GetStudentProjectModel(Project project, Student student)
        {
            return new StudentProjectModel
            {
                Name = project.Name,
                ProjectId = project.Id,
                IsAssigned = student.Projects.Any(studentProject => studentProject.Id == project.Id)
            };
        }
    }
}