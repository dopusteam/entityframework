using System.Linq;
using System.Web.Mvc;
using Dopusteam.EFR.Core.Entities;
using Dopusteam.EFR.Web.Models;

namespace Dopusteam.EFR.Web.Controllers
{
    public class GroupsController : BaseController
    {
        [HttpGet]
        public JsonResult All()
        {
            var groups = this.GroupsService.GetAllGroups();

            var data = groups.Select(this.GetGroupModel);

            return this.Json(new {data}, JsonRequestBehavior.AllowGet);
        }

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

        [HttpPost]
        public JsonResult Delete(long groupId)
        {
            this.GroupsService.RemoveGroup(groupId);

            return this.Json(new { success = true });
        }

        [HttpGet]
        public JsonResult Get(long id)
        {
            var group = this.GroupsService.GetGroupById(id);

            var data = this.GetGroupModel(group);

            return this.Json(new { data }, JsonRequestBehavior.AllowGet);
        }

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