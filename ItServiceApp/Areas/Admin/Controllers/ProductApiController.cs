using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using ItServiceApp.business.Abstract;
using ItServiceApp.dal.Abstract;
using ItServiceApp.dal.Concrete.EfCore;
using ItServiceApp.entity;
using ItServiceApp.Extensions;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    
    [Authorize(Roles = "Admin")]
    public class ProductApiController : Controller
    {
        
        private IProductService _productService;
        public ProductApiController(IProductService productService)
        {
            
            _productService = productService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetProducts(DataSourceLoadOptions options)
        {
            var products = await _productService.GetAll();

            return Ok(DataSourceLoader.Load(products, options));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(DataSourceLoadOptions options,int id)
        {
            var p = await _productService.GetById(id);
            
            if (p == null)
            {
                return NotFound(); // 404
            }
            var data = new List<Product>();
            data.Add(p);
            return Ok(DataSourceLoader.Load(data,options)); // 200
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            await _productService.CreateAsync(entity);
            //return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, ProductToDTO(entity));
            return RedirectToAction("Product", "Manage");
        }
        [HttpPost]
        public async Task<IActionResult> InsertProduct(string key,string values)
        {
            var product = new Product();
            JsonConvert.PopulateObject(values,product);
            /*product.ProductId = Convert.ToInt32(key)*/;
            await _productService.CreateAsync(product);
           
           
            return Ok(new JsonResponserViewModel());
        }


        // localhost:5000/api/products/2
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(DataSourceLoadOptions options,int id, Product entity)
        {
            if (id != entity.ProductId)
            {
                return BadRequest();
            }

            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.UpdateAsync(product, entity);
            return RedirectToAction("Product","Manage");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update2Product(string key,string values)
        {
            
            var product = await _productService.GetById(Convert.ToInt32(key));

            var prodDegisken = product;
            if (product == null)
            {
                return NotFound();
            }
            JsonConvert.PopulateObject(values,product);

            await _productService.UpdateAsync(prodDegisken, product);


            return Ok(new JsonResponserViewModel());
        }
       

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(DataSourceLoadOptions options,int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(product);
            return Ok(new JsonResponserViewModel());
        }

       
    }
}
