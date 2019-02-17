using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceBook.Config;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Extensions;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;

namespace ServiceBook.DAL
{
    public abstract class RepositoryBase<TEntity, Tkey> : IRepository<TEntity, Tkey>
        where TEntity : class, IEntity<Tkey>
    {
        protected readonly DbContext dbContext;

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public abstract Task<IEnumerable<TEntity>> GetAsync(
            string orderMember,
            OrderDirection orderDirection,
            int pageIndex = 1,
            int itemsPerPage = int.MaxValue);

        public async Task CreateAsync(TEntity entity)
        {
            await this.dbContext.AddAsync(entity);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this.dbContext.Remove(entity);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsync(Tkey key)
        {
            return await this.dbContext.FindAsync<TEntity>(key);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this.dbContext.Update(entity);

            await this.dbContext.SaveChangesAsync();
        }

        protected IQueryable<TEntity> All()
        {
            return this.dbContext.Set<TEntity>().AsNoTracking();
        }

        protected async Task<IEnumerable<TEntity>> PaginateEntitiesAsync(
            IQueryable<TEntity> entities,
            string orderMember,
            OrderDirection orderDirection,
            int pageIndex,
            int itemsPerPage)
        {
            switch (orderDirection)
            {
                case OrderDirection.Ascending:
                default:
                    entities = entities.OrderByMember(orderMember);

                    break;
                case OrderDirection.Descending:
                    entities = entities.OrderByMemberDescending(orderMember);

                    break;
            }

            return await entities
                .Skip((pageIndex - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
        }
    }
}
