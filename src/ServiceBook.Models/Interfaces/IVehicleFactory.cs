using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.Models.Interfaces
{
    public interface IVehicleFactory
    {
        Vehicle CreateInstance(
            string make,
            string model,
            string exactModelName,
            string vin,
            string registrationPlate,
            string ownerId);
    }
}
