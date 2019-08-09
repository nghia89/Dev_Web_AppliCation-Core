using AutoMapper;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{

	public class FooterMapperProfile:Profile
	{
		public FooterMapperProfile()
		{
			CreateMap<Footer,FooterViewModel>().ReverseMap();
		}
	}
	public static class FooterMapper
	{
		internal static IMapper Mapper { get; }

		static FooterMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<FooterMapperProfile>())
			   .CreateMapper();
		}

		public static FooterViewModel ToModel(this Footer footer)
		{
			return Mapper.Map<FooterViewModel>(footer);
		}
	}
}
