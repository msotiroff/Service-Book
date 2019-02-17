using ServiceBook.Config;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.DAL.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        Task CreateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAsync(
            string orderMember,
            OrderDirection orderDirection, 
            int pageIndex = 1, 
            int itemsPerPage = UIConstants.ItemsPerPage);

        Task<TEntity> GetAsync(TKey key);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
