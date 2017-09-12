using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.EntityFramework;

namespace Application
{
    public class ProjectsService
    {
        private readonly EfrDbContext _dbContext;

        public ProjectsService()
        {
            this._dbContext = new EfrDbContext();
        }
        
        /// <summary>
        /// Получение списка проектов, отфильтрованных по id
        /// </summary>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        public IList<Project> GetProjectsByIds(IList<long> projectIds)
        {
            // тут в общем ничего сложного, достаём проекты,
            // с помощью Where задаём условие выборки,
            // в данном случае, Id проекта должно присутствовать в списке projectIds
            var projects = this._dbContext
                .Projects
                .Where(project => projectIds.Contains(project.Id))
                .ToList();

            return projects;
        }

        public IList<Project> GetAllProjects()
        {
            // получение всех записей из БД
            return this._dbContext.Projects.ToList();
        }

        public void CreateProject(Project project)
        {
            // тупо добавляем проект в DbSet и вызываем SaveChanges.
            // EF обнаружит что мы что то добавили и сохранит в БД
            this._dbContext.Projects.Add(project);

            this._dbContext.SaveChanges();
        }

        public Project GetProjectById(long id)
        {
            // получение проекта по id
            // по идее надо проверку делать, т.к. Find может вернуть null
            return this._dbContext.Projects.Find(id);
        }

        public void UpdateProject(Project project)
        {
            // достаём проект из БД
            var existedProject = this._dbContext.Projects.Find(project.Id);

            // меняем его имя
            existedProject.Name = project.Name;

            // говорим EF что мы что то изменили
            this._dbContext.Projects.AddOrUpdate(project);

            // сохраняем значения
            this._dbContext.SaveChanges();
        }

        public void RemoveProject(long projectId)
        {
            // достаём из БД проект, который нужно удалить
            // при это явно грузим список студентов
            var existedProject = this._dbContext
                .Projects
                .Include(project => project.Students)
                .Single(project => project.Id == projectId);

            // у выбранного проекта очищаем список студентов
            existedProject.Students.Clear();

            // и затем удаляем проект
            this._dbContext.Projects.Remove(existedProject);

            // сохраняем всё
            this._dbContext.SaveChanges();
        }
    }
}
