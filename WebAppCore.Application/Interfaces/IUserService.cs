﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Interfaces
{
	public interface IUserService
	{
		Task<bool> AddAsync(AppUserViewModel userVm);

		Task<bool> ChangePassWord(AppUserViewModel userVm,string passWordConfirm);

		Task DeleteAsync(string id);

		Task<List<AppUserViewModel>> GetAllAsync();

		Task<bool> GetAll(string Email);

		PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword,int page,int pageSize);

		Task<AppUserViewModel> GetById(string id);

		Task UpdateAsync(AppUserViewModel userVm);

		Task<List<AppUserViewModel>> GetUserWithRole(string roleName);
	}
}
