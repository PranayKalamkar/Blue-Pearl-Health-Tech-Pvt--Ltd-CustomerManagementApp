using CustomerManagementApp.Models;

namespace CustomerManagementApp.Repositories
{
    public interface IUserRepository
    {
        UserModel ValidateUser(string username, string password);
        UserModel RegisterUser(string username, string password);
    }
}
