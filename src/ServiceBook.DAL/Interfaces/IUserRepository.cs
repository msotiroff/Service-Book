using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User, string>
    {
        Task<IEnumerable<User>> FilterAsync(string searchTerm);
    }
}
