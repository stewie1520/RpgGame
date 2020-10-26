using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RgpGame.Data;
using RgpGame.DTOs.Character;
using RgpGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RgpGame.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private IMapper _mapper;
        private DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            await _context.Characters.AddAsync(_mapper.Map<Character>(newCharacter));
            await _context.SaveChangesAsync();

            response.Data = (from c in _context.Characters select _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            var dbCharacters = await _context.Characters.Where(c => c.User.Id == userId).ToListAsync();

            response.Data = (from c in dbCharacters select _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetSingle(int id)
        {
            var foundCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            var response = new ServiceResponse<GetCharacterDto>();
            response.Data = _mapper.Map<GetCharacterDto>(foundCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto character)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                var matchedCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == character.Id);
                matchedCharacter.Name = character.Name;
                matchedCharacter.Class = character.Class;
                matchedCharacter.Defense = character.Defense;
                matchedCharacter.HitPoints = character.HitPoints;
                matchedCharacter.Intelligence = character.Intelligence;
                matchedCharacter.Strength = character.Strength;

                _context.Characters.Update(matchedCharacter);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);

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
            Character character = await _context.Characters.FirstAsync(c => c.Id == id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceResponse;
        }
    }
}
