using PismoWebInput.Core.Infrastructure.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PismoWebInput.Core.Infrastructure.Models.LecturerModel;
using PismoWebInput.Core.Infrastructure.Common.Mappings;

namespace PismoWebInput.Core.Infrastructure.Models.LecturerModel
{
    public class LecturerModel : ICustomMappings
    {
        public int? Id { get; set; }
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
            configuration.CreateMap<Lecturer, LecturerModel>();
            configuration.CreateMap<LecturerModel, Lecturer>();
        }
    }
}

