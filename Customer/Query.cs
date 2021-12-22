namespace Demo;

public partial record Query
{
	public IEnumerable<Customer> GetCustomers([Service] CustomerRepository repository) =>
		repository.GetCustomers();

	public IEnumerable<Customer> GetCustomer([Service] CustomerRepository repository, Guid customerId) =>
		repository.GetCustomer(customerId);
}
