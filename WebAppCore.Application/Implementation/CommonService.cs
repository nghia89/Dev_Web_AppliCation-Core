using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Utilities.Constants;

namespace WebAppCore.Application.Implementation
{
   
    public class CommonService:ICommonService
    {
        IRepository<Footer, string> _footerRepository;
        IRepository<SystemConfig, string> _systemConfigRepository;
        IUnitOfWork _unitOfWork;
        IRepository<Slide, int> _slideRepository;

        public CommonService(IRepository<Footer, string> footerRepository,
           IRepository<SystemConfig, string> systemConfigRepository,
           IUnitOfWork unitOfWork,
           IRepository<Slide, int> slideRepository)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
            _systemConfigRepository = systemConfigRepository;
            _slideRepository = slideRepository;
        }

        public FooterViewModel GetFooter()
        {
            return Mapper.Map<Footer, FooterViewModel>(_footerRepository.FindSingle(x => x.Id ==
            CommonConstants.DefaultFooterId));
        }

        public List<SlideShowViewModel> GetSlides(string groupAlias)
        {
            return _slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias)
                .ProjectTo<SlideShowViewModel>().ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            return Mapper.Map<SystemConfig, SystemConfigViewModel>(_systemConfigRepository.FindSingle(x => x.Id == code));
        }
    }
}
