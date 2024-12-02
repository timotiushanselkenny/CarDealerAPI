using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Model
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

        public async Task Init()
        {
            using var connection = CreateConnection();
            try
            {
                connection.Open();

                // Create tables and insert default data
                var sql = @"
                CREATE TABLE IF NOT EXISTS DealerData (
                    DealerID        INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    DealerName      TEXT    NOT NULL,
                    DealerUsername  TEXT    NOT NULL UNIQUE,
                    DealerSecretKey TEXT    NOT NULL,
                    IsDeleted       INTEGER NOT NULL DEFAULT (0),
                    InsertBy        TEXT    NOT NULL,
                    InsertTime      TEXT    NOT NULL,
                    UpdateBy        TEXT,
                    UpdateTime      TEXT
                );

                CREATE TABLE IF NOT EXISTS CarData (
                    CarID      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                    CarMake    TEXT    NOT NULL,                      
                    CarModel   TEXT    NOT NULL,
                    CarYear    INTEGER NOT NULL,
                    CarStock   INTEGER NOT NULL,
                    DealerID   INTEGER NOT NULL REFERENCES DealerData (DealerID),
                    IsDeleted  INTEGER NOT NULL DEFAULT (0),
                    InsertBy   TEXT    NOT NULL,
                    InsertTime TEXT    NOT NULL, 
                    UpdateBy   TEXT,
                    UpdateTime TEXT
                );

                INSERT INTO DealerData (
                    UpdateTime,
                    UpdateBy,
                    InsertTime,
                    InsertBy,
                    IsDeleted,
                    DealerSecretKey,
                    DealerUsername,
                    DealerName,
                    DealerID
                )
                SELECT 
                    NULL,
                    NULL,
                    '2024-12-02T10:15:22',
                    'THK',
                    0,
                    'THK',
                    'THK',
                    'Timotius Hansel Kenny',
                    1
                WHERE NOT EXISTS (
                    SELECT 1 FROM DealerData
                );
            ";

                await connection.ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                // Handle errors (log or rethrow based on use case)
                Console.WriteLine($"Database initialization failed: {ex.Message}");
                throw;
            }
        }
    }
}
