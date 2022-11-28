using RestApi.Entities;
using RestApi.Helpers;

namespace RestApi.Interfaces
{
    public interface IItemRepository
    {
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Item item);
        Task<Item> GetItem(int id);
        Task<PagedList<Item>> GetItems(ItemParams paginationParams);
    }
}
