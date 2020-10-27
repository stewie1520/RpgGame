using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RgpGame.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public virtual User User { get; set; }
        public virtual Weapon Weapon { get; set; }
        public virtual List<CharacterSkill> CharacterSkills { get; set; }
    }
}
