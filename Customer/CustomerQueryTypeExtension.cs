namespace Demo;

public class CustomerQueryTypeExtension : ObjectTypeExtension<Query>
{
	protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
	{
		descriptor.Field(f => f.GetCustomers(default!))
			.Description("Get many customers")
			.UsePaging()
			.UseFiltering()
			.UseSorting()
		;

		descriptor.Field(f => f.GetCustomer(default!, default))
			.Description("Get one customer")
			.UseFirstOrDefault()
		;
	}
}
