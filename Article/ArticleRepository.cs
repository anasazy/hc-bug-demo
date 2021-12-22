namespace Demo;

public record ArticleRepository
{
	private readonly List<Article> articles = new List<Article>();

	public ArticleRepository()
	{
		this.articles.Add(new Article {
			Id = Guid.NewGuid(),
			Name = "Article A",
			Price = 9.99F,
		});

		this.articles.Add(new Article {
			Id = Guid.NewGuid(),
			Name = "Article B",
			Price = 4.95F,
		});

		this.articles.Add(new Article {
			Id = Guid.NewGuid(),
			Name = "Article C",
			Price = 12.34F,
		});
	}

	public IEnumerable<Article> GetArticles()
	{
		return this.articles;
	}

	public IEnumerable<Article> GetArticle(Guid articleId)
	{
		return this.articles.Where(a => a.Id == articleId);
	}
}
