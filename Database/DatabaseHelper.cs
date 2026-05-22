using Dapper;
using Microsoft.Data.Sqlite;
using SoundBoardLite.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoundBoardLite.Database;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper()
    {
        // Plik bazy będzie obok .exe
        var dbPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "soundboard.db");
        _connectionString = $"Data Source={dbPath}";
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        var createSoundsTable = @"
            CREATE TABLE IF NOT EXISTS Sounds (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                FilePath TEXT NOT NULL,
                Volume REAL NOT NULL,
                Shortcut TEXT
            );";
        await connection.ExecuteAsync(createSoundsTable);

        var createSettingsTable = @"
            CREATE TABLE IF NOT EXISTS Settings (
                Key TEXT PRIMARY KEY,
                Value TEXT NOT NULL
            );";
        await connection.ExecuteAsync(createSettingsTable);
    }

    public async Task<List<SoundItem>> GetSoundsAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var sounds = await connection.QueryAsync<SoundItem>("SELECT * FROM Sounds");
        return sounds.ToList();
    }

    public async Task AddSoundAsync(SoundItem sound)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "INSERT INTO Sounds (Name, FilePath, Volume, Shortcut) VALUES (@Name, @FilePath, @Volume, @Shortcut)";
        await connection.ExecuteAsync(sql, sound);
    }

    public async Task UpdateSoundAsync(SoundItem sound)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "UPDATE Sounds SET Name = @Name, FilePath = @FilePath, Volume = @Volume, Shortcut = @Shortcut WHERE Id = @Id";
        await connection.ExecuteAsync(sql, sound);
    }

    public async Task DeleteSoundAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM Sounds WHERE Id = @Id", new { Id = id });
    }

    public async Task<string?> GetSettingAsync(string key)
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<string>("SELECT Value FROM Settings WHERE Key = @Key", new { Key = key });
    }

    public async Task SetSettingAsync(string key, string value)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"
            INSERT INTO Settings (Key, Value) VALUES (@Key, @Value)
            ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value;";
        await connection.ExecuteAsync(sql, new { Key = key, Value = value });
    }
}
