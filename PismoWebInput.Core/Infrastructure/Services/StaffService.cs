using Microsoft.EntityFrameworkCore;
using PismoWebInput.Core.Infrastructure.Common.Mappings;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Models.StudentModel;
using PismoWebInput.Core.Infrastructure.Models.User;
using PismoWebInput.Core.Infrastructure.Models.UserManager;
using PismoWebInput.Core.Persistence.Contexts;
using PismoWebInput.Core.Persistence.Uow;


namespace PismoWebInput.Core.Infrastructure.Services
{
    public interface IStaffService
    {
        Task<Lecturer> GetAll(int id);
        Task<IList<SubjectClass>> GetSubjectClass(int id);
    }
    public class StaffService : IStaffService
    {
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly EfContext _context;
        public StaffService(IEfUnitOfWork unitOfWork, EfContext efContext)
        {
            _unitOfWork = unitOfWork;
            _context = efContext;
        }

        public async Task<Lecturer> GetAll(int id)
        {
            return await _context.Lecturers
                .Include(x => x.SubjectClasses).ThenInclude(x => x.RegisCourses)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<SubjectClass>> GetSubjectClass(int id)
        {
            return await _context.SubjectClasses
                .Include(x => x.Category)
                .Include(x => x.Subject)
                .ToListAsync();
        }
    }
}
