using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
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
            var studentsQuery = this.DbContext.Students.Take(limit);

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

            if (showGroup)
            {
                studentsQuery = studentsQuery.Include(student => student.Group);
            }
            if (showProjects)
            {
                studentsQuery = studentsQuery.Include(student => student.Projects);
            }

            var students = studentsQuery.ToList();

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
        public JsonResult Create(Student student)
        {
            student.Id = 0;

            this.DbContext.Students.Add(student);

            this.DbContext.Entry(student).State = EntityState.Added;

            this.DbContext.SaveChanges();

            return this.Json(new {success = true});
        }

        [HttpGet]
        public JsonResult Get(long id)
        {
            var student = this.DbContext.Students.Find(id);

            var studentModel = new StudentModel
            {
                Id = student.Id,
                LastName = student.LastName,
                Name = student.Name
            };

            return this.Json(new {data = studentModel}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(long studentId)
        {
            var student = this.DbContext.Students.Find(studentId);

            this.DbContext.Students.Remove(student);

            this.DbContext.SaveChanges();

            return this.Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Update(Student student)
        {
            var existedStudent = this.DbContext.Students.Find(student.Id);

            existedStudent.Name = student.Name;
            existedStudent.LastName = student.LastName;
            this.DbContext.Students.AddOrUpdate(existedStudent);

            this.DbContext.SaveChanges();

            return this.Json(new { success = true });
        }
    }
}