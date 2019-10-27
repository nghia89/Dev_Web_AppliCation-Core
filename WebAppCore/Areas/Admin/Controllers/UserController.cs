using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Authorization;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Extensions;
using WebAppCore.SignalR;

namespace WebAppCore.Areas.Admin.Controllers
{
	public class UserController:BaseController
	{
		private IUserService _userService;
		private readonly IAuthorizationService _authorizationService;
		private readonly IHubContext<SignalRHub> _hubContext;
		private IAnnouncementUserService _announcementUserService;
		private IAnnouncementService _announcementService;

		public UserController(IUserService userService,IAuthorizationService authorizationService,
			IHubContext<SignalRHub> hubContext,IAnnouncementUserService announcementUserService,
			IAnnouncementService announcementService)
		{
			_userService = userService;
			_authorizationService = authorizationService;
			_hubContext = hubContext;
			_announcementUserService = announcementUserService;
			_announcementService = announcementService;
		}

		public async Task<IActionResult> Index()
		{
			var result = await _authorizationService.AuthorizeAsync(User,"USER",Operations.Read);
			if(result.Succeeded == false)
				return new RedirectResult("/Admin/Login/Index");

			return View();
		}

		public IActionResult GetAll()
		{
			var model = _userService.GetAllAsync();
			return new OkObjectResult(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetById(string id)
		{
			var model = await _userService.GetById(id);

			return new OkObjectResult(model);
		}

		[HttpGet]
		public IActionResult GetAllPaging(string Keyword,int page,int pageSize)
		{
			var model = _userService.GetAllPagingAsync(Keyword,page,pageSize);
			return new OkObjectResult(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassWord(AppUserViewModel userVm,string passWordConfirm)
		{
			if(userVm.Password != passWordConfirm)
			{
				ErrorMesage e = new ErrorMesage();
				e.Message = "Mật khẩu không khớp với nhau.";
				e.Error = true;
				return new BadRequestObjectResult(e);
			}
			var res = await _userService.ChangePassWord(userVm,null);
			if(res)
			{
				return new OkObjectResult(res);
			}
			else
			{
				ErrorMesage e = new ErrorMesage();
				e.Message = "thay đổi mật khẩu không thành công.";
				e.Error = true;
				return new BadRequestObjectResult(e);
			}
		}

		[HttpPost]
		public async Task<IActionResult> SaveEntity(AppUserViewModel userVm)
		{
			var data = await _userService.GetUserWithRole("Admin");
			if(!ModelState.IsValid)
			{
				IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
				return new BadRequestObjectResult(allErrors);
			}
			var userId = User.GetUserId();
			var notificationId = Guid.NewGuid().ToString();
			var avatar = User.GetSpecificClaim("Avatar");
			if(userVm.Id == null)
			{
				var announcement = new AnnouncementViewModel() {
					Content = $"User {userVm.UserName} has been created",
					DateCreated = DateTime.Now,
					Status = Status.Active,
					Title = "User created",
					UserId = userId,
					Id = notificationId,
					Avartar = string.IsNullOrEmpty(avatar) ? "/admin-side/images/user.png" : avatar

				};
				var result = await _userService.AddAsync(userVm);
				if(result == true)
				{
					await _announcementService.AnnounSendUser(userId,announcement,data);
					foreach(var item in data)
					{
						if(item.Id != userId)
						{
							await _hubContext.Clients.User(item.Id.ToString()).SendAsync("ReceiveMessage",announcement);
						}
					}
				}
				else
				{
					return new OkObjectResult(result);
				}
			}
			else
			{
				await _userService.UpdateAsync(userVm);
			}
			return new OkObjectResult(userVm);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			if(!ModelState.IsValid)
			{
				return new BadRequestObjectResult(ModelState);
			}
			else
			{
				await _userService.DeleteAsync(id);

				return new OkObjectResult(id);
			}
		}
	}
}