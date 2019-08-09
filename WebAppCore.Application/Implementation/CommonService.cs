using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;
using WebAppCore.Utilities.Constants;

namespace WebAppCore.Application.Implementation
{

	public class CommonService:ICommonService
	{
		IRepository<Footer,string> _footerRepository;
		IRepository<SystemConfig,string> _systemConfigRepository;
		IUnitOfWork _unitOfWork;
		IRepository<Slide,int> _slideRepository;
		ISlideRepository _slideServiceRepository;

		public CommonService(IRepository<Footer,string> footerRepository,
		   IRepository<SystemConfig,string> systemConfigRepository,
		   IUnitOfWork unitOfWork,ISlideRepository slideServiceRepository,
		   IRepository<Slide,int> slideRepository)
		{
			_footerRepository = footerRepository;
			_unitOfWork = unitOfWork;
			_systemConfigRepository = systemConfigRepository;
			_slideRepository = slideRepository;
			_slideServiceRepository = slideServiceRepository;
		}

		public FooterViewModel GetFooter()
		{
			return _footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId).ToModel();
		}

		public async Task<List<SlideShowViewModel>> GetSlides(string groupAlias)
		{
			var listData = await _slideServiceRepository.GetSlides(groupAlias);
			return listData.Select(a => a.ToModel()).ToList();
		}

		public SystemConfigViewModel GetSystemConfig(string code)
		{
			return _systemConfigRepository.FindSingle(x => x.Id == code).ToModel();
		}
	}
}
