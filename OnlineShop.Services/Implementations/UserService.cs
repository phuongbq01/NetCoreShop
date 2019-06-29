using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Entities;
using OnlineShop.Data.Enums;
using OnlineShop.Data.IRepositories;
using OnlineShop.Infrastructure.Interfaces;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.system;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementUserRepository _announcementUserRepository;
        private readonly IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public UserService(UserManager<AppUser> userManager, IMapper mapper, IAnnouncementRepository announcementRepository, IAnnouncementUserRepository announcementUserRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _announcementRepository = announcementRepository;
            _announcementUserRepository = announcementUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(AnnouncementViewModel announcementVm, List<AnnouncementUserViewModel> announcementUsers, AppUserViewModel userVm)
        {
            var user = new AppUser()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber,
                Status = userVm.Status
            };
            //var result = await _userManager.CreateAsync(user, userVm.Password);
            //if (result.Succeeded && userVm.Roles.Count > 0)
            //{
            //    var appUser = await _userManager.FindByNameAsync(user.UserName);
            //    if (appUser != null)
            //        await _userManager.AddToRolesAsync(appUser, userVm.Roles);

            //}
            //return true;
            var result = await _userManager.CreateAsync(user);
            var announcement = _mapper.Map<AnnouncementViewModel, Announcement>(announcementVm);
            _announcementRepository.Add(announcement);
            foreach (var userItem in announcementUsers)
            {
                var u = _mapper.Map<AnnouncementUserViewModel, AnnouncementUser>(userItem);
                _announcementUserRepository.Add(u);
            }
            _unitOfWork.Commit();
            return result.Succeeded;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<AppUserViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.FullName.Contains(keyword)
                || x.UserName.Contains(keyword)
                || x.Email.Contains(keyword));

            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = query.Select(x => new AppUserViewModel()
            {
                UserName = x.UserName,
                Avatar = x.Avatar,
                BirthDay = x.BirthDay.ToString(),
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateCreated = x.DateCreated

            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppUserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<AppUser, AppUserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }

        public async Task UpdateAsync(AppUserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());

            // danh sách Roles hiện tại
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Thêm Role mới trừ những Role đã có
            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());

            if (result.Succeeded)
            {
                // Xóa Role mà không có trong roles mới
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Avatar = userVm.Avatar;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                await _userManager.UpdateAsync(user);
            }

        }
    }
}
