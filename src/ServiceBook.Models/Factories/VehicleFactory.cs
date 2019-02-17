using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Interfaces;

namespace ServiceBook.Models.Factories
{
    public class VehicleFactory : IVehicleFactory
    {
        private readonly IModelValidator modelValidator;

        public VehicleFactory(IModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
        }

        public Vehicle CreateInstance(
            string make, 
            string model, 
            string exactModelName, 
            string vin, 
            string registrationPlate, 
            string ownerId)
        {
            var vehicle = new Vehicle
            {
                Make = make,
                Model = model,
                ExactModelName = exactModelName,
                VIN = vin,
                RegistrationPlate = registrationPlate,
                OwnerId = ownerId
            };

            this.modelValidator.Validate(vehicle);

            return vehicle;
        }
    }
}
