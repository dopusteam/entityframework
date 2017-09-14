using System.Linq;
using System.Web.Mvc;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class GroupsController : BaseController
    {
        /// <summary>
        /// Получение списка всех групп
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult All()
        {
            var groups = this.GroupsService.GetAllGroups();

            var data = groups.Select(this.GetGroupModel);

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="groupInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create(GroupFormInput groupInput)
        {
            var group = new Group
            {
                Number = groupInput.Number
            };

            this.GroupsService.CreateGroup(group);

            return this.Json(new {success = true});
        }

        /// <summary>
        /// Обновление группы
        /// </summary>
        /// <param name="groupInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(GroupFormInput groupInput)
        {
            var group = new Group
            {
                Id = groupInput.Id,
                Number = groupInput.Number
            };

            this.GroupsService.UpdateGroup(group);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(long groupId)
        {
            this.GroupsService.RemoveGroup(groupId);

            return this.Json(new { success = true });
        }

        /// <summary>
        /// Получение группы по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get(long id)
        {
            var group = this.GroupsService.GetGroupById(id);

            var data = this.GetGroupModel(group);

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Создание экземпляра класса, который может быть отправлен на фронт
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private GroupInfoModel GetGroupModel(Group group)
        {
            return new GroupInfoModel
            {
                Id = group.Id,
                Number = group.Number,
                StudentsAmount = this.StudentsService.GetGroupStudentsCount(group.Id)
            };
        }
    }
}