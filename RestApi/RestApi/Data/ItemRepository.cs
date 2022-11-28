using Microsoft.EntityFrameworkCore;
using RestApi.Entities;
using RestApi.Helpers;
using RestApi.Interfaces;

namespace RestApi.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepository(DataContext dataContext)
        {
            _dataContext=dataContext;
        }

        public void AddItem(Item item)
        {
            _dataContext.Items.Add(item);
        }
        public void UpdateItem(Item item)
        {
            _dataContext.Items.Update(item);
        }
        public void DeleteItem(Item item)
        {
            _dataContext.Items.Remove(item);
        }
        public async Task<Item> GetItem(int id)
        {
            return await _dataContext.Items
               .SingleOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PagedList<Item>> GetItems(ItemParams paginationParams)
        {
            var query = _dataContext.Items.AsQueryable().Include(i => i.Category);
            query.Where(i => i.CategoryId == paginationParams.CategoryId);
            return await PagedList<Item>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}
