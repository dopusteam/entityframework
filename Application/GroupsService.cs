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

        public GroupsService()
        {
            this._dbContext= new EfrDbContext();
        }

        public Group GetGroupById(long? groupId)
        {
            if (!groupId.HasValue)
            {
                return null;
            }

            var group = this._dbContext.Groups.Find(groupId.Value);

            return group;
        }

        public IList<Group> GetAllGroups()
        {
            return this._dbContext.Groups.ToList();
        }

        public void CreateGroup(Group group)
        {
            this._dbContext.Groups.Add(group);

            this._dbContext.SaveChanges();
        }

        public void UpdateGroup(Group group)
        {
            var existedGroup = this._dbContext.Groups.Find(group.Id);

            existedGroup.Number = group.Number;

            this._dbContext.Groups.AddOrUpdate(group);

            this._dbContext.SaveChanges();
        }

        public void RemoveGroup(long groupId)
        {
            var existedGroup = this._dbContext.Groups.Find(groupId);

            this._dbContext.Students.Where(student => student.GroupId == groupId).Load();

            this._dbContext.Groups.Remove(existedGroup);

            this._dbContext.SaveChanges();
        }
    }
}
