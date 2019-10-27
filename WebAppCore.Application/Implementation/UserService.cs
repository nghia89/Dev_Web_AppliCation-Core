using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Implementation
{
	public class UserService:IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly AppDbContext _context;

		public UserService(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager,AppDbContext context)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}

		public async Task<bool> AddAsync(AppUserViewModel userVm)
		{
			var user = new AppUser() {
				UserName = userVm.UserName,
				Avatar = userVm.Avatar,
				Email = userVm.Email,
				FullName = userVm.FullName,
				DateCreated = DateTime.Now,
				PhoneNumber = userVm.PhoneNumber,
				Status = userVm.Status
			};
			var result = await _userManager.CreateAsync(user,userVm.Password);
			if(result.Succeeded && userVm.Roles.Count > 0)
			{
				var appUser = await _userManager.FindByNameAsync(user.UserName);
				if(appUser != null)
					await _userManager.AddToRolesAsync(appUser,userVm.Roles);
			}
			if(result.Succeeded == false)
			{
				return false;
			}
			else
			{
				return true;
			}

		}

		public async Task DeleteAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			await _userManager.DeleteAsync(user);
		}

		public async Task<bool> GetAll(string Email)
		{
			var CheckUser = await _userManager.Users.CountAsync(x => x.Email == Email) > 0;
			return false;
		}

		public async Task<List<AppUserViewModel>> GetAllAsync()
		{
			return await _userManager.Users.Select(a => a.ToModel()).ToListAsync();
		}

		public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword,int page,int pageSize)
		{
			var query = _userManager.Users;
			if(!string.IsNullOrEmpty(keyword))
				query = query.Where(x => x.FullName.Contains(keyword)
				|| x.UserName.Contains(keyword)
				|| x.Email.Contains(keyword));

			int totalRow = query.Count();
			query = query.Skip((page - 1) * pageSize)
			   .Take(pageSize);

			var data = query.Select(x => new AppUserViewModel() {
				UserName = x.UserName,
				Avatar = x.Avatar,
				BirthDay = x.BirthDay.ToString(),
				Email = x.Email,
				FullName = x.FullName,
				Id = x.Id,
				PhoneNumber = x.PhoneNumber,
				Status = x.Status,
				DateCreated = x.DateCreated
			}).ToList();
			var paginationSet = new PagedResult<AppUserViewModel>() {
				Results = data,
				CurrentPage = page,
				RowCount = totalRow,
				PageSize = pageSize
			};

			return paginationSet;
		}

		public async Task<AppUserViewModel> GetById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var roles = await _userManager.GetRolesAsync(user);
			var userVm = user.ToModel();
			userVm.Roles = roles.ToList();
			return userVm;
		}

		//public async Task UpdateAsync(AppUserViewModel userVm)
		//{
		//    var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
		//    //Remove current roles in db
		//    var currentRoles = await _userManager.GetRolesAsync(user);

		//    var result = await _userManager.AddToRolesAsync(user,
		//        userVm.Roles.Except(currentRoles).ToArray());

		//    if (result.Succeeded)
		//    {
		//        string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
		//        await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

		//        //Update user detail
		//        user.FullName = userVm.FullName;
		//        user.Status = userVm.Status;
		//        user.Email = userVm.Email;
		//        user.PhoneNumber = userVm.PhoneNumber;
		//        await _userManager.UpdateAsync(user);
		//    }
		//}
		public async Task UpdateAsync(AppUserViewModel userVm)
		{
			var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
			//Remove current roles in db
			var currentRoles = await _userManager.GetRolesAsync(user);

			await RemoveRolesFromUser(userVm.Id.ToString(),currentRoles.ToArray());

			user.FullName = userVm.FullName;
			user.Status = userVm.Status;
			user.Email = userVm.Email;
			user.PhoneNumber = userVm.PhoneNumber;

			await _userManager.UpdateAsync(user);

			await _userManager.AddToRolesAsync(user,userVm.Roles);

		}

		public async Task RemoveRolesFromUser(string userId,string[] roles)
		{
			var user = await _userManager.FindByIdAsync(userId);
			var roleIds = _roleManager.Roles.Where(x => roles.Contains(x.Name)).Select(x => x.Id).ToList();
			List<IdentityUserRole<Guid>> userRoles = new List<IdentityUserRole<Guid>>();
			foreach(var roleId in roleIds)
			{
				userRoles.Add(new IdentityUserRole<Guid> { RoleId = roleId,UserId = user.Id });
			}
			_context.UserRoles.RemoveRange(userRoles);
			await _context.SaveChangesAsync();

		}
		
		public async Task<List<AppUserViewModel>> GetUserWithRole(string roleName)
		{
			var role = await _context.AppRoles.Where(n => n.Name.Equals(roleName)).SingleOrDefaultAsync();
			var userRoles = _context.UserRoles.Where(n => n.RoleId == role.Id);

			var lst = new List<AppUser>();

			foreach(var item in userRoles)
			{
				var user= await _userManager.FindByIdAsync(item.UserId.ToString());
				lst.Add(user);
			}

			var lstViewModel = lst.Select(a => a.ToModel()).ToList();

			return lstViewModel;
		}

		public async Task<bool> ChangePassWord(AppUserViewModel userVm,string passWordConfirm)
		{
			var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
			if(user == null) { /**/ }

			// compute the new hash string
			var newPassword = _userManager.PasswordHasher.HashPassword(user,userVm.Password);
			user.PasswordHash = newPassword;
			var res = await _userManager.UpdateAsync(user);

			if(res.Succeeded)
			{
				return true;
			}
			return false;
		}
	}
}