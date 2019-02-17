using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;
using ServiceBook.Models.ViewModels.Vehicle;
using ServiceBook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBook.Services
{
    public class VehicleService : ServiceBase, IVehicleService
    {
        private readonly IVehicleRepository repository;
        private readonly IVehicleFactory vehicleFactory;

        public VehicleService(
            IVehicleRepository repository,
            IVehicleFactory vehicleFactory,
            IModelValidator modelValidator)
            : base(modelValidator)
        {
            this.repository = repository;
            this.vehicleFactory = vehicleFactory;
        }

        public async Task<IEnumerable<VehicleListViewModel>> AllAsync(
            string orderMember, 
            OrderDirection orderDirection = OrderDirection.Ascending, 
            int pageIndex = 1, 
            int itemsPerPage = int.MaxValue)
        {
            return (await this.repository
                .GetAsync(orderMember, orderDirection, pageIndex, itemsPerPage))
                .Select(v => new VehicleListViewModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    ExactModelName = v.ExactModelName,
                    OwnerFullName = v.Owner.FullName,
                    RegistrationPlate = v.RegistrationPlate,
                    ServiceInterventionsCount = v.ServiceInterventions.Count()
                });
        }

        public async Task<IServiceOperationResult> CreateAsync(
            string make, 
            string model, 
            string exactModelName, 
            string vin, 
            string regPlate, 
            string ownerId)
        {
            try
            {
                var vehicle = this.vehicleFactory
                .CreateInstance(make, model, exactModelName, vin, regPlate, ownerId);

                await this.repository.CreateAsync(vehicle);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<IEnumerable<VehicleListViewModel>> FilterAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return (await this.repository.FilterAsync(searchTerm))
                .Select(v => new VehicleListViewModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    ExactModelName = v.ExactModelName,
                    RegistrationPlate = v.RegistrationPlate,
                    OwnerFullName = v.Owner.FullName,
                    ServiceInterventionsCount = v.ServiceInterventions.Count()
                });
        }

        public async Task<Vehicle> GetAsync(string id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<IEnumerable<VehicleListViewModel>> GetByUserIdAsync(string id)
        {
            return (await this.repository.GetByUserIdAsync(id))
                .Where(v => v.OwnerId == id)
                .Select(v => new VehicleListViewModel
                {
                    Id = v.Id,
                    Make = v.Make,
                    Model = v.Model,
                    ExactModelName = v.ExactModelName,
                    RegistrationPlate = v.RegistrationPlate,
                    OwnerFullName = v.Owner.FullName,
                    ServiceInterventionsCount = v.ServiceInterventions.Count()
                });
        }

        public async Task<IServiceOperationResult> UpdateAsync(Vehicle vehicle)
        {
            try
            {
                this.modelValidator.Validate(vehicle);

                await this.repository.UpdateAsync(vehicle);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }
    }
}
