using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using ItServiceApp.dal.Abstract;
using ItServiceApp.entity;
using ItServiceApp.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]

    [Authorize(Roles = "Admin")]
    public class CategoryApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public void Create(Category entity)
        {
            _unitOfWork.Categories.Create(entity);
            _unitOfWork.Save();
        }

        public async Task<Category> CreateAsync(Category entity)
        {
            await _unitOfWork.Categories.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entity;
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _unitOfWork.Categories.DeleteFromCategory(productId, categoryId);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _unitOfWork.Categories.GetAll();
        }
        public async Task<IActionResult> GetAllCategory(DataSourceLoadOptions options)
        {

            var dataList = await _unitOfWork.Categories.GetAll();
            
            return Ok(DataSourceLoader.Load(dataList, options));
        }

        public async Task<Category> GetById(int id)
        {
            return await _unitOfWork.Categories.GetById(id);
        }

        public  Category GetByIdWithProducts(int categoryId)
        {
            return _unitOfWork.Categories.GetByIdWithProducts(categoryId);
        }

        public async Task<IActionResult> getByIdWithProd(DataSourceLoadOptions options,int id)
        {
            await _unitOfWork.Categories.GetById(id);
           var data = _unitOfWork.Categories.GetByIdWithProducts(id);
           var dataList = new List<Category>();
           dataList.Add(data);
            return Ok(DataSourceLoader.Load(dataList,options));
        }

        public void Update(Category entity)
        {
            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Save();
        }

        public bool Validation(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
