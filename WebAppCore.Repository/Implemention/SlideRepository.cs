using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Repository.Interfaces;

namespace WebAppCore.Repository.Implemention
{
	public class SlideRepository:ISlideRepository
	{
		private IRepository<Slide,int> _slideRepository;
		public SlideRepository(IRepository<Slide,int> slideRepository)
		{
			this._slideRepository = slideRepository;
		}

		public async Task<List<Slide>> GetSlides(string groupAlias)
		{
			var data= await _slideRepository.FindAllAsync(x => x.Status == Status.Active && x.GroupAlias == groupAlias);
			return data.ToList();
		}
	}
}
