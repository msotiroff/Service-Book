using ServiceBook.Models.DatabaseModels;
using System;

namespace ServiceBook.Models.Interfaces
{
    public interface IServiceInterventionFactory
    {
        ServiceIntervention CreateInstance(
            DateTime currentDate, 
            DateTime nextInterventionDate, 
            int mileage, 
            string description,
            string vehicleId);
    }
}
