using System.Collections.Generic;

namespace Dopusteam.EFR.Core.Entities
{
    public class Group
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
