using AutoMapper;
using AutoMapper.QueryableExtensions;
using OnlineShop.Data.Enums;
using OnlineShop.Data.IRepositories;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.product;
using OnlineShop.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Services.Implementations
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(x => x.ProductCategory).ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            // lọc tìm kiếm
            var query = _productRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));
            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            // tổng số bản ghi
            int totalRow = query.Count();

            // phân trang
            query = query.OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);


            //mapping sang ViewModel
            var data = query.ProjectTo<ProductViewModel>(_mapper.ConfigurationProvider).ToList();

            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }
    }
}
