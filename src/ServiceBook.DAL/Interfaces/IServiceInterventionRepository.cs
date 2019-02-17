using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.DAL.Interfaces
{
    public interface IServiceInterventionRepository : IRepository<ServiceIntervention, string>
    {
        Task<IEnumerable<ServiceIntervention>> GetByVehicleIdAsync(string vehicleId);
    }
}
