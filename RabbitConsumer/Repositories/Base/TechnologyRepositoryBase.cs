using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;

namespace RabbitConsumer.Repositories.Base
{
    public abstract class TechnologyRepositoryBase<T> : ITechnology<T>
        where T : EntityBase
    {
        DbContext _db;
        protected TechnologyRepositoryBase(DbContext db)
        {
            _db = db;
        }

        public async Task<T?> GetOne(int id)
        {
            return await NoTrackingCollection
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<T>> GetAll(int itemsOnPage, int page, bool isLoadDeleted)
        {

            return await NoTrackingCollection.ToListAsync();

        }

        public async Task Post(T entity)
        {
            await DbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Put(int id, T entity)
        {
            entity.Id = id;
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var item = DbSet.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                DbSet.Remove(item);
                await _db.SaveChangesAsync();
            }
        }


        private IQueryable<T> NoTrackingCollection => DbSet.AsNoTracking();
        protected abstract DbSet<T> DbSet { get; }

    }
}
