using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class UserMapperProfile:Profile
	{
		public UserMapperProfile()
		{
			CreateMap<AppUser,AppUserViewModel>().ReverseMap();
		}
	}
	public static class UserMapperMapper
	{
		internal static IMapper Mapper { get; }

		static UserMapperMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserMapperProfile>())
			   .CreateMapper();
		}

		public static AppUserViewModel ToModel(this AppUser user)
		{
			return Mapper.Map<AppUserViewModel>(user);
		}
	}
}
