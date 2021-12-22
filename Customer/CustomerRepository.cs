namespace Demo;

public record CustomerRepository
{
	private readonly List<Customer> customers = new List<Customer>();

	public CustomerRepository()
	{
		this.customers.Add(new Customer {
			Id = Guid.NewGuid(),
			Name = "Customer A",
			IsActive = true,
		});

		this.customers.Add(new Customer {
			Id = Guid.NewGuid(),
			Name = "Customer B",
			IsActive = false,
		});

		this.customers.Add(new Customer {
			Id = Guid.NewGuid(),
			Name = "Customer C",
			IsActive = true,
		});
	}

	public IEnumerable<Customer> GetCustomers()
	{
		return this.customers;
	}

	public IEnumerable<Customer> GetCustomer(Guid customerId)
	{
		return this.customers.Where(a => a.Id == customerId);
	}
}
