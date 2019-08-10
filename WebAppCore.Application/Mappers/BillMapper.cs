using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class BillMapperProfile:Profile
	{
		public BillMapperProfile()
		{
			CreateMap<Bill,BillViewModel>().ReverseMap();
		}
	}
	public static class BillMapper
	{
		internal static IMapper Mapper { get; }

		static BillMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<BillMapperProfile>())
			   .CreateMapper();
		}

		public static BillViewModel ToModel(this Bill bill)
		{
			return Mapper.Map<BillViewModel>(bill);
		}
		public static Bill AddModel(this BillViewModel addModel)
		{
			return Mapper.Map<Bill>(addModel);
		}
	}
}
