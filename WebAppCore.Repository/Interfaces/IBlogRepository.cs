using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;

namespace WebAppCore.Repository.Interfaces
{
	public interface IBlogRepository
	{
		Task<List<Blog>> GetLastest(int top);
	}
}
