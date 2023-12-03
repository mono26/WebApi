namespace Catalog.Api.Dtos
{
    /// <summary>
    /// Data transfer object that represents an item contract.
    /// </summary>
    /// <value></value>
    public record ItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}