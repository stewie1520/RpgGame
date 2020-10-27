using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RgpGame.DTOs.CharacterSkill;
using RgpGame.Services.CharacterSkillService;
using System.Threading.Tasks;

namespace RgpGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterSkillController : ControllerBase
    {
        public readonly ICharacterSkillService _service;
        public CharacterSkillController(ICharacterSkillService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCharacterSkill(AddCharacterSkillDto newCharacterS)
        {
            var response = await _service.AddCharacterSkill(newCharacterS);
            return Ok(response);
        }
    }
}