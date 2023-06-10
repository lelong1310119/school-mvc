using PismoWebInput.Core.Infrastructure.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Domain.Entities
{
    public class RegisCourse : Entity<int>
    {
        public int SubjectClassId { get; set; }
        public int StudentId { get; set; }  
        public float? Score { get; set; }

        [ForeignKey("SubjectClassId")]
        public SubjectClass SubjectClass { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
