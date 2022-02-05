using DevExtreme.AspNet.Data;
using ItServiceApp.business.Abstract;
using ItServiceApp.entity;
using ItServiceApp.Extensions;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]

    [Authorize(Roles = "Admin")]
    public class BlogApiController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogApiController(IBlogService blogService)
        {

            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlog(DataSourceLoadOptions options)
        {
            var blogs = await _blogService.GetAll();

            return Ok(DataSourceLoader.Load(blogs, options));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(DataSourceLoadOptions options, int id)
        {
            var b = await _blogService.GetById(id);

            if (b == null)
            {
                return NotFound(); // 404
            }
            var data = new List<Blog>();
            data.Add(b);
            return Ok(DataSourceLoader.Load(data, options)); // 200
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(Blog entity)
        {
            await _blogService.CreateAsync(entity);
            return RedirectToAction("Blog", "Manage");
        }
        [HttpPost]
        public async Task<IActionResult> InsertBlog(string key, string values)
        {
            var blog = new Blog();
            JsonConvert.PopulateObject(values, blog);
            await _blogService.CreateAsync(blog);


            return Ok(new JsonResponserViewModel());
        }


        // localhost:5000/api/products/2
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(DataSourceLoadOptions options, int id, Blog entity)
        {
            if (id != entity.BlogID)
            {
                return BadRequest();
            }

            var blog = await _blogService.GetById(id);

            if (blog == null)
            {
                return NotFound();
            }

            await _blogService.UpdateAsync(blog, entity);
            return RedirectToAction("Blog", "Manage");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update2Product(string key, string values)
        {

            var blog = await _blogService.GetById(Convert.ToInt32(key));

            var prodDegisken = blog;
            if (blog == null)
            {
                return NotFound();
            }
            JsonConvert.PopulateObject(values, blog);

            await _blogService.UpdateAsync(prodDegisken, blog);


            return Ok(new JsonResponserViewModel());
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(DataSourceLoadOptions options, int id)
        {
            var blog = await _blogService.GetById(id);

            if (blog == null)
            {
                return NotFound();
            }

            await _blogService.DeleteAsync(blog);
            return Ok(new JsonResponserViewModel());
        }
    }
}
