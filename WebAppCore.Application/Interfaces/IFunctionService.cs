using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;

namespace WebAppCore.Application.Interfaces
{
	public interface IFunctionService:IDisposable
	{
		void Add(FunctionViewModel function);

		Task<List<FunctionViewModel>> GetAll(string filter);

		Task<List<Function>> GetAllFunctionByRole(string[] roles);

		IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId);

		FunctionViewModel GetById(string id);

		void Update(FunctionViewModel function);

		void Delete(string id);

		void Save();

		bool CheckExistedId(string id);

		void UpdateParentId(string sourceId,string targetId,Dictionary<string,int> items);

		void ReOrder(string sourceId,string targetId);
	}
}
