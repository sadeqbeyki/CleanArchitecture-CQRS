namespace Domain.Entities.CustomerAgg
{
    public class Address : BaseEntity<int>
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

    }
}
