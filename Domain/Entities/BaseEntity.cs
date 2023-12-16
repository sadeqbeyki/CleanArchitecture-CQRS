namespace Domain.Entities;

public class BaseEntity
{
    public int Id { get; private set; }
    public DateTime ProduceDate { get; private set; }

    public BaseEntity()
    {
        ProduceDate = DateTime.Now;
    }
}
