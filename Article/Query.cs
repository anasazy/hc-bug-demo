namespace Demo;

public partial record Query
{
	public IEnumerable<Article> GetArticles([Service] ArticleRepository repository) =>
		repository.GetArticles();

	public IEnumerable<Article> GetArticle([Service] ArticleRepository repository, Guid articleId) =>
		repository.GetArticle(articleId);
}
