using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.Models.Interfaces
{
    public interface IServiceItemFactory
    {
        ServiceItem CreateInstance(
            string part, decimal pricePerUnit, double units, string interventionId);
    }
}
