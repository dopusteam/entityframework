using System.Collections.Generic;

namespace Dopusteam.EFR.Core.Entities
{
    public class Project
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
