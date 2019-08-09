using AutoMapper;
using System.Linq;
using WebAppCore.Application.ViewModels.Product;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class ProductMapperProfile:Profile
	{
		public ProductMapperProfile()
		{
			CreateMap<Product,ProductViewModel>().ForMember(a => a.ProductTags,o => o.ResolveUsing(b => b.ProductTags == null
				? null: b.ProductTags.Select(c => c.ToModel()).ToList())).MaxDepth(2)
				.ReverseMap();
		}
	}
	public static class ProductMapper
	{
		internal static IMapper Mapper { get; }

		static ProductMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ProductMapperProfile>())
			   .CreateMapper();
		}

		public static ProductViewModel ToModel(this Product product)
		{
			return Mapper.Map<ProductViewModel>(product);
		}

		public static Product AddModel(this ProductViewModel addModel)
		{
			return Mapper.Map<Product>(addModel);
		}

		public static ProductImageViewModel ProductImage(this ProductImage addModel)
		{
			return Mapper.Map<ProductImageViewModel>(addModel);
		}
	}
}
