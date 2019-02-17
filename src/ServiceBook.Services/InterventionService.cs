using ServiceBook.DAL.Interfaces;
using ServiceBook.Extensions;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;
using ServiceBook.Models.ViewModels.ServiceIntervention;
using ServiceBook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBook.Services
{
    public class InterventionService : ServiceBase, IInterventionService
    {
        private const int DescriptionMaxLenght = 40;

        private readonly IServiceInterventionRepository repository;
        private readonly IServiceInterventionFactory interventionFactory;

        public InterventionService(
            IServiceInterventionRepository repository,
            IServiceInterventionFactory interventionFactory,
            IModelValidator modelValidator)
            : base(modelValidator)
        {
            this.interventionFactory = interventionFactory;
            this.repository = repository;
        }

        public async Task<IEnumerable<ServiceInterventionListViewModel>> All()
        {
            return (await this.repository
                .GetAsync(nameof(ServiceIntervention.DateTime), OrderDirection.Descending))
                .Select(si => new ServiceInterventionListViewModel
                {
                    Id = si.Id,
                    Date = si.DateTime.ToSimplifiedDateString(),
                    Mileage = si.Mileage,
                    ShortDescription = si.Description.Length > DescriptionMaxLenght
                        ? si.Description.Substring(0, DescriptionMaxLenght) + "..."
                        : si.Description
                });
        }

        public async Task<IServiceOperationResult> CreateAsync(
            DateTime currentDate, 
            DateTime nextInterventionDate, 
            int mileage, 
            string description, 
            string vehicleId)
        {
            try
            {
                var intervention = this.interventionFactory.CreateInstance(
                    currentDate, nextInterventionDate, mileage, description, vehicleId);

                await this.repository.CreateAsync(intervention);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<ServiceIntervention> GetAsync(string id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<IEnumerable<ServiceInterventionListViewModel>> GetByVehicleIdAsync(
            string vehicleId)
        {
            var interventions = await this.repository.GetByVehicleIdAsync(vehicleId);
            return interventions
                .Where(si => si.VehicleId == vehicleId)
                .Select(si => new ServiceInterventionListViewModel
                {
                    Id = si.Id,
                    Date = si.DateTime.ToSimplifiedDateString(),
                    Mileage = si.Mileage,
                    TotalCost = si.ServiceItems.Sum(sit => sit.TotalCost),
                    ShortDescription = si.Description.Length > DescriptionMaxLenght
                            ? si.Description.Substring(0, DescriptionMaxLenght) + "..."
                            : si.Description
                })
                .ToList();
        }

        public async Task<IServiceOperationResult> RemoveAsync(string id)
        {
            try
            {
                var intervention = await this.GetAsync(id);

                await this.repository.DeleteAsync(intervention);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<IServiceOperationResult> UpdateAsync(ServiceIntervention intervention)
        {
            try
            {
                this.modelValidator.Validate(intervention);

                await this.repository.UpdateAsync(intervention);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }
    }
}
