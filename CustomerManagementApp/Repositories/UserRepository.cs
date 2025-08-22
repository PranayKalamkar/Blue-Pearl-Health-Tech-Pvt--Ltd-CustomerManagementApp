using CustomerManagementApp.Models;
using MySql.Data.MySqlClient;

namespace CustomerManagementApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public UserModel RegisterUser(string username, string password)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("CALL sp_RegisterUser(@in_username, @in_password);", conn);
                    cmd.Parameters.AddWithValue("@in_username", username);
                    cmd.Parameters.AddWithValue("@in_password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && Convert.ToInt32(reader["UserId"]) > 0)
                        {
                            return new UserModel
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString()
                            };
                        }
                        return null; // Duplicate
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public UserModel ValidateUser(string username, string password)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("CALL sp_ValidateUser(@in_username, @in_password);", conn);
                    cmd.Parameters.AddWithValue("@in_username", username);
                    cmd.Parameters.AddWithValue("@in_password", password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserModel
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString()
                            };
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                            return null;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.Error.WriteLine($"MySQL Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
