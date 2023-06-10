using PismoWebInput.Core.Infrastructure.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Domain.Entities
{
    public class SubjectClass : Entity<int>
    {
        public int SubjectId { get; set; }  
        public int SemesterId { get; set; } 
        public int CategoryId { get; set; }
        public int LecturerId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [ForeignKey("SemesterId")]
        public Semester? Semester { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("LecturerId")]
        public Lecturer? Lecturer { get; set; }

        public ICollection<RegisCourse> RegisCourses { get; set; }
    }
}
