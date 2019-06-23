using AutoMapper;
using OnlineShop.Data.Entities;
using OnlineShop.Services.ViewModels.product;
using OnlineShop.Services.ViewModels.system;
using System;

namespace OnlineShop.Services.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                c.SortOrder, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                 c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                 c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<AppUserViewModel, AppUser>()
            .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
            c.Email, c.PhoneNumber, c.Avatar, c.Status));

            CreateMap<PermissionViewModel, Permission>()
            .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
        }
    }

}