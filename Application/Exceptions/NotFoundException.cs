namespace Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string entityName, object entityId)
        : base($"Entity '{entityName}' with ID '{entityId}' was not found.")
    { }
    public NotFoundException(int id) : base($"The product with the id '{id}' not found.")
    { }
    public NotFoundException(Guid id) : base($"The product with the id '{id}' not found.")
    { }
    public NotFoundException(string s) : base("not found.")
    { }
}
