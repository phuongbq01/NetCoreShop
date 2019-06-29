using OnlineShop.Data.Entities;
using OnlineShop.Data.IRepositories;
using OnlineShop.Infrastructure.Interfaces;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.system;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.QueryableExtensions;
using System.Linq;
using AutoMapper;

namespace OnlineShop.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private IAnnouncementRepository _announcementRepository;
        private IAnnouncementUserRepository _announcementUserRepository;

        private IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public AnnouncementService(IAnnouncementRepository announcementRepository,
            IAnnouncementUserRepository announcementUserRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _announcementUserRepository = announcementUserRepository;
            _announcementRepository = announcementRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public PagedResult<AnnouncementViewModel> GetAllUnReadPaging(Guid userId, int pageIndex, int pageSize)
        {
            var query = from x in _announcementRepository.FindAll()
                        join y in _announcementUserRepository.FindAll()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from annonUser in xy.DefaultIfEmpty()
                        where annonUser.HasRead == false && (annonUser.UserId == null || annonUser.UserId == userId)
                        select x;
            int totalRow = query.Count();

            var model = query.OrderByDescending(x => x.DateCreated)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ProjectTo<AnnouncementViewModel>(_mapper.ConfigurationProvider).ToList();

            var paginationSet = new PagedResult<AnnouncementViewModel>
            {
                Results = model,
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public bool MarkAsRead(Guid userId, string id)
        {
            bool result = false;
            var announ = _announcementUserRepository.FindSingle(x => x.AnnouncementId == id
                                                                               && x.UserId == userId);
            if (announ == null)
            {
                _announcementUserRepository.Add(new AnnouncementUser
                {
                    AnnouncementId = id,
                    UserId = userId,
                    HasRead = true
                });
                result = true;
            }
            else
            {
                if (announ.HasRead == false)
                {
                    announ.HasRead = true;
                    result = true;
                }

            }
            return result;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
