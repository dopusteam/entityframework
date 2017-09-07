using System.Collections.Generic;

namespace Dopusteam.EFR.Web.Models
{
    public class StudentModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public ICollection<ProjectModel> Projects { get; set; } = new List<ProjectModel>();

        public long GroupId { get; set; }

        public GroupModel Group { get; set; }
    }
}