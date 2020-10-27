using System.Threading.Tasks;
using RgpGame.Models;
using RgpGame.DTOs.Character;
using RgpGame.DTOs.Weapon;

namespace RgpGame.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
