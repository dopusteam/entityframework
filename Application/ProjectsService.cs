using System;
using System.Collections.Generic;
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

        public IList<Project> GetProjectsByIds(IList<long> projectIds)
        {
            var projects = this._dbContext
                .Projects
                .Where(project => projectIds.Contains(project.Id))
                .ToList();

            return projects;
        }

        public IList<Project> GetAllProjects()
        {
            return this._dbContext.Projects.ToList();
        }
    }
}
