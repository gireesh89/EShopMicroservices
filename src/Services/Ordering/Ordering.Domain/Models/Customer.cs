namespace Ordering.Domain.Models
{
    public class Customer:Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(CustomerId customerId,string Name,string Email)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(Name);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(Email);

            return new Customer
            {
                Id = customerId,
                Name = Name,
                Email = Email
            };
        }
    }
}
