using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.EF;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;

namespace WebAppCore.Application.Implementation
{
	public class SystemConfigService:ISystemConfigService
	{
		private readonly IRepository<SystemConfig,int> _systemRepository;
		private IUnitOfWork _unitOfWork;
		private readonly AppDbContext _context;

		public SystemConfigService(IRepository<SystemConfig,int> systemRepository,IUnitOfWork unitOfWork,AppDbContext context)
		{
			_systemRepository = systemRepository;
			_unitOfWork = unitOfWork;
			_context = context;
		}
		public void Create(SystemConfigViewModel slideVM)
		{
			var slide = slideVM.AddModel();
			_systemRepository.Add(slide);
		}

		public async Task<SystemConfigViewModel> GetByIdOrDefault()
		{
			var data = await _context.Set<SystemConfig>().FirstOrDefaultAsync();
			return  data.ToModel();
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(SystemConfigViewModel slideVM)
		{
			var slide = slideVM.AddModel();
			_systemRepository.Update(slide);
		}

		public async Task<SystemConfigViewModel> GetById()
		{
			var data =await _context.Set<SystemConfig>().FirstOrDefaultAsync();
			return data.ToModel();
		}
	}
}
