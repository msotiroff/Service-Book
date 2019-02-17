using Microsoft.EntityFrameworkCore;
using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;
using ServiceBook.Models.ViewModels.ServiceItem;
using ServiceBook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceBook.Services
{
    public class ItemService : ServiceBase, IItemService
    {
        private readonly IServiceItemRepository repository;
        private readonly IServiceItemFactory factory;

        public ItemService(
            IServiceItemRepository repository,
            IServiceItemFactory factory,
            IModelValidator modelValidator)
            : base(modelValidator)
        {
            this.repository = repository;
            this.factory = factory;
        }

        public async Task<IServiceOperationResult> CreateAsync(
            string part, decimal pricePerUnit, double units, string interventionId)
        {
            try
            {
                var item = this.factory
                .CreateInstance(part, pricePerUnit, units, interventionId);

                await this.repository.CreateAsync(item);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<ServiceItem> GetAsync(string id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<IEnumerable<ServiceItemListViewModel>> GetByInterventionIdAsync(string id)
        {
            return (await this.repository.GetByInterventionIdAsync(id))
                .OrderByDescending(si => si.ServiceIntervention.DateTime)
                .Where(si => si.ServiceInterventionId == id)
                .Select(si => new ServiceItemListViewModel
                {
                    Id = si.Id,
                    Part = si.Part,
                    PricePerUnit = si.PricePerUnit,
                    Units = si.Units
                });
        }

        public async Task<IServiceOperationResult> RemoveAsync(string id)
        {
            try
            {
                var item = await this.GetAsync(id);

                await this.repository.DeleteAsync(item);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<IServiceOperationResult> UpdateAsync(ServiceItem item)
        {
            try
            {
                this.modelValidator.Validate(item);

                await this.repository.UpdateAsync(item);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }
    }
}
