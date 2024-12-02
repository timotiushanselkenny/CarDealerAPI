using CarDealerAPI.Controllers;
using CarDealerAPI.Model;
using CarDealerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CarCarAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly CarDataRepository _repository;

        public CarController(DatabaseContext context)
        {
            _repository = new CarDataRepository(context);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve the Authorization header
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var items = await _repository.GetAllByDealerIDAsync(DealerID);
                return Ok(items);
            }
            return BadRequest(parseDealerID);

        }
        [HttpGet]
        [Route("searchbymakemodel")]
        public async Task<IActionResult> GetByMakeAndModel(string make,string model)
        {
            if (String.IsNullOrEmpty(make) || String.IsNullOrEmpty(model))
            {
                // Return validation errors if the model is invalid
                return BadRequest("Make and Model cannot be empty ! ");
            }
            // Retrieve the Authorization header
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var items = await _repository.GetByDealerIDMakeModelAsync(DealerID,make,model);
                return Ok(items);
            }
            return BadRequest(parseDealerID);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {                           
            // Retrieve the Authorization header
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var item = await _repository.GetByIdAsync(id,DealerID);
                if (item == null) return NotFound();
                return Ok(item);
            }
            return BadRequest(parseDealerID);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCarDataModel item)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors if the model is invalid
                return BadRequest(ModelState);
            }
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var result = await _repository.CreateAsync(new CarDataModel
                {
                    CarMake=item.CarMake,
                    CarModel=item.CarModel,
                    CarStock=item.CarStock,
                    CarYear=item.CarYear,
                    DealerID =DealerID,
                    InsertBy = DealerID,
                    InsertTime =DateTime.Now.ToString()
                });

                if (result.GetType().GetProperty("Id") != null)
                {
                    var newItem = await _repository.GetByIdAsync(result.Id,DealerID);
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, newItem);
                }
                // Return failure response
                return BadRequest(new { message = "Insert failed", error = result.ex.Message });
            }
            return BadRequest(parseDealerID);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] int stock)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var existingItem = await _repository.GetByIdAsync(id, DealerID);
                if (existingItem == null) return NotFound();

                //Update car stock
                existingItem.CarStock = stock;
                existingItem.UpdateBy = DealerID;
                existingItem.UpdateTime = DateTime.Now.ToString();
                await _repository.UpdateAsync(existingItem);
                return NoContent();
            }
            return BadRequest(parseDealerID);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string parseDealerID = JwtTokenHelper.ReturnDealerID(authHeader);
            int DealerID = 0;
            Int32.TryParse(parseDealerID, out DealerID);
            if (DealerID > 0)
            {
                var existingItem = await _repository.GetByIdAsync(id,DealerID);
                if (existingItem == null) return NotFound();
                await _repository.DeleteAsync(id,DealerID, DealerID.ToString());
                return Ok();
            }
            return BadRequest(parseDealerID);
        }
    }
}
