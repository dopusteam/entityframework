using System.Collections.Generic;
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
    }
}
