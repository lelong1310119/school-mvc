using PismoWebInput.Core.Infrastructure.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Domain.Entities
{
    public class Subject : Entity<int>
    {
        public string Name { get; set; } = null!;

        public ICollection<SubjectClass> SubjectClasses { get; set; }
    }
}
