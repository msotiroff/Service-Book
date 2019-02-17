namespace ServiceBook.Models.Interfaces
{
    public interface IModelValidator
    {
        /// <summary>
        /// Throws an exception if model state is not valid.
        /// </summary>
        void Validate(object model);
    }
}
