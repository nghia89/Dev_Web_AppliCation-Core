using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Application.Implementation
{
	public class AnnouncementUserService:IAnnouncementUserService
	{
		private IRepository<Announcement,string> _announcementIRepository;
		private IAnnouncementUserRepository _announcementUserRepository;
		private IUnitOfWork _unitOfWork;

		public AnnouncementUserService(IRepository<Announcement,string> announcementIRepository
			,IAnnouncementUserRepository announcementUserRepository,IUnitOfWork unitOfWork)
		{
			this._announcementIRepository = announcementIRepository;
			this._announcementUserRepository = announcementUserRepository;
			this._unitOfWork = unitOfWork;
		}
		public async Task Add(AnnouncementUserViewModel announcement)
		{
			var result= await _announcementUserRepository.AddASync(announcement.AddModel());
			if(result == true)
				_unitOfWork.Commit();
		}

	}
}
