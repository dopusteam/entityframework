using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Enums;

namespace Dopusteam.EFR.Web.Controllers
{
    public class StudentsController : BaseController
    {
        [HttpGet]
        public JsonResult All(int limit, SortOrder sortOrder = SortOrder.Asc, SortField sortField = SortField.Id)
        {
            var students = this.DbContext.Students.Take(limit);

            switch (sortField)
            {
                case SortField.Id:
                    students = sortOrder == SortOrder.Asc
                        ? students.OrderBy(student => student.Id)
                        : students.OrderByDescending(student => student.Id);
                    break;

                case SortField.Name:
                    students = sortOrder == SortOrder.Asc
                        ? students.OrderBy(student => student.Name)
                        : students.OrderByDescending(student => student.Name);
                    break;

                case SortField.LastName:
                    students = sortOrder == SortOrder.Asc
                        ? students.OrderBy(student => student.LastName)
                        : students.OrderByDescending(student => student.LastName);
                    break;
            }

            return this.Json(new {data = students}, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult Delete(long studentId)
        {
            var student = this.DbContext.Students.Find(studentId);

            this.DbContext.Students.Remove(student);

            this.DbContext.SaveChanges();

            return this.Json(new { success = true });
        }
    }
}