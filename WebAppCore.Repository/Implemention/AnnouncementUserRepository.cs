using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class AnnouncementUserRepository:IAnnouncementUserRepository
	{
		private AppDbContext _appDbContext;
		private IRepository<AnnouncementUser,string> _AnnouncementUser;
		public AnnouncementUserRepository()
		{

		}
		public async Task<bool> AddASync(AnnouncementUser announcementUsers)
		{
			try
			{
				await _AnnouncementUser.AddAsyn(announcementUsers);
				return true;
			} catch(Exception)
			{

				return false;
			}
		
		}
	}
}
