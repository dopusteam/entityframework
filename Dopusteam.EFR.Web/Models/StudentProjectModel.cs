using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dopusteam.EFR.Web.Models
{
    public class StudentProjectModel
    {
        public long ProjectId { get; set; }

        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}