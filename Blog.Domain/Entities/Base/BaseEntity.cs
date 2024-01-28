namespace Blog.Domain.Entities.Base;

public class BaseEntity<T>
{
    public T Id { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
    }
}
