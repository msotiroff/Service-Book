using ServiceBook.Models.DatabaseModels;

namespace ServiceBook.Models.Interfaces
{
    public interface IUserFactory
    {
        User CreateInstance(
            string email,
            string firstName,
            string lastNasme,
            string phoneNumber);
    }
}
