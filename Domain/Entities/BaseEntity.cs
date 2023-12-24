namespace Domain.Entities;

public class BaseEntity<T>
{
    public T Id { get; private set; }/* = default!;*/
    public DateTime CreatedAt { get; private set; }

    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
    }
}
