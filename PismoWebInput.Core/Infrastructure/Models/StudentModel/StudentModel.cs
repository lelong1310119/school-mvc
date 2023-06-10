using AutoMapper;
using PismoWebInput.Core.Infrastructure.Common.Mappings;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Models.StudentModel
{
    public class StudentModel : ICustomMappings
    {
        public int? Id { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "StudentCode must be a number")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "StudentCode must be 8 digits")]
        public string StudentCode { get; set; } = null!;

        public string Name { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? UserId { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Student, StudentModel>();
            configuration.CreateMap<StudentModel, Student>();
        }
    }
}
