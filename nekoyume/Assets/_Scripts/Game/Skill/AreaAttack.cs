using System;
using Nekoyume.Model;
using Nekoyume.TableData;

namespace Nekoyume.Game
{
    [Serializable]
    public class AreaAttack : AttackSkill
    {
        public AreaAttack(SkillSheet.Row skillRow, int power, decimal chance) : base(skillRow, power, chance)
        {
        }

        public override Model.Skill Use(CharacterBase caster)
        {
            return new Model.AreaAttack((CharacterBase) caster.Clone(), ProcessDamage(caster), ProcessBuff(caster));
        }
    }
}
