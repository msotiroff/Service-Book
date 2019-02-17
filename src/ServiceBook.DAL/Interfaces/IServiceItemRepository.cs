using ServiceBook.Models.DatabaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.DAL.Interfaces
{
    public interface IServiceItemRepository : IRepository<ServiceItem, string>
    {
        Task<IEnumerable<ServiceItem>> GetByInterventionIdAsync(string id);
    }
}
