using CarDealerAPI.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Repository
{
    public class CarDataRepository
    {
        private readonly DatabaseContext _context;

        public CarDataRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CarDataModel>> GetAllByDealerIDAsync(int DealerId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarDataModel>("SELECT * FROM CarData WHERE DealerID = @Id AND IsDeleted=0", new { Id = DealerId });
        }
        public async Task<IEnumerable<CarDataModel>> GetByDealerIDMakeModelAsync(int DealerId,string Make,string Model)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CarDataModel>(@"
                SELECT * FROM CarData 
                WHERE DealerID = @Id 
                AND CarMake LIKE @CarMake 
                AND CarModel LIKE @CarModel
                AND IsDeleted=0", new { Id = DealerId,CarMake = $"%{Make}%",CarModel= $"%{Model}%" });
        }
        public async Task<CarDataModel> GetByIdAsync(int id,int DealerID)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<CarDataModel>("SELECT * FROM CarData WHERE CarID = @Id AND DealerID=@DealerID AND IsDeleted=0", new { Id = id, DealerID= DealerID });
        }

        public async Task<dynamic> CreateAsync(CarDataModel item)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var sql =
                    @"INSERT INTO CarData (CarMake,CarModel, CarYear, CarStock,DealerID,IsDeleted,InsertBy,InsertTime) 
                VALUES (@CarMake,@CarModel,@CarYear,@CarStock, @DealerID,0,@InsertBy,@InsertTime);";

                connection.Open();

                // Execute the insert and return the last inserted ID
                var insertedId = await connection.ExecuteAsync(sql,
                new { CarMake = item.CarMake, CarYear = item.CarYear,CarModel=item.CarModel, CarStock = item.CarStock, DealerID = item.DealerID, InsertBy = item.InsertBy, InsertTime = item.InsertTime });
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
                            Message = "Failed to insert car data."
                        }
                    };
            }
            catch (Exception ex)
            {
                return new { ex = ex };
            }
        }

        public async Task<int> UpdateAsync(CarDataModel item)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(
                @"UPDATE CarData 
                SET CarMake = @CarMake, CarModel=@CarModel,CarYear = @CarYear, CarStock = @CarStock, DealerID = @DealerID
                WHERE CarID = @CarID",
                item);
        }

        public async Task<int> DeleteAsync(int id,int DealerID,string UpdatedBy)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync("UPDATE CarData SET IsDeleted=1,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE CarID = @Id AND DealerID=@DealerID", 
                new { Id = id,DealerID=DealerID, UpdateBy = UpdatedBy, UpdateTime = DateTime.Now });
        }
    }
}
