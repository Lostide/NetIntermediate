using Microsoft.EntityFrameworkCore;
using RestApi.Entities;
using RestApi.Helpers;
using RestApi.Interfaces;

namespace RestApi.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public void AddCategory(Category category)
        {
            _dataContext.Categories.Add(category);
        }
        public void UpdateCategory(Category category)
        {
            _dataContext.Categories.Update(category);
        }
        public void DeleteCategory(Category category)
        {
            _dataContext.Categories.Remove(category);
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _dataContext.Categories
               .SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedList<Category>> GetCategories(PaginationParams paginationParams)
        {
            var query = _dataContext.Categories.AsQueryable();
            return await PagedList<Category>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
