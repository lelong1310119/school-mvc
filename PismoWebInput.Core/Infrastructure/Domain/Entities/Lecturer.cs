﻿using PismoWebInput.Core.Infrastructure.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Domain.Entities
{
    public class Lecturer : Entity<int>
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;  
        public string? Phone { get; set; }
        public string? Address { get; set; }    
        public string? Gender { get; set; }
        public string? UserId { get; set; }
        public DateTime? Birthday { get; set; }

        [ForeignKey("UserId")]
        public AppUser? AppUser { get; set; }

        public ICollection<SubjectClass> SubjectClasses { get; set; }   
    }
}
