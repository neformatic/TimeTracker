namespace TimeTracker.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(long entityId) : base(CreateMessage(entityId))
    {
    }

    private static string CreateMessage(long entityId)
    {
        return $"Entity was not found. EntityId: {entityId};";
    }
}