using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.Mappers;
using WebAppCore.Application.ViewModels.System;
using WebAppCore.Data.Entities;
using WebAppCore.Data.Enums;
using WebAppCore.Infrastructure.Interfaces;

namespace WebAppCore.Application.Implementation
{
	public class FunctionService:IFunctionService
	{
		private IRepository<Function,string> _functionRepository;
		private IRepository<Permission,int> _permissionRepository;
		private RoleManager<AppRole> _roleManager;
		private IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public FunctionService(IMapper mapper,
			IRepository<Function,string> functionRepository,
			IRepository<Permission,int> permissionRepository,
			RoleManager<AppRole> roleManager,
			IUnitOfWork unitOfWork)
		{
			_functionRepository = functionRepository;
			_permissionRepository = permissionRepository;
			_roleManager = roleManager;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public bool CheckExistedId(string id)
		{
			return _functionRepository.FindById(id) != null;
		}

		public void Add(FunctionViewModel functionVm)
		{
			var function = functionVm.AddModel();
			_functionRepository.Add(function);
		}

		public void Delete(string id)
		{
			_functionRepository.Remove(id);
		}

		public FunctionViewModel GetById(string id)
		{
			var function = _functionRepository.FindSingle(x => x.Id == id);
			return function.ToModel();
		}

		public Task<List<FunctionViewModel>> GetAll(string filter)
		{
			var query = _functionRepository.FindAll(x => x.Status == Status.Active);

			if(!string.IsNullOrEmpty(filter))
				query = query.Where(x => x.Name.Contains(filter));
			return query.OrderBy(x => x.ParentId).Select(x => x.ToModel()).ToListAsync();
		}

		public Task<List<Function>> GetAllFunctionByRole(string[] roles)
		{
			var functions = _functionRepository.FindAll();
			var permissions = _permissionRepository.FindAll();
			var query = from f in functions
						join p in permissions on f.Id equals p.FunctionId
						join r in _roleManager.Roles on p.RoleId equals r.Id
						where roles.Contains(r.Name)
						&& ((p.CanCreate == true)
						|| (p.CanUpdate == true)
						|| (p.CanDelete == true)
						|| (p.CanRead == true))
						select f;
			return query.ToListAsync();
		}

		public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
		{
			return _functionRepository.FindAll(x => x.ParentId == parentId).Select(x => x.ToModel());
		}

		public void Save()
		{
			_unitOfWork.Commit();
		}

		public void Update(FunctionViewModel functionVm)
		{
			var functionDb = _functionRepository.FindById(functionVm.Id);
			var function = functionVm.AddModel();
		}

		public void ReOrder(string sourceId,string targetId)
		{
			var source = _functionRepository.FindById(sourceId);
			var target = _functionRepository.FindById(targetId);
			int tempOrder = source.SortOrder;

			source.SortOrder = target.SortOrder;
			target.SortOrder = tempOrder;

			_functionRepository.Update(source);
			_functionRepository.Update(target);
		}

		public void UpdateParentId(string sourceId,string targetId,Dictionary<string,int> items)
		{
			//Update parent id for source
			var category = _functionRepository.FindById(sourceId);
			category.ParentId = targetId;
			_functionRepository.Update(category);

			//Get all sibling
			var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
			foreach(var child in sibling)
			{
				child.SortOrder = items[child.Id];
				_functionRepository.Update(child);
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}