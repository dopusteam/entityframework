using System.Collections.Generic;

namespace Dopusteam.EFR.Core.Entities
{
    public class Student
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public long? GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
