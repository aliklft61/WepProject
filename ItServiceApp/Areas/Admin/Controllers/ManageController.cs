using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ItServiceApp.Models;
using Microsoft.AspNetCore.Authorization;
using ItServiceApp.entity;
using ItServiceApp.business.Abstract;
using ItServiceApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ItServiceApp.Areas.Admin.Controllers
{
    public class ManageController : AdminBaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ManageController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            ViewBag.Categories = await _categoryService.GetAll();
            
            var product = new ProductModel();
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  ProductCreate(ProductModel model,IFormFile file)
        {
            ViewBag.Categories = await _categoryService.GetAll();

            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Url = model.Url,
                    Price = model.Price,
                    Description = model.Description,
                    
                };
                if (file != null)
                {
                    entity.ImageUrl = file.FileName;
                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/user-assets/images/{randomName}");

                    using var stream = new FileStream(path, FileMode.Create) ;
                    
                    await file.CopyToAsync(stream);
                   
                }

                var kategori = await _categoryService.GetById(model.CategoryId);
                
                if (_productService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "kayıt eklendi",
                        Message = "kayıt eklendi",
                        AlertType = "success"
                    });

                    var product = await _productService.GetByName(entity.Name);

                    var productCategori = new ProductCategory();
                    productCategori.ProductId = product.ProductId;
                    productCategori.CategoryId = kategori.CategoryId;
                    product.ProductCategories.Add(productCategori);
                    await _productService.UpdateAsync(entity, product);
                    
                    return RedirectToAction("Product");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });

                return View(model);
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = await _categoryService.GetAll();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = await _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Price = model.Price;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.IsHome = model.IsHome;
                entity.IsApproved = model.IsApproved;

                if (file != null)
                {
                    var extention = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/user-assets/images/{randomName}");

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "kayıt güncellendi",
                        Message = "kayıt güncellendi",
                        AlertType = "success"
                    });
                    return RedirectToAction("Product");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "hata",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
            }
            ViewBag.Categories = await _categoryService.GetAll();
            return View(model);
        }

        
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var entity = await _productService.GetById(id);

            if (entity != null)
            {
                _productService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("Product");
        }

        public IActionResult Category()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };

                _categoryService.Create(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "kayıt eklendi.",
                    Message = $"{entity.Name} isimli category eklendi.",
                    AlertType = "success"
                });

                return RedirectToAction("Category");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = await _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Url = model.Url;

                _categoryService.Update(entity);

                var msg = new AlertMessage()
                {
                    Message = $"{entity.Name} isimli category güncellendi.",
                    AlertType = "success"
                };

                TempData["message"] = JsonConvert.SerializeObject(msg);

                return RedirectToAction("Category");
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var entity = await _categoryService.GetById(id);

            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli category silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("Category");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return RedirectToAction("CategoryEdit" , new { id = categoryId });
        }
    }
}
