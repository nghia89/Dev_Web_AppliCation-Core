using System.Threading.Tasks;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class AnnouncementRepository:IAnnouncementRepository
	{
		private AppDbContext _appDbContext;
		private IRepository<Announcement,string> _Announcement;

		public AnnouncementRepository(AppDbContext appDbContext,IRepository<Announcement,string> Announcement)
		{
			this._appDbContext = appDbContext;
			this._Announcement = Announcement;
		}

		public async Task<Announcement> Add(Announcement announcement)
		{
			var data = await _Announcement.AddAsyn(announcement);
			return data;
		}
	}
}
