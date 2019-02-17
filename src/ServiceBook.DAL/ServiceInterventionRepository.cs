using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;

namespace ServiceBook.DAL
{
    public class ServiceInterventionRepository : RepositoryBase<ServiceIntervention, string>, IServiceInterventionRepository
    {
        public ServiceInterventionRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }

        public override Task<IEnumerable<ServiceIntervention>> GetAsync(
            string orderMember, 
            OrderDirection orderDirection, 
            int pageIndex = 1, 
            int itemsPerPage = int.MaxValue)
        {
            var allInterventions = base.All()
                .Include(i => i.Vehicle)
                    .ThenInclude(v => v.Owner);
            var paginatedInterventions = base.PaginateEntitiesAsync(
                allInterventions, orderMember, orderDirection, pageIndex, itemsPerPage);

            return paginatedInterventions;
        }

        public async Task<IEnumerable<ServiceIntervention>> GetByVehicleIdAsync(string vehicleId)
        {
            return await this.All()
                .Include(i => i.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(i => i.ServiceItems)
                .Where(i => i.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
}
