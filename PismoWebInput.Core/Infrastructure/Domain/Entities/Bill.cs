using PismoWebInput.Core.Infrastructure.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Domain.Entities
{
    public class Bill : AuditedEntity<int>
    {
        public int StudentId { get; set; }
        public float AmountMoney { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
