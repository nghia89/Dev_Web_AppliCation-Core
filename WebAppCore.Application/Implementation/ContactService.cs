using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAppCore.Application.Interfaces;
using WebAppCore.Application.ViewModels.Common;
using WebAppCore.Data.Entities;
using WebAppCore.Infrastructure.Interfaces;
using WebAppCore.Utilities.Dtos;
using WebAppCore.Application.Mappers;

namespace WebAppCore.Application.Implementation
{
    public class ContactService : IContactService
    {
        private IRepository<Contact, string> _contactRepository;
        IUnitOfWork _unitOfWork;

        public ContactService(IRepository<Contact, string> contactRepository, IUnitOfWork unitOfWork)
        {
            this._contactRepository = contactRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(ContactViewModel pageVm)
        {
            var page = pageVm.AddModel();
            _contactRepository.Add(page);
        }

        public void Delete(string id)
        {
            _contactRepository.Remove(id);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ContactViewModel> GetAll()
        {
            return _contactRepository.FindAll().Select(x => x.ToModel()).ToList();
        }

        public PagedResult<ContactViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _contactRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var paginationSet = new PagedResult<ContactViewModel>()
            {
                Results = data.Select(x => x.ToModel()).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public ContactViewModel GetById(string id)
        {
            return _contactRepository.FindById(id).ToModel();
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ContactViewModel pageVm)
        {
            var page = pageVm.AddModel();
            _contactRepository.Update(page);
        }
    }
}
