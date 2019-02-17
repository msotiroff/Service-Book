using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Interfaces;
using System;

namespace ServiceBook.Models.Factories
{
    public class ServiceInterventionFactory : IServiceInterventionFactory
    {
        private readonly IModelValidator modelValidator;

        public ServiceInterventionFactory(IModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
        }

        public ServiceIntervention CreateInstance(
            DateTime currentDate, 
            DateTime nextInterventionDate, 
            int mileage, 
            string description, 
            string vehicleId)
        {
            var serviceIntervention = new ServiceIntervention
            {
                DateTime = currentDate,
                NextServiceIntervalDate = nextInterventionDate,
                Description = description,
                Mileage = mileage,
                VehicleId = vehicleId
            };

            this.modelValidator.Validate(serviceIntervention);

            return serviceIntervention;
        }
    }
}
