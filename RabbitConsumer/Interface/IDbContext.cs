using Microsoft.EntityFrameworkCore;

namespace RabbitConsumer.Interface
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
