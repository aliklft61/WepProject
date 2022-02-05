using ItServiceApp.business.Abstract;
using ItServiceApp.dal.Abstract;
using ItServiceApp.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItServiceApp.business.Concrete
{
    public class BlogManager : IBlogService
    {
        private readonly IUnitOfWork _unitofwork;
        public BlogManager(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public bool Create(Blog entity)
        {
            if (Validation(entity))
            {
                _unitofwork.Blog.Create(entity);
                _unitofwork.Save();
                return true;
            }
            return false;
        }

        public async Task<Blog> CreateAsync(Blog entity)
        {
            await _unitofwork.Blogs.CreateAsync(entity);
            await _unitofwork.SaveAsync();
            return entity;
        }

        public void Delete(Blog entity)
        {
            // iş kuralları
            _unitofwork.Blogs.Delete(entity);
            _unitofwork.Save();
        }

        public async Task<List<Blog>> GetAll()
        {
            return await _unitofwork.Blogs.GetAll();
        }

        public async Task<Blog> GetById(int id)
        {
            return await _unitofwork.Blogs.GetById(id);
        }

        public Blog GetByIdWithCategories(int id)
        {
            return _unitofwork.Products.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _unitofwork.Products.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _unitofwork.Products.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _unitofwork.Products.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _unitofwork.Products.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _unitofwork.Products.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
            _unitofwork.Products.Update(entity);
            _unitofwork.Save();
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz.";
                    return false;
                }
                _unitofwork.Products.Update(entity, categoryIds);
                _unitofwork.Save();
                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz.\n";
                isValid = false;
            }

            if (entity.Price < 0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz.\n";
                isValid = false;
            }

            return isValid;
        }

        public async Task UpdateAsync(Product entityToUpdate, Product entity)
        {
            entityToUpdate.Name = entity.Name;
            entityToUpdate.Price = entity.Price;
            entityToUpdate.Description = entity.Description;

            await _unitofwork.SaveAsync();
        }

        public async Task DeleteAsync(Product entity)
        {
            _unitofwork.Products.Delete(entity);
            await _unitofwork.SaveAsync();
        }

        public async Task<Product> GetByName(string name)
        {
            return await _unitofwork.Products.GetByName(name);

        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Blog entity)
        {
            throw new NotImplementedException();
        }

        public Blog GetBlogDetails(string url)
        {
            throw new NotImplementedException();
        }

        Task<Blog> IBlogService.GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Blog> GetHomePageBlog()
        {
            throw new NotImplementedException();
        }

        List<Blog> IBlogService.GetSearchResult(string searchString)
        {
            throw new NotImplementedException();
        }

        public void Update(Blog entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Blog entityToUpdate, Blog entity)
        {
            throw new NotImplementedException();
        }
    }
}

