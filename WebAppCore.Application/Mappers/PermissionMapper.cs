using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class PermissionMapperProfile:Profile
	{
		public PermissionMapperProfile()
		{
			CreateMap<Permission,PermissionViewModel>().ReverseMap();
		}
	}
	public static class PermissionMapper
	{
		internal static IMapper Mapper { get; }

		static PermissionMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PermissionMapperProfile>())
			   .CreateMapper();
		}

		public static PermissionViewModel ToModel(this Permission permission)
		{
			return Mapper.Map<PermissionViewModel>(permission);
		}

		public static Permission AddModel(this PermissionViewModel permission)
		{
			return Mapper.Map<Permission>(permission);
		}
	}
}
