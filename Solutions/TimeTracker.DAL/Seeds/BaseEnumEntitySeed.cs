using TimeTracker.Common.Extensions;
using TimeTracker.DAL.Entities;

namespace TimeTracker.DAL.Seeds;

public static class BaseEnumEntitySeed
{
    public static IEnumerable<T> GetItems<T, TK>() where T : BaseEnumEntity<TK>, new() where TK : struct, Enum
    {
        foreach (var item in (TK[])Enum.GetValues(typeof(TK)))
        {
            yield return new T
            {
                Id = item,
                Description = item.GetDescription()
            };
        }
    }
}