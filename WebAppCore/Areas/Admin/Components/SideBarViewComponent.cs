using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Extensions;
using WebAppCore.Utilities.Constants;

namespace WebAppCore.Areas.Admin.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        IFunctionService _functionService;
        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
				var listRole = roles.Split(";");
				var listfunctions = await _functionService.GetAllFunctionByRole(listRole);
				functions = listfunctions.Select(x => new FunctionViewModel() {
					Id=x.Id,
					Name=x.Name,
					IconCss=x.IconCss,
					ParentId=x.ParentId,
					SortOrder=x.SortOrder,
					Status=x.Status,
					URL=x.URL
				}).ToList();
			}
            return View(functions);
        }
    }
}