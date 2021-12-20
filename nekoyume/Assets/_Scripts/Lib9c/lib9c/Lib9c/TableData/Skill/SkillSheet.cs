using System;
using System.Collections.Generic;
using Nekoyume.Model.Elemental;
using Nekoyume.Model.Skill;
using Nekoyume.Model.State;
using static Nekoyume.TableData.TableExtensions;

namespace Nekoyume.TableData
{
    [Serializable]
    public class SkillSheet : Sheet<int, SkillSheet.Row>
    {
        [Serializable]
        public class Row : SheetRow<int>, IState
        {
            public override int Key => Id;
            public int Id { get; private set; }
            public ElementalType ElementalType { get; private set; }
            public SkillType SkillType { get; private set; }
            public SkillCategory SkillCategory { get; private set; }
            public SkillTargetType SkillTargetType { get; private set; }
            public int HitCount { get; private set; }
            public int Cooldown { get; private set; }

            public override void Set(IReadOnlyList<string> fields)
            {
                Id = ParseInt(fields[0]);
                ElementalType = (ElementalType) Enum.Parse(typeof(ElementalType), fields[1]);
                SkillType = (SkillType) Enum.Parse(typeof(SkillType), fields[2]);
                SkillCategory = (SkillCategory) Enum.Parse(typeof(SkillCategory), fields[3]);
                SkillTargetType = (SkillTargetType) Enum.Parse(typeof(SkillTargetType), fields[4]);
                HitCount = ParseInt(fields[5]);
                Cooldown = ParseInt(fields[6]);
            }
        }

        public SkillSheet() : base(nameof(SkillSheet))
        {
        }
    }
}
