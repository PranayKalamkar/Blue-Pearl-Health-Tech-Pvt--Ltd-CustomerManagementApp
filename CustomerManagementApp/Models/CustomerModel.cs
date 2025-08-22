namespace CustomerManagementApp.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
        public string Address { get; set; } = string.Empty;
    }
}
