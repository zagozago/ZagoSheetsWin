using System.Text.Json;
using Microsoft.Data.Sqlite;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

// Immutable per-operation interpretation. Legacy remote intents retain the old parser,
// including its limitations; correcting an old document requires an explicit new import.
public sealed class ImportInterpretations(LocalStorage storage)
{
    private SqliteConnection Open()
    {
        var db = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = storage.DatabasePath, Pooling = false }.ToString());
        db.Open();
        using var schema = db.CreateCommand();
        schema.CommandText = "PRAGMA foreign_keys=ON; PRAGMA synchronous=FULL; CREATE TABLE IF NOT EXISTS import_interpretations(operation_id TEXT PRIMARY KEY REFERENCES operations(id), options TEXT NOT NULL);";
        schema.ExecuteNonQuery(); return db;
    }
    public TextImportOptions Bind(ImportOperation operation, TextImportOptions proposed, bool hasRemoteIntent)
    {
        using var db = Open(); using var tx = db.BeginTransaction(deferred: false);
        using var cmd = db.CreateCommand(); cmd.Transaction = tx;
        cmd.CommandText = "SELECT options FROM import_interpretations WHERE operation_id=$id";
        cmd.Parameters.AddWithValue("$id", operation.Id.ToString("N"));
        if (cmd.ExecuteScalar() is string saved)
        {
            var loaded = JsonSerializer.Deserialize<TextImportOptions>(saved) ?? throw new InvalidDataException("Invalid interpretation.");
            Validate(loaded); tx.Commit(); return loaded;
        }
        var options = proposed with { InterpretationVersion = hasRemoteIntent ? 1 : proposed.InterpretationVersion };
        Validate(options);
        cmd.CommandText = "INSERT INTO import_interpretations(operation_id,options) VALUES($id,$options)";
        cmd.Parameters.AddWithValue("$options", JsonSerializer.Serialize(options)); cmd.ExecuteNonQuery();
        tx.Commit(); return options;
    }
    public TextImportOptions Load(ImportOperation operation, TextImportOptions legacy, int fallbackVersion = 1)
    {
        using var db = Open(); using var cmd = db.CreateCommand();
        cmd.CommandText = "SELECT options FROM import_interpretations WHERE operation_id=$id";
        cmd.Parameters.AddWithValue("$id", operation.Id.ToString("N"));
        var options = cmd.ExecuteScalar() is string saved ? JsonSerializer.Deserialize<TextImportOptions>(saved)! : legacy with { InterpretationVersion = fallbackVersion };
        Validate(options); return options;
    }
    private static void Validate(TextImportOptions? options)
    {
        if (options is null || options.Encoding is not ("auto" or "windows-1252")
            || options.Delimiter is not ("auto" or "comma" or "semicolon" or "tab" or "pipe") || options.InterpretationVersion is not (1 or 2 or 3))
            throw new InvalidDataException("Invalid interpretation.");
    }
}
