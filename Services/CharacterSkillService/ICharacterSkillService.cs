using System.Threading.Tasks;
using RgpGame.DTOs.Character;
using RgpGame.DTOs.CharacterSkill;
using RgpGame.Models;

namespace RgpGame.Services.CharacterSkillService
{
    public interface ICharacterSkillService
    {
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
    }
}