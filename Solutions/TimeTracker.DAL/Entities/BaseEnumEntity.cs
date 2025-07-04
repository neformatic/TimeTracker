namespace TimeTracker.DAL.Entities;

public class BaseEnumEntity<T> where T : Enum
{
    public T Id { get; set; }
    public string Description { get; set; }
}