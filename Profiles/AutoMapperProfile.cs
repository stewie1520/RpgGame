using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RgpGame.DTOs.Character;
using RgpGame.DTOs.Weapon;
using RgpGame.Models;

namespace RgpGame.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, GetCharacterDto>();
            CreateMap<Weapon, GetWeaponDto>();
        }
    }
}
