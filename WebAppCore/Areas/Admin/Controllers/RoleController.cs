using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Authorization;
using WebAppCore.Extensions;
using WebAppCore.SignalR;

namespace WebAppCore.Areas.Admin.Controllers
{
	public class RoleController:BaseController
	{
		private readonly IRoleService _roleService;
		private readonly IAuthorizationService _authorizationService;
		private readonly IHubContext<SignalRHub> _hubContext;
		private readonly IUserService _IUserService;
		private IAnnouncementService _announcementService;
		public RoleController(IRoleService roleService,IAuthorizationService authorizationService,
		   IHubContext<SignalRHub> hubContext,IUserService IUserService,IAnnouncementService announcementService)
		{
			_roleService = roleService;
			_authorizationService = authorizationService;
			_hubContext = hubContext;
			_IUserService = IUserService;
			_announcementService = announcementService;
		}
		public async Task<IActionResult> Index()
		{
			var result = await _authorizationService.AuthorizeAsync(User,"USER",Operations.Read);
			if(result.Succeeded == false)
				return new RedirectResult("/Admin/Login/Index");

			return View();
		}

		public async Task<IActionResult> GetAll()
		{
			var model = await _roleService.GetAllAsync();

			return new OkObjectResult(model);
		}
		[HttpGet]
		public async Task<IActionResult> GetById(Guid id)
		{
			var model = await _roleService.GetById(id);

			return new OkObjectResult(model);
		}

		[HttpGet]
		public IActionResult GetAllPaging(string keyword,int page,int pageSize)
		{
			var model = _roleService.GetAllPagingAsync(keyword,page,pageSize);
			return new OkObjectResult(model);
		}

		[HttpPost]
		public async Task<IActionResult> SaveEntity(AppRoleViewModel roleVm)
		{
			var data = await _IUserService.GetUserWithRole("Admin");
			if(!ModelState.IsValid)
			{
				IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
				return new BadRequestObjectResult(allErrors);
			}
			if(roleVm.Id == null)
			{
				var avatar = User.GetSpecificClaim("Avatar");
				var notificationId = Guid.NewGuid().ToString();
				var userId = User.GetUserId();
				var announcement = new AnnouncementViewModel() {
					Title = "Role created",
					DateCreated = DateTime.Now,
					Content = $"Role {roleVm.Name} has been created",
					Id = notificationId,
					UserId = userId,
					Avartar = string.IsNullOrEmpty(avatar) ? "/admin-side/images/user.png" : avatar
				};
				
				await _announcementService.AnnounSendUser(userId,announcement,data);
				foreach(var item in data)
				{
					if(item.Id != userId)
					{
						var announcementUser = null as AnnouncementUserViewModel;
						if(item.Id != userId)
						{
							await _hubContext.Clients.User(item.Id.ToString()).SendAsync("ReceiveMessage",announcement);
						}
					}

				}
				await _roleService.AddAsync(roleVm);
			}
			else
			{
				await _roleService.UpdateAsync(roleVm);
			}
			return new OkObjectResult(roleVm);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			if(!ModelState.IsValid)
			{
				return new BadRequestObjectResult(ModelState);
			}
			await _roleService.DeleteAsync(id);
			return new OkObjectResult(id);
		}


		[HttpPost]
		public IActionResult ListAllFunction(Guid roleId)
		{
			var functions = _roleService.GetListFunctionWithRole(roleId);
			return new OkObjectResult(functions);
		}

		[HttpPost]
		public IActionResult SavePermission(List<PermissionViewModel> listPermmission,Guid roleId)
		{
			_roleService.SavePermission(listPermmission,roleId);
			return new OkResult();
		}
	}
}