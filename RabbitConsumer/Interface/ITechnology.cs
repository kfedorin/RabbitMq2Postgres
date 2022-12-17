using RabbitConsumer.Repositories.Base;

namespace RabbitConsumer.Interface
{
    public interface ITechnology<T> where T : EntityBase
    {
        Task<T?> GetOne(int id);
        Task<List<T>> GetAll(int itemsOnPage = 0, int page = 0, bool isLoadDeleted = false);
        Task Post(T entity);
        Task Put(int id, T entity);
        Task Delete(int id);
    }
}