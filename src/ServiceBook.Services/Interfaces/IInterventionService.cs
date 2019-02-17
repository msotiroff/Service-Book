using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.ViewModels.ServiceIntervention;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.Services.Interfaces
{
    public interface IInterventionService
    {
        Task<IEnumerable<ServiceInterventionListViewModel>> All();

        Task<IServiceOperationResult> CreateAsync(
            DateTime currentDate, 
            DateTime nextInterventionDate,
            int mileage, 
            string description, 
            string vehicleId);

        Task<IEnumerable<ServiceInterventionListViewModel>> GetByVehicleIdAsync(string vehicleId);

        Task<ServiceIntervention> GetAsync(string id);

        Task<IServiceOperationResult> UpdateAsync(ServiceIntervention intervention);

        Task<IServiceOperationResult> RemoveAsync(string id);
    }
}
