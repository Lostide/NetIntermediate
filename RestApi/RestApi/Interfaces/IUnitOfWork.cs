
namespace RestApi.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get;}
        //IItemRepository ItemRepository { get;}
        Task<bool> Complete();
        bool HasChanges();
    }
}