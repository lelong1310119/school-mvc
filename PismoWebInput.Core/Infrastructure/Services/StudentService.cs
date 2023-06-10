using Microsoft.EntityFrameworkCore;
using PismoWebInput.Core.Infrastructure.Common.Mappings;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Models.LecturerModel;
using PismoWebInput.Core.Infrastructure.Models.StudentModel;
using PismoWebInput.Core.Infrastructure.Models.Tuition;
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
    public interface IStudentService
    {
        Task<IList<StudentModel>> GetAll();
        Task Create(StudentModel model);
        Task<StudentModel> GetStudent(string Code);
        Task<float> GetTotal(string Code);

        Task<IList<SubjectClass>> GetRegisteredSubjectClass(string userId);
        Task<IList<SubjectClass>> GetUnregisteredSubjectClass(string userId);
    }
    public class StudentService : IStudentService
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly EfContext _context;
        private DbSet<Student> _studentSet => _unitOfWork.Set<Student>();
        public StudentService(IEfUnitOfWork unitOfWork, IUserManagerService userManagerService, EfContext context)
        {
            _unitOfWork = unitOfWork;
            _userManagerService = userManagerService;
            _context = context;
        }

        public async Task<IList<StudentModel>> GetAll()
        {
            return (await _studentSet.ToListAsync()).MapToList<StudentModel>();
        }

        public async Task<StudentModel> GetStudent(string code)
        {
            var student = await _studentSet.FirstOrDefaultAsync(x => x.StudentCode == code);
            return student.MapTo<StudentModel>();
        }

        public async Task<float> GetTotal(string code)
        {

            var input = "20230001";
            //var result = _context.Set<TuitionModel>()
            //    .FromSqlInterpolated($"SELECT dbo.MyFunction({input}) AS Result")
            //    .Select(x => x.total)
            //    .FirstOrDefault();
            return 13;
        }

        public async Task Create(StudentModel model)
        {
            var entity = model.MapTo<Student>();
            var existing = await _studentSet.FirstOrDefaultAsync(x => x.StudentCode == model.StudentCode);
            if (existing != null)
            {
                throw new Exception("Content already exist!");
            }
            CreateUserModel user = new CreateUserModel();
            user.Username = model.StudentCode;
            user.Password = "Open4me!";
            user.ConfirmPassword = "Open4me!";
            user.Role = "Student";
            CreateUserManagerModel createUserManagerModel = new CreateUserManagerModel();
            createUserManagerModel.User = user;
            var createdUser = await _userManagerService.Create(createUserManagerModel);
            entity.UserId = createdUser.Id;
            _studentSet.Add(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<SubjectClass>> GetRegisteredSubjectClass(string userId)
        {
            var student = await _context.Students
                            .Where(x => x.UserId == userId)
                            .FirstOrDefaultAsync();
            var registeredClasses = _context.SubjectClasses
                .FromSqlRaw($"EXEC GetRegisteredClass @studentId = {student.Id}")
                //.Include(x => x.Category)
                //.Include(x => x.Subject)
                //.Include(x => x.Semester)
                .ToList();
            foreach(var item in registeredClasses)
            {
                item.Subject = _context.Subjects
                    .FirstOrDefault(x => x.Id == item.SubjectId);
                item.Category = _context.Categories
                    .FirstOrDefault(x => x.Id == item.CategoryId);
                item.Lecturer = _context.Lecturers
                    .FirstOrDefault(x => x.Id == item.LecturerId);
            }

            return registeredClasses;
        }

        public async Task<IList<SubjectClass>> GetUnregisteredSubjectClass(string userId)
        {
            var student = await _context.Students
                            .Where(x => x.UserId == userId)
                            .FirstOrDefaultAsync();
            var unregisteredClasses = _context.SubjectClasses
                    .FromSqlRaw($"EXEC GetUnregisteredClass @studentId = {student.Id}")
                    .ToList();
            foreach (var item in unregisteredClasses)
            {
                item.Subject = _context.Subjects
                    .FirstOrDefault(x => x.Id == item.SubjectId);
                item.Category = _context.Categories
                    .FirstOrDefault(x => x.Id == item.CategoryId);
                item.Lecturer = _context.Lecturers
                    .FirstOrDefault(x => x.Id == item.LecturerId);
            }
            return unregisteredClasses;
        }
    }
}
