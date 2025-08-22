using CustomerManagementApp.Models;

namespace CustomerManagementApp.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerModel> GetAllCustomers();
        bool AddCustomer(CustomerModel customer, out string errorMessage);
    }
}
