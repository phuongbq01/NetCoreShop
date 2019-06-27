using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using OnlineShop.Infrastructure.Interfaces;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.Common;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Services.Implementations
{
    public class ContactService : IContactService
    {
        private IContactRepository _contactRepository;
        private IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._contactRepository = contactRepository;
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(ContactViewModel pageVm)
        {
            var page = _mapper.Map<ContactViewModel, Contact>(pageVm);
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
            return _contactRepository.FindAll().ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider).ToList();
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
                Results = data.ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider).ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }


        public ContactViewModel GetById(string id)
        {
            return _mapper.Map<Contact, ContactViewModel>(_contactRepository.FindById(id));
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ContactViewModel pageVm)
        {
            var page = _mapper.Map<ContactViewModel, Contact>(pageVm);
            _contactRepository.Update(page);
        }
        
    }
}
