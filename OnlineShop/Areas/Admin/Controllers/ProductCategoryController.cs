using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineShop.Authorization;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.product;
using OnlineShop.Utilities.Helpers;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IAuthorizationService _authorizationService;

        public ProductCategoryController(IProductCategoryService productCategoryService, IAuthorizationService authorizationService)
        {
            _productCategoryService = productCategoryService;
            _authorizationService = authorizationService;
        }
        public async Task<IActionResult> Index()
        {
            // Kiểm tra quyền View của User cho function ProductCategory
            var result = await _authorizationService.AuthorizeAsync(User, "PRODUCT_CATEGORY", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }


        #region AJAX
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productCategoryService.GetById(id);

            return new ObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId, items);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }


        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.ReOrder(sourceId, targetId);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productCategoryService.Add(productVm);
                }
                else
                {
                    _productCategoryService.Update(productVm);
                }
                _productCategoryService.Save();
                return new OkObjectResult(productVm);

            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _productCategoryService.Delete(id);
                _productCategoryService.Save();
                return new OkObjectResult(id);
            }
        }

        #endregion
    }
}