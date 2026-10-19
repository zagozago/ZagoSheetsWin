using Microsoft.Data.Sqlite;

namespace SheetsWindows.Infrastructure;

internal static class SqliteSchema
{
    // Opening an up-to-date registry is a read, not a competing schema writer.
    // Callers still recheck the version inside their immediate migration transaction.
    public static bool IsCurrent(SqliteConnection db, int expected, string newerMessage)
    {
        using var cmd = db.CreateCommand(); cmd.CommandText = "PRAGMA user_version";
        var version = Convert.ToInt32(cmd.ExecuteScalar());
        if (version > expected) throw new NotSupportedException(newerMessage);
        return version == expected;
    }
}
