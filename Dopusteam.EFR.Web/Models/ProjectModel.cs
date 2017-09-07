using System.Collections.Generic;

namespace Dopusteam.EFR.Web.Models
{
    public class ProjectModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
    }
}