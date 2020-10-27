using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RgpGame.DTOs.Weapon;
using RgpGame.Services.WeaponService;
using System.Threading.Tasks;

namespace RgpGame.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        public readonly IWeaponService _service;
        public WeaponController(IWeaponService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = await _service.AddWeapon(newWeapon);
            return Ok(response);
        }
    }
}