using System.Linq;
using System.Web.Mvc;
using Application.Enums;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class StudentsController : BaseController
    {
        /// <summary>
        /// Получение списка всех студентов
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="sortOrder"></param>
        /// <param name="sortField"></param>
        /// <param name="showGroup"></param>
        /// <param name="showProjects"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult All(
            int limit,
            SortOrder sortOrder = SortOrder.Asc,
            SortField sortField = SortField.Id,
            bool showGroup = false,
            bool showProjects = false)
        {
			// получаем из сервиса всех студентов
            var students = this.StudentsService.GetStudents(
                limit,
                sortOrder,
                sortField,
                showGroup,
                showProjects);

            // это для отправки на фронт, чтоб вывести можно было нормально
			// фактически мы можем отправить и без преобразования, т.е. массив students
			// но это делать не рекомендуется, т.к.
			// во-первых, если есть виртуальные поля у модели
			// (т.е. те, которые будут подгружаться лениво, при обращении к ним)
			// то при сериализации в json может возникнуть ситуация,
			// что при обращении к виртуальному полю будет дополнительный запрос, который вернёт новую сущность,
			// потом сериализатор попробует её сериализовать и достанет из БД ещё что-нибудь и т.д.
			// плюс если, например, student содержит в себе список проектов, каждый из которых содержит в себе список студентов,
			// то при сериализации надо явно указывать, чтоб это правильно обрабатывалось
			// иначе будет огромная структура вида { studentId: 1, projects: [ {projectId: 1, students: [{studentId: 1, projects: [...] ]]}
			// во-вторых, лучше не отдавать на фронт всё, что есть в бд (все поля).
			// например в БД может хранить id пользователя, создавшего сущность, на фронте эта ифнормация не нужна.
			// даже контроллер, по идее, не должен иметь доступ к сущности,
			// а должен получать какой то объект, сформированный из сущности, но для простоты я этого тут не делал
			// в третьих, иногда на фронте нужно вывести информацию не в том виде, в котором она лежит в БД
            var data = students.Select(this.GetStudentModel);

			// возвращаем на фронта
            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Создание студента
        /// </summary>
        /// <param name="studentInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(StudentFormInput studentInput)
        {
			// для создания студента был создан класс studentInput,
			// который содержит только то, что нужно для создания
			// и ничего лишнего, чтоб не гонять лишние данные
            var student = new Student
            {
                Name = studentInput.Name,
                LastName = studentInput.LastName
            };

            var projectIds = studentInput.ProjectIds;
            var groupId = studentInput.GroupId;

			// тут в общем просто отправляем сущность в сервис, для создания
            this.StudentsService.Create(student, projectIds, groupId);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Получение всех проектов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetProjects()
        {
            var projects = this.ProjectsService.GetAllProjects();

			// аналогично списку студентов, лучше не отдавать сущность на фронт
			// в таком виде, в котором она лежит в БД
            var data = projects.Select(this.GetProjectModel).ToList();

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получение всех групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetGroups()
        {
            var groups = this.GroupsService.GetAllGroups();

            var data = groups.Select(this.GetGroupModel).ToList();

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получение студента по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get(long id)
        {
			// тут достаём студента
            var student = this.StudentsService.GetStudentInfoById(id);

			// отдельно достаём все проекты,
			// потому что на форме добавления\редактирования нам нужны все проекты\группы
            var projects = this.ProjectsService.GetAllProjects();

            var groups = this.GroupsService.GetAllGroups();

			// тут как раз пример того, что в БД инфа лежит в одном виде
			// но вывести на фронт её нам нужно в другом
			// в БД лежит таблица с проектами и есть ссылки из таблицы студентов на таблицу с проектами
			// на фронте же нам нужен список всех проектов, помеченных флагом, есть ли связь между студентом и проектом
            // кстати, для разных страниц мы можем создавать разные классы для одних и тех же сущностей
            var studentProjects = projects
                .Select(project => this.GetStudentProjectModel(project, student))
                .ToList();

			// аналогично проектам
            var studentGroups = groups
                .Select(group => this.GetStudentGroupModel(group, student))
                .ToList();

			// создаём сущность для отправки на фронт
            var data = new StudentFormModel
            {
                Id = student.Id,
                LastName = student.LastName,
                Name = student.Name,
                Groups = studentGroups,
                Projects = studentProjects
            };

			// возращаем данные
            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Удаление студента
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(long studentId)
        {
			// просто вызываем метод сервиса, для удаления по id
            this.StudentsService.RemoveStudent(studentId);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Обновление студента
        /// </summary>
        /// <param name="studentInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(StudentFormInput studentInput)
        {
			// достаём студента из БД
            var existedStudent = this.StudentsService.GetStudentInfoById(studentInput.Id);

			// изменяем поля, которые могут быть изменены
			// например поле Id мы не меняем, т.к. оно ялвяется ключом
            existedStudent.Name = studentInput.Name;
            existedStudent.LastName = studentInput.LastName;

			// и обновляем студента
            this.StudentsService.UpdateStudent(
                existedStudent,
                studentInput.ProjectIds,
                studentInput.GroupId);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        private StudentModel GetStudentModel(Student student)
        {
			// это внтуренний метод, который преобразует сущность студента в вид, пригодный для отправки на фронт
            var studentGroup = student.Group != null
                ? new GroupModel
                {
                    Id = student.Group.Id,
                    Number = student.Group.Number
                }
                : null;

			// для отображения списка проектов студента, достаточно id и названия проекта
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

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private StudentProjectModel GetProjectModel(Project project)
        {
			// аналогично, создаём объект для оптравки на фронт
            return new StudentProjectModel
            {
                Name = project.Name,
                ProjectId = project.Id
            };
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private StudentGroupModel GetGroupModel(Group group)
        {
			// аналогично, создаём объект для оптравки на фронт
            return new StudentGroupModel
            {
                Number = group.Number,
                GroupId = group.Id,
                IsEnrolled = false
            };
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="group"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        private StudentGroupModel GetStudentGroupModel(Group group, Student student)
        {
            return new StudentGroupModel
            {
                Number = group.Number,
                GroupId = group.Id,
                IsEnrolled = student.GroupId == group.Id
            };
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="project"></param>
        /// <param name="student"></param>
        /// <returns></returns>
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