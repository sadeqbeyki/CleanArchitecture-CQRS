namespace Application.DTOs
{
    public class ProductDetailsDto : ProductDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class AddProductDto : ProductDto
    {
    }
    public class UpdateProductDto : ProductDto
    {
        public Guid Id { get; set; }
    }
}
