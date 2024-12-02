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
    public class AuthController : ControllerBase
    {
        private readonly DealerDataRepository _repository;

        public AuthController(DatabaseContext context)
        {
            _repository = new DealerDataRepository(context);
        }
        private const string SecretKey = "SecretCarDealerKey!@#$()"; // Replace with your secret key
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var DealerData = await _repository.GetByUsernameAsync(request.Username);
            if(DealerData==null)
                return Unauthorized();
            if (request.Password == DealerData.DealerSecretKey) // Example user
            {
                var token = JwtTokenHelper.GenerateJwtToken(request.Username,DealerData.DealerID, SecretKey);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
