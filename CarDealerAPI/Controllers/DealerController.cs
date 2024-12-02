using CarDealerAPI.Model;
using CarDealerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealerController : ControllerBase
    {
        private readonly DealerDataRepository _repository;

        public DealerController(DatabaseContext context)
        {
            _repository = new DealerDataRepository(context);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealerDataModel item)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors if the model is invalid
                return BadRequest(ModelState);
            }
            var result = await _repository.CreateAsync(new DealerDataModel
            {
                DealerName=item.DealerName,
                DealerSecretKey=item.DealerSecretKey,
                DealerUsername=item.DealerUsername,
                InsertTime=DateTime.Now,
                IsDeleted=false,
                InsertBy="admin"
            });       

            if (result.GetType().GetProperty("Id")!=null)
            {
                var newItem = await _repository.GetByIdAsync(result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, newItem);
            }
            // Return failure response
            return BadRequest(new { message = "Insert failed", error = result.ex.Message });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateDealerDataModel item)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors if the model is invalid
                return BadRequest(ModelState);
            }
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null) return NotFound();
            existingItem.DealerName = item.DealerName;
            existingItem.DealerSecretKey = item.DealerSecretKey;
            existingItem.DealerUsername = item.DealerUsername;
            existingItem.UpdateBy = "admin";
            existingItem.UpdateTime = DateTime.Now;
            existingItem.IsDeleted = false;
            await _repository.UpdateAsync(existingItem);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id,string DeleteBy)
        {
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null) return NotFound();
            await _repository.DeleteAsync(id,DeleteBy);
            return Ok();
        }
    }
}
