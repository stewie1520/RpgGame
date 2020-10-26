using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RgpGame.DTOs.Character;
using RgpGame.Services.CharacterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RgpGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private ICharacterService _service;
        public CharacterController(ICharacterService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get infomation of a character
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            return Ok(await _service.GetSingle(id));
        }

        [Authorize]
        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await _service.GetAllCharacter(userId));
        }

        /// <summary>
        /// Add new character to your character lists
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Character
        ///     {
        ///         "name": "Batman",
        ///         "hitPoints": 100,
        ///         "strength": 80,
        ///         "defense": 70,
        ///         "intelligence": 110,
        ///         "class": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="newCharacter"></param>
        /// <returns>A list of current characters of user</returns>
        /// <response code="200">Add new user successfully</response>
        [HttpPost]
        public async Task<IActionResult> AddCharacter([FromBody] AddCharacterDto newCharacter)
        {
            return Ok(await _service.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter([FromBody] UpdateCharacterDto character)
        {
            var response = await _service.UpdateCharacter(character);

            return response.Data == null ? (IActionResult)NotFound(response) : Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(int id)
        {
            var response = await _service.DeleteCharacter(id);

            return Ok(response);
        }
    }
}
