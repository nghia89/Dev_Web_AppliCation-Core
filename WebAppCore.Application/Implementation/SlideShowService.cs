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

namespace WebAppCore.Application.Implementation
{
    public class SlideShowService : ISlideShowService
    {
        private readonly IRepository<Slide, int> _SlideRepository;
        private IUnitOfWork _unitOfWork;

        public SlideShowService(IRepository<Slide, int> SlideRepository, IUnitOfWork unitOfWork)
        {
            _SlideRepository = SlideRepository;
            _unitOfWork = unitOfWork;
        }
        public void Create(SlideShowViewModel slideVM)
        {
            var slide = Mapper.Map<SlideShowViewModel, Slide>(slideVM);
            _SlideRepository.Add(slide);
        }

        public void DeleteDetail(int id)
        {
            _SlideRepository.Remove(id);
        }

        public PagedResult<SlideShowViewModel> GetAllPaging(int pageIndex, int pageSize)
        {
            var query = _SlideRepository.FindAll();
            int totalRow = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var data = query.ProjectTo<SlideShowViewModel>().ToList();

            var paginationSet = new PagedResult<SlideShowViewModel>()
            {
                Results = data,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;

        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(SlideShowViewModel slideVM)
        {
            var slide = Mapper.Map<SlideShowViewModel, Slide>(slideVM);
            _SlideRepository.Update(slide);
        }
    }
}
