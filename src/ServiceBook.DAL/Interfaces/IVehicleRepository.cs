using ServiceBook.Models.DatabaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.DAL.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle, string>
    {
        Task<IEnumerable<Vehicle>> FilterAsync(string searchTerm);

        Task<IEnumerable<Vehicle>> GetByUserIdAsync(string id);
    }
}
