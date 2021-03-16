using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.Common;

namespace WebAppCore.Application.Interfaces
{
	public interface ISystemConfigService
	{
		void Create(SystemConfigViewModel slideVM);
		void Update(SystemConfigViewModel slideVM);
		Task<SystemConfigViewModel> GetByIdOrDefault();
		Task<SystemConfigViewModel> GetById();
		void Save();
	}
}
