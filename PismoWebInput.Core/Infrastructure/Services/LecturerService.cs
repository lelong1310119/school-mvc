using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PismoWebInput.Core.Infrastructure.Common.Exceptions;
using PismoWebInput.Core.Infrastructure.Common.Extensions;
using PismoWebInput.Core.Infrastructure.Common.Mappings;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Models.LecturerModel;
using PismoWebInput.Core.Infrastructure.Models.User;
using PismoWebInput.Core.Infrastructure.Models.UserManager;
using PismoWebInput.Core.Persistence.Contexts;
using PismoWebInput.Core.Persistence.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PismoWebInput.Core.Infrastructure.Services
{
    public interface ILecturerService
    {
        Task<IList<LecturerModel>> GetAll();
        Task Create(LecturerModel model);
        Task<IList<SubjectClass>> GetSubjectClass(string id);
        Task<IList<RegisCourse>> GetRegisCode(int id);
    }

    public class LecturerService : ILecturerService
    {
        private readonly IUserManagerService _userManagerService;   
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly EfContext _context;
        private DbSet<Lecturer> _LecturerSet => _unitOfWork.Set<Lecturer>();
        public LecturerService(IEfUnitOfWork unitOfWork, IUserManagerService userManagerService, EfContext efContext)
        {
            _unitOfWork = unitOfWork;
            _userManagerService = userManagerService;
            _context = efContext;
        }

        public async Task<IList<LecturerModel>> GetAll()
        {
            return (await _LecturerSet.ToListAsync()).MapToList<LecturerModel>();
        }

        
        public async Task Create(LecturerModel model)
        {
            var entity = model.MapTo<Lecturer>();
            var existing = await _LecturerSet.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (existing != null)
            {
                throw new Exception("Content already exist!");
            }
            CreateUserModel user = new CreateUserModel();
            user.Username = model.Email;
            user.Password = "Open4me!";
            user.ConfirmPassword = "Open4me!";
            user.Role = "Lecturer";
            CreateUserManagerModel createUserManagerModel = new CreateUserManagerModel();
            createUserManagerModel.User = user;
            var createdUser = await _userManagerService.Create(createUserManagerModel);
            entity.UserId= createdUser.Id;
            _LecturerSet.Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<SubjectClass>> GetSubjectClass(string userId)
        {
            var lecturer = await _context.Lecturers
                            .Where(x => x.UserId == userId)
                            .FirstOrDefaultAsync();
            return await _context.SubjectClasses
                .Where(x => x.LecturerId == lecturer.Id)
                .Include(x => x.Category)
                .Include(x => x.Subject)
                .Include(x => x.Semester)
                .ToListAsync();
        }

        public async Task<IList<RegisCourse>> GetRegisCode(int id)
        {
            return await _context.RegisCourses
                .Where(x => x.SubjectClassId == id)
                .Include(x => x.Student)
                .ToListAsync();
        }
    }
}
