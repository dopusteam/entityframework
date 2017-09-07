using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dopusteam.EFR.Core.Entities;

namespace Dopusteam.EFR.EntityFramework
{
    public class EfrDbInitializer : DropCreateDatabaseAlways<EfrDbContext>
    {
        protected override void Seed(EfrDbContext context)
        {
            var firstGroup = new Group
            {
                Number = "2112"
            };

            var secondGroup = new Group
            {
                Number = "2113"
            };

            context.Groups.Add(firstGroup);
            context.Groups.Add(secondGroup);

            var microsoftProject = new Project
            {
                Name = "MicrosoftWindows",
                Students = new List<Student>
                {
                    new Student { Name = "Bill Gates", Group = firstGroup }
                }
            };
            var linuxProject = new Project { Name = "Linux" };
            var macOsProject = new Project
            {
                Name = "macOS",
                Students = new List<Student>
                {
                    new Student { Name = "Steve Jobs", Group = secondGroup }
                }
            };

            context.Projects.Add(macOsProject);
            context.Projects.Add(linuxProject);
            context.Projects.Add(microsoftProject);

            base.Seed(context);
        }
    }
}
