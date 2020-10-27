using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RgpGame.Data;
using RgpGame.DTOs.Character;
using RgpGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RgpGame.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private IMapper _mapper;
        private DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private int GetUserId() =>
            int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);

            character.User = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());

            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();

            response.Data = (from c in _context.Characters select _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();

            response.Data = (from c in dbCharacters select _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetSingle(int id)
        {
            var foundCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

            var response = new ServiceResponse<GetCharacterDto>();
            response.Data = _mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var matchedCharacter = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == character.Id);
                if (matchedCharacter.User.Id == GetUserId())
                {
                    matchedCharacter.Name = character.Name;
                    matchedCharacter.Class = character.Class;
                    matchedCharacter.Defense = character.Defense;
                    matchedCharacter.HitPoints = character.HitPoints;
                    matchedCharacter.Intelligence = character.Intelligence;
                    matchedCharacter.Strength = character.Strength;

                    _context.Characters.Update(matchedCharacter);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);

                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User is not found";
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User is not found";
                }
                else
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = (_context.Characters.Where(c => c.User.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
