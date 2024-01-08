namespace Domain.Entities.OrderAgg
{
    public class Order : BaseEntity<int>
    {
        public double Cost { get; set; }
        public int Count { get; set; }
        public int CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public double Total { get; set; }
    }
}
