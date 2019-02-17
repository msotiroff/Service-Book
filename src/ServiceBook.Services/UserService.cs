using ServiceBook.DAL.Interfaces;
using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Enums;
using ServiceBook.Models.Interfaces;
using ServiceBook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBook.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository repository;
        private readonly IUserFactory userFactory;

        public UserService(
            IUserRepository repository,
            IUserFactory userFactory,
            IModelValidator modelValidator)
            : base(modelValidator)
        {
            this.repository = repository;
            this.userFactory = userFactory;
        }

        public async Task<IEnumerable<User>> AllAsync(
            string orderMember, 
            OrderDirection orderDirection = OrderDirection.Ascending, 
            int pageIndex = 1, 
            int itemsPerPage = int.MaxValue)
        {
            return await this.repository
                .GetAsync(orderMember, orderDirection, pageIndex, itemsPerPage);
        }

        public async Task<IServiceOperationResult> CreateAsync(
            string email, 
            string firstName, 
            string lastName, 
            string phoneNumber)
        {
            try
            {
                var user = this.userFactory
                .CreateInstance(email, firstName, lastName, phoneNumber);

                await this.repository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }

        public async Task<IEnumerable<User>> FilterAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await this.repository.FilterAsync(searchTerm);
        }

        public async Task<User> GetAsync(string id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<IServiceOperationResult> UpdateAsync(User user)
        {
            try
            {
                this.modelValidator.Validate(user);

                await this.repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return new ServiceOperationResult(ex.Message);
            }

            return ServiceOperationResult.Succeeded;
        }
    }
}
