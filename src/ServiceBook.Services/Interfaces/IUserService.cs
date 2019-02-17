using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> AllAsync(
            string orderMember,
            OrderDirection orderDirection = OrderDirection.Ascending,
            int pageIndex = 1,
            int itemsPerPage = int.MaxValue);

        Task<IServiceOperationResult> CreateAsync(
            string email,
            string firstName,
            string lastName,
            string phoneNumber);

        Task<User> GetAsync(string id);

        Task<IServiceOperationResult> UpdateAsync(User user);

        Task<IEnumerable<User>> FilterAsync(string searchTerm);
    }
}
