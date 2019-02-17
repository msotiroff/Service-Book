using Microsoft.EntityFrameworkCore;
using ServiceBook.Config;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBook.DAL
{
    public class ServiceItemRepository : RepositoryBase<ServiceItem, string>, IServiceItemRepository
    {
        public ServiceItemRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }

        public override async Task<IEnumerable<ServiceItem>> GetAsync(
            string orderMember, 
            OrderDirection orderDirection, 
            int pageIndex = 1, 
            int itemsPerPage = int.MaxValue)
        {
            var allItems = base.All()
                .Include(item => item.ServiceIntervention)
                    .ThenInclude(i => i.Vehicle)
                        .ThenInclude(v => v.Owner);
            var paginatedItems = await base.PaginateEntitiesAsync(
                allItems, orderMember, orderDirection, pageIndex, itemsPerPage);

            return paginatedItems;
        }

        public async Task<IEnumerable<ServiceItem>> GetByInterventionIdAsync(string id)
        {
            return await this.All()
                .Include(item => item.ServiceIntervention)
                .Where(item => item.ServiceInterventionId == id)
                .ToListAsync();
        }
    }
}
