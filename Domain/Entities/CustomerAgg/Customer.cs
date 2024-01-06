using Domain.Entities.OrderAgg;

namespace Domain.Entities.CustomerAgg
{
    public class Customer : BaseEntity<int>
    {
        public string Surname { get; set; }
        public string Forename { get; set; }
        public int Age { get; set; }
        public decimal Discount { get; set; }
        public Address Address { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
