﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PismoWebInput.Core.Infrastructure.Domain.Entities;
using PismoWebInput.Core.Infrastructure.Models.UserManager;
using PismoWebInput.Core.Persistence.Uow;
using System.Linq;
using PismoWebInput.Core.Infrastructure.Common.Extensions;
using PismoWebInput.Core.Infrastructure.Common.Mappings;
using PismoWebInput.Core.Infrastructure.Models.User;

namespace PismoWebInput.Core.Infrastructure.Services
{
    public interface IUserManagerService
    {
        Task CreateUser(CreateUserManagerModel model);
        Task<IList<UserModel>> GetAll();
        Task<EditUserManagerModel> GetUser(string userId);
        Task EditUser(EditUserManagerModel model);
        Task DeleteUser(string userId);
        Task<AppUser> Create(CreateUserManagerModel model);
    }

    public class UserManagerService : IUserManagerService
    {
        private readonly IEfUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private UserStore<AppUser> UserSet => new(_unitOfWork.DataContext);

        public UserManagerService(
            IEfUnitOfWork unitOfWork,
            UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IList<UserModel>> GetAll()
        {
            var result = await UserSet.Users
                .Where(x => x.UserName != "superadmin")
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .ToListAsync();
            return result.MapToList<UserModel>();
        }

        public async Task<EditUserManagerModel> GetUser(string userId)
        {
            var user = await UserSet.Users
                .Where(x => x.Id == userId)
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync()
                ;

            if (user == null)
            {
                throw new Exception("User does not exist");
            }

            return new EditUserManagerModel
            {
                User = user.MapTo<EditUserModel>(),
            };
        }

        public async Task<AppUser> Create(CreateUserManagerModel model)
        {
            var entity = await UserSet.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == model.User.Username.ToUpper());
            if (entity != null)
            {
                throw new Exception("User already exist");
            }

            var user = new AppUser
            {
                UserName = model.User.Username,
                NormalizedUserName = model.User.Username.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var password = new PasswordHasher<AppUser>();
            var hashed = password.HashPassword(user, model.User.Password);
            user.PasswordHash = hashed;

            await UserSet.CreateAsync(user);

            await AssignRoles(user.Id, new List<string> { model.User.Role });
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task CreateUser(CreateUserManagerModel model)
        {
            var entity = await UserSet.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == model.User.Username.ToUpper());
            if (entity != null)
            {
                throw new Exception("User already exist");
            }

            var user = new AppUser
            {
                UserName = model.User.Username,
                NormalizedUserName = model.User.Username.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var password = new PasswordHasher<AppUser>();
            var hashed = password.HashPassword(user, model.User.Password);
            user.PasswordHash = hashed;

            await UserSet.CreateAsync(user);

            await AssignRoles(user.Id, new List<string> { model.User.Role });
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task EditUser(EditUserManagerModel model)
        {
            var entity = await UserSet.Users.FirstOrDefaultAsync(x => x.Id == model.User.Id);
            if(entity == null)
            {
                throw new Exception("User does not exist");
            }

            if (entity.UserName != model.User.Username)
            {
                var checkExist = await UserSet.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == model.User.Username.ToUpper());
                if (checkExist != null)
                {
                    throw new Exception("User already exist");
                }
            }

            entity.UserName = model.User.Username;
            if (!string.IsNullOrEmpty(model.User.Password))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(entity, model.User.Password);
                entity.PasswordHash = hashed;
            }

            await UserSet.UpdateAsync(entity);

            await AssignRoles(entity.Id, new List<string> { model.User.Role });
            //await AssignOperation(entity.Id,
            //    model.Operators.SelectMany(x => x).Where(x => x.Checked && x.Id.HasValue).Select(x => x.Id.Value).ToList());
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUser(string userId)
        {
            var entity = await UserSet.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                throw new Exception("User does not exist");
            }
            await UserSet.DeleteAsync(entity);
        }

        private async Task AssignRoles(string userId, IEnumerable<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            await _userManager.AddToRolesAsync(user, roles);
        }

        //private async Task AssignOperation(string userId, ICollection<long> operations)
        //{
        //    var oldData = await UserOperationSet.Where(x => x.UserId == userId).ToListAsync();

        //    var removeItems = oldData.Where(x => !operations.Contains(x.OperationId)).ToList();
        //    var addItems = operations.Where(x => oldData.All(o => o.OperationId != x)).ToList();

        //    UserOperationSet.RemoveRange(removeItems);
        //    UserOperationSet.AddRange(addItems.Select(x => new UserOperation { UserId = userId, OperationId = x }));

        //    await _unitOfWork.SaveChangesAsync();
        //}
    }
}
