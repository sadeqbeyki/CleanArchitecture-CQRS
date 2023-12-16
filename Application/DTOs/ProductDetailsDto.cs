namespace Application.DTOs
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public DateTime ProduceDate { get; set; }
        public string Name { get; set; }
        public string ManufacturerPhone { get; set; }
        public string ManufacturerEmail { get; set; }
        public bool IsAvailable { get; set; }
    }
}
