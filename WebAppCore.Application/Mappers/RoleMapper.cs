using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class  RoleMapperProfile:Profile
	{
		public RoleMapperProfile()
		{
			CreateMap<AppRole,AppRoleViewModel>().ReverseMap();
		}
	}
	public static class RoleMapper
	{
		internal static IMapper Mapper { get; }

		static RoleMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<RoleMapperProfile>())
			   .CreateMapper();
		}

		public static AppRoleViewModel ToModel(this AppRole appRole)
		{
			return Mapper.Map<AppRoleViewModel>(appRole);
		}

		public static AppRole AddModel(this AppRoleViewModel appRole)
		{
			return Mapper.Map<AppRole>(appRole);
		}
	}
}
