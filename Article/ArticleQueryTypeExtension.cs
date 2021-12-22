namespace Demo;

public class ArticleQueryTypeExtension : ObjectTypeExtension<Query>
{
	protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
	{
		descriptor.Field(f => f.GetArticles(default!))
			.Description("Get many articles")
			.UsePaging()
			.UseFiltering()
			.UseSorting()
		;

		descriptor.Field(f => f.GetArticle(default!, default))
			.Description("Get one article")
			.UseFirstOrDefault()
		;
	}
}
