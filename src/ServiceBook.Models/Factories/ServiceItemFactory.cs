using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Interfaces;

namespace ServiceBook.Models.Factories
{
    public class ServiceItemFactory : IServiceItemFactory
    {
        public ServiceItem CreateInstance(
            string part, decimal pricePerUnit, double units, string interventionId)
        {
            return new ServiceItem
            {
                Part = part,
                PricePerUnit = pricePerUnit,
                Units = units,
                ServiceInterventionId = interventionId
            };
        }
    }
}
