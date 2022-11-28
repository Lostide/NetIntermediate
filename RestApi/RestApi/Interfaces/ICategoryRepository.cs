using RestApi.Entities;
using RestApi.Helpers;

namespace RestApi.Interfaces
{
    public interface ICategoryRepository
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        Task<Category> GetCategory(int id);
        Task<PagedList<Category>> GetCategories(PaginationParams paginationParams);
    }
}
