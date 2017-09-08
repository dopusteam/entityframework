using System.Collections.Generic;

namespace Dopusteam.EFR.Web.Models
{
    public class StudentFormInput
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public IList<long> ProjectIds { get; set; } = new List<long>();

        public long? GroupId { get; set; }
    }
}