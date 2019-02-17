using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.ViewModels.ServiceItem;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.Services.Interfaces
{
    public interface IItemService
    {
        Task<IServiceOperationResult> CreateAsync(
            string part,
            decimal pricePerUnit,
            double units,
            string interventionId);

        Task<ServiceItem> GetAsync(string id);

        Task<IEnumerable<ServiceItemListViewModel>> GetByInterventionIdAsync(string id);

        Task<IServiceOperationResult> UpdateAsync(ServiceItem item);

        Task<IServiceOperationResult> RemoveAsync(string id);
    }
}
