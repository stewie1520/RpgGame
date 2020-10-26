using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RgpGame.Data;
using RgpGame.DTOs.User;
using RgpGame.Models;

namespace RgpGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
    
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ServiceResponse<string> { Success = false, Message = "Body is not valid" });
            }
            var response = await _authRepository.Register(
                new User { Username = user.Username }, user.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var response = await _authRepository.Login(user.Username, user.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
