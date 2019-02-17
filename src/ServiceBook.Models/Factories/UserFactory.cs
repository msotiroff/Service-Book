using ServiceBook.Models.DatabaseModels;
using ServiceBook.Models.Interfaces;
using System;

namespace ServiceBook.Models.Factories
{
    public class UserFactory : IUserFactory
    {
        private readonly IModelValidator modelValidator;

        public UserFactory(IModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
        }

        public User CreateInstance(
            string email, string firstName, string lastNasme, string phoneNumber)
        {
            var user = new User
            {
                Email = email,
                FirstName = firstName,
                LastName = lastNasme,
                PhoneNumber = phoneNumber,
                DateCreated = DateTime.UtcNow
            };

            modelValidator.Validate(user);

            return user;
        }
    }
}
