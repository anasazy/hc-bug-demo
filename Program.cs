using Demo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ArticleRepository>();
builder.Services.AddSingleton<CustomerRepository>();

builder.Services
	.AddGraphQLServer()
	.AddQueryType<QueryType>()

	// Swap the next two line to see, that only for the later one the output type transformation will work
	.AddTypeExtension<ArticleQueryTypeExtension>()
	.AddTypeExtension<CustomerQueryTypeExtension>()

	.AddFiltering()
	.AddSorting()
	.AddProjections()
;

var app = builder.Build();

app.MapGraphQL();
app.MapGet("/", context =>
{
	context.Response.Redirect("/graphql", false);
	return Task.FromResult(0);
});

app.Run();
