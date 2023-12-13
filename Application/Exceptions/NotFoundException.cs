namespace Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object entityId)
        : base($"Entity '{entityName}' with ID '{entityId}' was not found.")
    {
    }
}
