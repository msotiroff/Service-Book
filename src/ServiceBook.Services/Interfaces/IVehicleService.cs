using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using ServiceBook.Models.ViewModels.Vehicle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IServiceOperationResult> CreateAsync(
            string make, 
            string model, 
            string exactModelName, 
            string vin, 
            string regPlate, 
            string ownerId);

        Task<Vehicle> GetAsync(string id);

        Task<IServiceOperationResult> UpdateAsync(Vehicle vehicle);

        Task<IEnumerable<VehicleListViewModel>> AllAsync(
            string orderMember,
            OrderDirection orderDirection = OrderDirection.Ascending,
            int pageIndex = 1,
            int itemsPerPage = int.MaxValue);

        Task<IEnumerable<VehicleListViewModel>> FilterAsync(string searchTerm);

        Task<IEnumerable<VehicleListViewModel>> GetByUserIdAsync(string id);
    }
}
