using ServiceBook.Models.Interfaces;

namespace ServiceBook.Services
{
    public abstract class ServiceBase
    {
        protected readonly IModelValidator modelValidator;

        public ServiceBase(IModelValidator modelValidator)
        {
            this.modelValidator = modelValidator;
        }
    }
}
