using System.Collections.Generic;

namespace Dopusteam.EFR.Web.Models
{
    public class GroupModel
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
    }
}