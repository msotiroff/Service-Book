using Microsoft.EntityFrameworkCore;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBook.DAL
{
    public class UserRepository : RepositoryBase<User, string>, IUserRepository
    {
        public UserRepository(DbContext dbContext) 
            : base(dbContext)
        {
        }

        public override async Task<IEnumerable<User>> GetAsync(
            string orderMember, 
            OrderDirection orderDirection, 
            int pageIndex = 1, 
            int itemsPerPage = int.MaxValue)
        {
            var allUsers = base.All().Include(u => u.Vehicles);
            var paginatedUsers = await base.PaginateEntitiesAsync(
                allUsers, orderMember, orderDirection, pageIndex, itemsPerPage);

            return paginatedUsers;
        }

        public async Task<IEnumerable<User>> FilterAsync(string searchTerm)
        {
            return await base.All()
                .Include(u => u.Vehicles)
                .Where(u => u.Email.ToLower().Contains(searchTerm)
                    || u.FirstName.ToLower().Contains(searchTerm)
                    || u.LastName.ToLower().Contains(searchTerm))
                .ToListAsync();
        }
    }
}
