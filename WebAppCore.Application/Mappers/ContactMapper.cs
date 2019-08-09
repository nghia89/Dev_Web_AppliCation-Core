using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ContactMapperProfile:Profile
	{
		public ContactMapperProfile()
		{
			CreateMap<Contact,ContactViewModel>().ReverseMap();
		}
	}
	public static class ContactMapper
	{
		internal static IMapper Mapper { get; }

		static ContactMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ContactMapperProfile>())
			   .CreateMapper();
		}

		public static ContactViewModel ToModel(this Contact contact)
		{
			return Mapper.Map<ContactViewModel>(contact);
		}

		public static Contact AddModel(this ContactViewModel contact)
		{
			return Mapper.Map<Contact>(contact);
		}
	}
}
