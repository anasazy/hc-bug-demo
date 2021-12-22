namespace Demo;

public record Customer
{
	public Guid Id { get; init; }
	public string Name { get; init; } = default!;
	public bool IsActive { get; init; }
}
