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
    public class VehicleRepository : RepositoryBase<Vehicle, string>, IVehicleRepository
    {
        public VehicleRepository(DbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<IEnumerable<Vehicle>> GetAsync(
            string orderMember,
            OrderDirection orderDirection,
            int pageIndex = 1,
            int itemsPerPage = int.MaxValue)
        {
            var all = base.All()
                .Include(v => v.Owner)
                .Include(v => v.ServiceInterventions);
            var paginatedVehicles = await base.PaginateEntitiesAsync(
                all, orderMember, orderDirection, pageIndex, itemsPerPage);

            return paginatedVehicles;
        }

        public async Task<IEnumerable<Vehicle>> FilterAsync(string searchTerm)
        {
            return await base.All()
                    .Include(v => v.Owner)
                    .Include(v => v.ServiceInterventions)
                    .Where(v => v.RegistrationPlate.ToLower().Contains(searchTerm)
                         || v.Make.ToLower().Contains(searchTerm)
                         || v.Model.ToLower().Contains(searchTerm)
                         || v.ExactModelName.ToLower().Contains(searchTerm)
                         || v.Owner.FirstName.ToLower().Contains(searchTerm)
                         || v.Owner.LastName.ToLower().Contains(searchTerm))
                    .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetByUserIdAsync(string id)
        {
            return await base.All()
                .Include(v => v.Owner)
                .Include(v => v.ServiceInterventions)
                .Where(v => v.OwnerId == id)
                .ToListAsync();
        }
    }
}
