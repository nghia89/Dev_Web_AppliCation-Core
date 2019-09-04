using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;
using WebAppCore.Utilities.Dtos;

namespace WebAppCore.Application.Implementation
{
	public class AnnouncementService:IAnnouncementService
	{
		private IRepository<Announcement,string> _announcementIRepository;
		private IRepository<AnnouncementUser,int> _announcementUserRepository;
		private IAnnouncementRepository _announcementRepository;
		private IUnitOfWork _unitOfWork;

		public AnnouncementService(IRepository<Announcement,string> announcementIRepository,
			IRepository<AnnouncementUser,int> announcementUserRepository,
			IUnitOfWork unitOfWork,IAnnouncementRepository announcementRepository)
		{
			_announcementUserRepository = announcementUserRepository;
			this._announcementIRepository = announcementIRepository;
			this._unitOfWork = unitOfWork;
			this._announcementRepository = announcementRepository;
		}

		public async Task<bool> AnnounSendUser(Guid userId,AnnouncementViewModel content,List<AppUserViewModel> appUsers)
		{
			var announcement = content.AddModel();
			await _announcementRepository.Add(announcement);
			foreach(var item in appUsers)
			{
				if(item.Id != userId)
				{
					var announcementUser = null as AnnouncementUserViewModel;
					announcementUser = new AnnouncementUserViewModel() { AnnouncementId = content.Id,HasRead = false,UserId = item.Id.Value };
					var user = announcementUser.AddModel();
					await _announcementUserRepository.AddAsyn(user);
				}
			}
			_unitOfWork.Commit();
			return true;
		}

		public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId,int pageIndex,int pageSize)
		{
			var announcement = _announcementIRepository.FindAll();
			var announcementUser = _announcementUserRepository.FindAll();
			var query = from x in announcement
						join y in announcementUser
							on x.Id equals y.AnnouncementId
							into xy
						from annonUser in xy.DefaultIfEmpty()
						where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
						select x;
			int totalRow = query.Count();

			var model = query.OrderByDescending(x => x.DateCreated)
				.Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(x => x.ToModel()).ToList();

			var paginationSet = new PagedResult<AnnouncementViewModel> {
				Results = model,
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};

			return paginationSet;
		}

		public bool MarkAsRead(Guid userId,string id)
		{
			bool result = false;
			var announ = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
																			   && x.UserId == userId);
			if(announ == null)
			{
				_announcementUserRepository.Add(new AnnouncementUser {
					AnnouncementId = id,
					UserId = userId,
					HasRead = true
				});
				result = true;
			}
			else
			{
				if(announ.HasRead == false)
				{
					announ.HasRead = true;
					result = true;
				}

			}
			return result;
		}
	}
}
