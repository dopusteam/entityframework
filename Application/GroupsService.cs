using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.EntityFramework;

namespace Application
{
    public class GroupsService
    {
        private readonly EfrDbContext _dbContext;

        /// <summary>
        /// Конструктор
        /// </summary>
        public GroupsService()
        {
            // при создании сервиса, сразу создаём экземпляр контекста
            // контекст - это по сути наш метод работы с БД
            this._dbContext= new EfrDbContext();
        }

        /// <summary>
        /// Получения группы по Id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Group GetGroupById(long? groupId)
        {
            // если параметр равен null, то возвращаем null
            if (!groupId.HasValue)
            {
                return null;
            }

            // метод find ищет сущность по первичному ключу
            var group = this._dbContext.Groups.Find(groupId.Value);

            return group;
        }

        /// <summary>
        /// Получение списка всех групп
        /// </summary>
        /// <returns></returns>
        public IList<Group> GetAllGroups()
        {
            // для получения всех сущностей, достаточно вызвать метод ToList у нужного DbSet
            return this._dbContext.Groups.ToList();
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="group"></param>
        public void CreateGroup(Group group)
        {
            // всё просто, добавляем группу
            this._dbContext.Groups.Add(group);

            // сохраняем изменения
            this._dbContext.SaveChanges();
        }

        /// <summary>
        /// Обновление группы
        /// </summary>
        /// <param name="group"></param>
        public void UpdateGroup(Group group)
        {
            // достаём группу из БД
            var existedGroup = this._dbContext.Groups.Find(group.Id);

            // обновляем её номер
            existedGroup.Number = group.Number;

            // отправляем обратно в контекст
            this._dbContext.Groups.AddOrUpdate(group);

            // сохраняем
            this._dbContext.SaveChanges();
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="groupId"></param>
        public void RemoveGroup(long groupId)
        {
            // достаём группу
            var existedGroup = this._dbContext.Groups.Find(groupId);

            // тут не совсем очевидно.
            // у студента есть свойство GroupId, которое nullable
            // по идее, при удалении группы, у студента должно свойство groupId стать null
            // но суть в том, что EF фактически работает только с сущностями, которые сейчас загружены
            // т.е. для того, чтоб при удалении группы, всем студентам, состоящим в этой группе,
            // EF обновил свойство GroupId, надо их явно загрузить в память
            this._dbContext.Students.Where(student => student.GroupId == groupId).Load();

            // дальше можно спокойно удалить группу
            this._dbContext.Groups.Remove(existedGroup);

            // и сохранить всё
            this._dbContext.SaveChanges();
        }
    }
}
