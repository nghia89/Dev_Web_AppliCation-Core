using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class BillDetailMapperProfile:Profile
	{
		public BillDetailMapperProfile()
		{
			CreateMap<BillDetail,BillDetailViewModel>().MaxDepth(2).ReverseMap();
		}
	}
	public static class BillDetailMapper
	{
		internal static IMapper Mapper { get; }

		static BillDetailMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<BillDetailMapperProfile>())
			   .CreateMapper();
		}

		public static BillDetailViewModel ToModel(this BillDetail BillDetail)
		{
			return Mapper.Map<BillDetailViewModel>(BillDetail);
		}
		public static BillDetail AddModel(this BillDetailViewModel addModel)
		{
			return Mapper.Map<BillDetail>(addModel);
		}
	}
}
