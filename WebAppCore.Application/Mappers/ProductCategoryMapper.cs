using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ProductCategoryMapperProfile:Profile
	{
		public ProductCategoryMapperProfile()
		{
			CreateMap<ProductCategory, ProductCategoryViewModel>().MaxDepth(2).ReverseMap();
			//CreateMap<ProductCategory,ProductCategoryViewModel>().ForMember(a=>a.Products,o=>o.ResolveUsing(b=>b.Products!=null
			//? b.Products.Select(c=>c.ToModel()).ToList():null)).MaxDepth(2).ReverseMap();
		}
	}
	public static class ProductCategoryMapper
	{
		internal static IMapper Mapper { get; }

		static ProductCategoryMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProductCategoryMapperProfile>())
			   .CreateMapper();
		}

		public static ProductCategoryViewModel ToModel(this ProductCategory productCategory)
		{
			return Mapper.Map<ProductCategoryViewModel>(productCategory);
		}

		//public static Product ToModel(this ProductCategory productVm)
		//{
		//	return Mapper.Map<Product>(productVM);
		//}

		public static ProductCategory AddModel(this ProductCategoryViewModel addModel)
		{
			return Mapper.Map<ProductCategory>(addModel);
		}
	}
}
