using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;

namespace WebAppCore.Repository.Interfaces
{
	public interface IAnnouncementUserRepository
	{
		Task<bool> AddASync(AnnouncementUser announcementUsers);
	}
}
