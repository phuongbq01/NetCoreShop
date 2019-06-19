using AutoMapper;
using OnlineShop.Data.Entities;
using OnlineShop.Services.ViewModels.product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.Services.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile() {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}
