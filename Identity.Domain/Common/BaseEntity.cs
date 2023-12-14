namespace Identity.Domain.Common;

public class BaseEntity
{
    public int Id { get; private set; }
    public DateTime ProduceDate { get; private set; }

    public BaseEntity()
    {
        ProduceDate = DateTime.Now;
    }
}
