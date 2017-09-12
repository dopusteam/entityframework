using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Application.Enums;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.EntityFramework;
using System.Linq;

namespace Application
{
    public class StudentsService
    {
        private readonly EfrDbContext _dbContext;
        private readonly ProjectsService _projectsService;
        private readonly GroupsService _groupsService;

        public StudentsService()
        {
            this._dbContext = new EfrDbContext();
            this._projectsService = new ProjectsService();
            this._groupsService = new GroupsService();
        }

        public void Create(Student student, IList<long> studentProjectIds, long? groupId)
        {
            // достаём из БД проекты, отфильтрованные по id
            var projects = this._projectsService.GetProjectsByIds(studentProjectIds);

            if (groupId != null)
            {
                // достаём группу по id
                var group = this._groupsService.GetGroupById(groupId);

                // и присваиваем сущности студента
                student.GroupId = group.Id;
            }

            // теперь надо указать EF, что список проектов, уже существует в БД
            // т.е. тут мы итерируемсписок проектов
            foreach (var project in projects)
            {
                // и говорим, что текущий проект (project) уже существует в БД
                // EF таким образом будет знать, что не нужно создавать новую сущность в БД
                this._dbContext.Projects.Attach(project);
            }

            // потом мы присваиваем сущности студента список проектов
            student.Projects = projects;

            // добавляем студента в БД
            this._dbContext.Students.Add(student);

            // и сохраняем. Т.к. мы указали EF, что список проектов уже сущесвует
            // то новые проекты созданы не будут, а будут добавленны ссылки на уже существующие
            this._dbContext.SaveChanges();
        }

        public IList<Student> GetStudents(
            int limit,
            SortOrder sortOrder = SortOrder.Asc,
            SortField sortField = SortField.Id,
            bool showGroup = false,
            bool showProjects = false)
        {
            //создаём запрос
            var studentsQuery = this._dbContext.Students.Take(limit);

            // в зависимости от параметров добавляем сортировку и её направление
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

            // выполняем запрос и возвращаем результат
            var students = studentsQuery.ToList();

            return students;
        }

        public Student GetStudentInfoById(long studentId)
        {
            // достаём студента вместе с проектами
            // для группы Include делать не надо,
            // так как Student в любом случае содержит GroupId, этого нам достаточно
            var student = this._dbContext
                .Students
                .Include(s => s.Projects)
                .Single(s => s.Id == studentId);

            return student;
        }

        public void RemoveStudent(long studentId)
        {
            // находим студента в БД
            // метод Find ищет запись по первичному ключу
            // в данном случае, поле Id - первичный ключ
            var student = this._dbContext.Students.Find(studentId);

            // если студента нет, то ничего не делаем
            if (student == null)
            {
                return;
            }

            // и удаляем его
            this._dbContext.Students.Remove(student);

            // сохраняем изменения
            this._dbContext.SaveChanges();
        }

        /// <summary>
        /// Обновление студента
        /// </summary>
        /// <param name="student"></param>
        /// <param name="projectIds"></param>
        /// <param name="groupId"></param>
        public void UpdateStudent(Student student, IList<long> projectIds, long? groupId)
        {
            // сначала достаём из БД студенты по id
            var existedStudent = this.GetStudentInfoById(student.Id);

            // затем достаём проекты, аналогично тому, как доставали при создании нового студента
            var projects = this._projectsService.GetProjectsByIds(projectIds);

            if (groupId != null)
            {
                // и достаём группу
                var group = this._groupsService.GetGroupById(groupId);

                existedStudent.GroupId = group.Id;
            }

            // определяем какие проекты удалить надо 'из студента'
            // т.е. отвязываем проект от сущности студента
            // для этого достаём из текущей сущности студента те проекты,
            // id которых нету во входных параметрах метода (projectIds)
            foreach (var projectForDelete in existedStudent.Projects
                .Where(project => !projectIds.Contains(project.Id)).ToList())
            {
                // и удаляем у проекта из списка студентов текущего студента
                existedStudent.Projects.Remove(projectForDelete);
            }

            // так же, определяем, какие проекты надо привязать к студенту
            // для этого берём такие проекты, id которых ещё нет в проектах студента
            var newProjects = projects.Where(project => existedStudent.Projects.All(studentProject => studentProject.Id != project.Id));

            foreach (var newProject in newProjects)
            {
                // аналогично как делали при добавлении студента,
                // указываем DB, что сущности уже существуют и новые создавать не надо
                // в дааном случае newProjects - это проекты, уже существующие в БД,
                // но не привязанные к студенту.
                this._dbContext.Projects.Attach(newProject);
                // и добавляем их в соответствующй список
                existedStudent.Projects.Add(newProject);
            }

            // и сохраняем
            this._dbContext.Students.AddOrUpdate(existedStudent);
            this._dbContext.SaveChanges();
        }

        public long GetGroupStudentsCount(long groupId)
        {
            return this._dbContext.Students.Count(student => student.GroupId == groupId);
        }

        public long GetProjectStudentsCount(long projectId)
        {
            return this._dbContext.Students.Count(student => student.Projects.Any(project => project.Id == projectId));
        }
    }
}
