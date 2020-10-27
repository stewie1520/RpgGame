using System.Collections.Generic;

namespace RgpGame.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public virtual List<CharacterSkill> CharacterSkills { get; set; }
    }
}