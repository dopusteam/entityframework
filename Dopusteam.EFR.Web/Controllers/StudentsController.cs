using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Enums;
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
            //создаём запрос
            var studentsQuery = this.DbContext.Students.Take(limit);

            // зависимоти от параметров добавляем сортировку и её направление
            switch (sortField)
            {
                case SortField.Id:
                    studentsQuery = sortOrder == SortOrder.Asc
                        ? studentsQuery.OrderBy(student => student.Id)
                        : studentsQuery.OrderByDescending(student => student.Id);
                    break;

                case SortField.Name:
                    studentsQuery = sortOrder == SortOrder.Asc
                        ? studentsQuery.OrderBy(student => student.Name)
                        : studentsQuery.OrderByDescending(student => student.Name);
                    break;

                case SortField.LastName:
                    studentsQuery = sortOrder == SortOrder.Asc
                        ? studentsQuery.OrderBy(student => student.LastName)
                        : studentsQuery.OrderByDescending(student => student.LastName);
                    break;
            }

            // если необходимо загрузить группу студента
            if (showGroup)
            {
                // то указываем эо явно. Поле Group в сущности Student не помечено virtual,
                // поэтому загружаться не будет, если мы явно не сделаем Include
                studentsQuery = studentsQuery.Include(student => student.Group);
            }
            // аналогично с проектами
            if (showProjects)
            {
                studentsQuery = studentsQuery.Include(student => student.Projects);
            }

            // выполняем запрос
            var students = studentsQuery.ToList();

            // это для отправки на фронт, чтоб выести можно было
            var data = students.Select(student => new StudentModel
            {
                Id = student.Id,
                GroupId = student.GroupId ?? 0,
                Group = student.Group != null ? new GroupModel
                {
                    Id = student.Group.Id,
                    Number = student.Group.Number
                } : null,
                LastName = student.LastName,
                Name = student.Name,
                Projects = student.Projects.Select(project => new ProjectModel
                {
                    Name = project.Name,
                    Id = project.Id
                }).ToList()
            });

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(StudentFormInput studentInput)
        {
            var student = new Student();

            var projects = this.DbContext.Projects.Where(project => studentInput.ProjectIds.Contains(project.Id)).ToList();
            var group = studentInput.GroupId.HasValue ? this.DbContext.Groups.First(g => g.Id == studentInput.GroupId.Value) : null;

            student.Name = studentInput.Name;
            student.LastName = studentInput.LastName;
            student.Projects = projects;
            student.Group = group;

            this.DbContext.Students.Add(student);

            this.DbContext.Entry(student).State = EntityState.Added;

            this.DbContext.SaveChanges();

            return this.Json(new {success = true});
        }

        [HttpGet]
        public JsonResult GetProjects()
        {
            var projects = this.DbContext.Projects.ToList();

            var data = projects.Select(project => new StudentProjectModel
            {
                Name = project.Name,
                ProjectId = project.Id,
                IsAssigned = false
            }).ToList();

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGroups()
        {
            var groups = this.DbContext.Groups.ToList();

            var data = groups.Select(group => new StudentGroupModel
            {
                Number = group.Number,
                GroupId = group.Id,
                IsEnrolled = false
            }).ToList();

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get(long id)
        {
            // достаём студента вместе с проектами
            // для группы Include делать не надо, так как Student в любом случае содержит GroupId, этого нам достаточно
            var student = this.DbContext.Students.Include(s => s.Projects).Single(s => s.Id == id);

            var projects = this.DbContext.Projects.ToList();

            var groups = this.DbContext.Groups.ToList();

            // тут для вывода на фромте дальше
            var studentProjects = projects.Select(project => new StudentProjectModel
            {
                Name = project.Name,
                ProjectId = project.Id,
                IsAssigned = student.Projects.Any(studentProject => studentProject.Id == project.Id)
            }).ToList();

            var studentGroups = groups.Select(group => new StudentGroupModel
            {
                Number = group.Number,
                GroupId = group.Id,
                IsEnrolled = student.GroupId == group.Id
            }).ToList();

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
            // находим студента в БД
            var student = this.DbContext.Students.Find(studentId);

            // и удаляем его
            this.DbContext.Students.Remove(student);

            this.DbContext.SaveChanges();

            return this.Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Update(StudentFormInput studentInput)
        {
            var existedStudent = this.DbContext.Students.Include(s => s.Projects).Single(s => s.Id == studentInput.Id);

            var projects = this.DbContext.Projects.Where(project => studentInput.ProjectIds.Contains(project.Id)).ToList();
            var group = studentInput.GroupId.HasValue ? this.DbContext.Groups.First(g => g.Id == studentInput.GroupId.Value) : null;

            // определяем какие проекты удалить надо 'из студента'
            // т.е. отвязываем проект от сущности студента
            var deletedProjects = existedStudent.Projects.Except(projects);

            foreach (var deletedProject in deletedProjects)
            {
                // и удаляем у проекта из списка студентов текущего студента
                deletedProject.Students.Remove(existedStudent);
            }

            // так же, определяем, какие проекты надо привязать к студенту
            var newProjects = projects.Except(existedStudent.Projects);

            foreach (var newProject in newProjects)
            {
                // и добавляем их в соответствующй список
                existedStudent.Projects.Add(newProject);
            }

            // инициируем остальные поля
            existedStudent.Name = studentInput.Name;
            existedStudent.LastName = studentInput.LastName;

            existedStudent.Group = group;
            existedStudent.GroupId = group?.Id ?? 0;

            // и сохраняем
            this.DbContext.Students.AddOrUpdate(existedStudent);

            this.DbContext.SaveChanges();

            return this.Json(new { success = true });
        }
    }
}