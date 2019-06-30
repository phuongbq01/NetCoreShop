using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using OnlineShop.Authorization;
using OnlineShop.Services.Interfaces;
using OnlineShop.Services.ViewModels.product;
using OnlineShop.Utilities.Helpers;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IAuthorizationService authorizationService, IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _authorizationService = authorizationService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "PRODUCT_LIST", Operations.Read);
            if (result.Succeeded == false)  // Nếu không có quyền trả về trang Login
                return new RedirectResult("/Admin/Login/Index");

            return View();
        }

        #region AJAX
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll() ;
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productVm)
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
                    _productService.Add(productVm);
                }
                else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }

        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }

        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }


        [HttpPost]
        public IActionResult SaveImages(int productId, string[] images)
        {
            _productService.AddImages(productId, images);
            _productService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = _productService.GetImages(productId);
            return new OkObjectResult(images);
        }

        [HttpPost]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }

        [HttpGet]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrices = _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                // tên file
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                // đường dẫn chứa file
                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))  // kiểm tra sự tồn tại của đường dẫn
                {
                    Directory.CreateDirectory(folder);  // Nếu chưa có sẽ tạo mới
                }
                string filePath = Path.Combine(folder, filename);   // địa chỉ tuyệt đối của file sau khi upload

                using (FileStream fs = System.IO.File.Create(filePath)) // upload
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, categoryId);  // Import
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult ExportExcel()
        {
            // đường dẫn
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";

            // file
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists) // kiểm tra file
            {
                file.Delete();  // Nếu file đã tồn tại thì xóa và tạo lại
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var products = _productService.GetAll();
            using (ExcelPackage package = new ExcelPackage(file))   // Đọc file
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        #endregion
    }
}