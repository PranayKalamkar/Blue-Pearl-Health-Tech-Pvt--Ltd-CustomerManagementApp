using CustomerManagementApp.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CustomerManagementApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<CustomerModel> GetAllCustomers()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("CALL sp_GetAllCustomers();", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerModel
                            {
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                CustomerName = reader["CustomerName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                                Address = reader["Address"].ToString()
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Log or handle MySQL-specific errors
                Console.Error.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle general errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
            return customers;
        }

        public bool AddCustomer(CustomerModel customer, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = new MySqlCommand("sp_AddCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_CustomerName", customer.CustomerName);
                        cmd.Parameters.AddWithValue("p_Email", customer.Email);
                        cmd.Parameters.AddWithValue("p_PhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("p_DateOfBirth", customer.DateOfBirth);
                        cmd.Parameters.AddWithValue("p_Address", customer.Address);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                // MySQL 45000 = custom SIGNAL
                errorMessage = ex.Message.Contains("Email already exists")
                    ? "Email already exists. Please use a different email."
                    : "Database error occurred.";
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = $"Unexpected error: {ex.Message}";
                return false;
            }
        }

    }
}
