namespace Demo;

public record Article
{
	public Guid Id { get; init; }
	public string Name { get; init; } = default!;
	public float Price { get; init; }
}
