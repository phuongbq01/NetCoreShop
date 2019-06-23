using AutoMapper;
using OnlineShop.Data.Entities;
using OnlineShop.Services.ViewModels.product;
using OnlineShop.Services.ViewModels.system;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Permission, PermissionViewModel>();
        }
    }
}
