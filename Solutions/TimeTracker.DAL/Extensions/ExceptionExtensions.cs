using Npgsql;
using TimeTracker.DAL.Constants;

namespace TimeTracker.DAL.Extensions;

public static class ExceptionExtensions
{
    public static bool CheckPostgresUniqueConstraintException(this Exception ex)
    {
        var result = ex.GetBaseException() is PostgresException postgresEx
                     && postgresEx.SqlState == DatabaseErrorCodeConstants.Duplicate;

        return result;
    }
}