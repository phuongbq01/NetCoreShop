using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineShop.Models.ProductViewModels;
using OnlineShop.Services.Interfaces;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IConfiguration _configuration;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IConfiguration configuration)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
        }

        // All Product
        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        // Products By Category
        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.SortType = sortBy;
            catalog.PageSize = pageSize;
            catalog.Data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value);
            catalog.Category = _productCategoryService.GetById(id);

            ViewData["BodyClass"] = "shop_grid_full_width_page";
            return View(catalog);
        }

        // Products By Category
        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new SearchResultViewModel();
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.SortType = sortBy;
            catalog.PageSize = pageSize;
            catalog.Data = _productService.GetAllPaging(null, keyword, page, pageSize.Value);
            catalog.Keyword = keyword;

            ViewData["BodyClass"] = "shop_grid_full_width_page";
            return View(catalog);
        }


        // Product Detail
        [Route("{alias}-p.{id}.html", Name ="ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel();
            model.Product = _productService.GetById(id);
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            model.RelatedProducts = _productService.GetRelatedProducts(id, 9);
            model.UpsellProducts = _productService.GetUpsellProducts(6);
            model.ProductImages = _productService.GetImages(id);
            model.Tags = _productService.GetProductTags(id);

            return View(model);
        }
    }
}