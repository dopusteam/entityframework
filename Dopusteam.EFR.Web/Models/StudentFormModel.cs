using System.Collections.Generic;

namespace Dopusteam.EFR.Web.Models
{
    public class StudentFormModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public ICollection<StudentProjectModel> Projects { get; set; } = new List<StudentProjectModel>();

        public ICollection<StudentGroupModel> Groups { get; set; } = new List<StudentGroupModel>();
    }
}