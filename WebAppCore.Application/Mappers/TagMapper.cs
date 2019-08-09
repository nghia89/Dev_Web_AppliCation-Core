using AutoMapper;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Mappers
{
	public class TagMapperProfile:Profile
	{
		public TagMapperProfile()
		{
			CreateMap<Tag,TagViewModel>().ReverseMap();
		}
	}
	public static class TagMapper
	{
		internal static IMapper Mapper { get; }

		static TagMapper()
		{
			Mapper = new MapperConfiguration(cfg => cfg.AddProfile<TagMapperProfile>())
			   .CreateMapper();
		}

		public static TagViewModel ToModel(this Tag Tag)
		{
			return Mapper.Map<TagViewModel>(Tag);
		}
	}
}
