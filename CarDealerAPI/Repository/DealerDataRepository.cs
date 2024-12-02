using CarDealerAPI.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Repository
{
    public class DealerDataRepository
    {
        private readonly DatabaseContext _context;

        public DealerDataRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DealerDataModel>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DealerDataModel>("SELECT * FROM DealerData WHERE IsDeleted=0");
        }
        public async Task<DealerDataModel> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<DealerDataModel>("SELECT * FROM DealerData WHERE DealerID = @Id AND IsDeleted=0", new { Id = id });
        }
        public async Task<DealerDataModel> GetByUsernameAsync(string username)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<DealerDataModel>("SELECT * FROM DealerData WHERE DealerUsername = @username AND IsDeleted=0", new { username = username });
        }
        public async Task<dynamic> CreateAsync(DealerDataModel item)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var sql =
                    @"INSERT INTO DealerData (DealerName, DealerUsername, DealerSecretKey,IsDeleted,InsertBy,InsertTime) 
                VALUES (@DealerName,@DealerUsername,@DealerSecretKey, 0,@InsertBy,@InsertTime);";


                connection.Open();

                // Execute the insert and return the last inserted ID
                var insertedId = await connection.ExecuteAsync(sql, new
                {
                    DealerName = item.DealerName,
                    DealerUsername = item.DealerUsername,
                    DealerSecretKey = item.DealerSecretKey,
                    InsertBy = item.InsertBy,
                    InsertTime = item.InsertTime
                });
                if (insertedId > 0)
                {
                    // Retrieve the last inserted row ID
                    string lastIdQuery = "SELECT last_insert_rowid();";
                    var lastId = await connection.ExecuteScalarAsync<int>(lastIdQuery);
                    connection.Close();
                    return new { Id = lastId }; // Return the last inserted ID
                }
                connection.Close();
                return new
                {
                    ex = new 
                    {
                        Message = "Failed to insert dealer data."
                    }
                };
            }
            catch (Exception ex)
            {
                return new { ex = ex };
            }
        }

        public async Task<int> UpdateAsync(DealerDataModel item)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(
                @"UPDATE DealerData 
                SET DealerName = @DealerName, DealerUsername = @DealerUsername, DealerSecretKey = @DealerSecretKey
                WHERE DealerID = @DealerID",
                item);
        }

        public async Task<int> DeleteAsync(int id, string UpdatedBy)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(@"UPDATE DealerData SET IsDeleted=1,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE DealerID = @Id",
                new { UpdateBy = UpdatedBy, UpdateTime = DateTime.Now.ToString(),Id = id });
        }
    }
}
