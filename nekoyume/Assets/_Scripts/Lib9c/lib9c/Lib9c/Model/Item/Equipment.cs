using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Nekoyume.Extensions;
using Nekoyume.Model.Stat;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Equipment : ItemUsable, IEquippableItem
    {
        // FIXME: Whether the equipment is equipped or not has no asset value and must be removed from the state.
        public bool equipped { get; set; }
        public int level { get; set; }
        public int optionCountFromCombination;

        public DecimalStat Stat { get; }
        public int SetId { get; }
        public string SpineResourcePath { get; }
        public StatType UniqueStatType => Stat.Type;
        public bool Equipped => equipped;

        public decimal GetIncrementAmountOfEnhancement()
        {
            return Math.Max(1.0m, StatsMap.GetStat(UniqueStatType, true) * 0.1m);
        }

        public Equipment(EquipmentItemSheet.Row data, string id) : base(data, id)
        {
            Stat = data.Stat;
            SetId = data.SetId;
            SpineResourcePath = data.SpineResourcePath;
        }

        public void Equip()
        {
            equipped = true;
        }

        public void Unequip()
        {
            equipped = false;
        }

        // FIXME: 기본 스탯을 복리로 증가시키고 있는데, 단리로 증가시켜야 한다.
        // 이를 위해서는 기본 스탯을 유지하면서 추가 스탯에 더해야 하는데, UI 표현에 문제가 생기기 때문에 논의 후 개선한다.
        // 장비가 보유한 스킬의 확률과 수치 강화가 필요한 상태이다.
        public void LevelUp()
        {
            level++;
            StatsMap.AddStatValue(UniqueStatType, GetIncrementAmountOfEnhancement());
            if (new[] {4, 7, 10}.Contains(level) &&
                GetOptionCount() > 0)
            {
                UpdateOptions();
            }
        }

        public void LevelUpV2(Random random, EnhancementCostSheetV2.Row row, bool isGreatSuccess)
        {
            level++;
            var rand = isGreatSuccess ? row.BaseStatGrowthMax
                :random.Next(row.BaseStatGrowthMin, row.BaseStatGrowthMax + 1);
            var ratio = rand.NormalizeFromTenThousandths();
            var baseStat = StatsMap.GetStat(UniqueStatType, true) * ratio;
            if (baseStat > 0)
            {
                baseStat = Math.Max(1.0m, baseStat);
            }

            StatsMap.AddStatValue(UniqueStatType, baseStat);

            if (GetOptionCount() > 0)
            {
                UpdateOptionsV2(random, row, isGreatSuccess);
            }
        }

        public List<object> GetOptions()
        {
            var options = new List<object>();
            options.AddRange(Skills);
            options.AddRange(BuffSkills);
            foreach (var statMapEx in StatsMap.GetAdditionalStats())
            {
                options.Add(new StatModifier(
                    statMapEx.StatType,
                    StatModifier.OperationType.Add,
                    statMapEx.AdditionalValueAsInt));
            }

            return options;
        }

        private void UpdateOptions()
        {
            foreach (var statMapEx in StatsMap.GetAdditionalStats())
            {
                StatsMap.SetStatAdditionalValue(
                    statMapEx.StatType,
                    statMapEx.AdditionalValue * 1.3m);
            }

            var skills = new List<Skill.Skill>();
            skills.AddRange(Skills);
            skills.AddRange(BuffSkills);
            foreach (var skill in skills)
            {
                var chance = decimal.ToInt32(skill.Chance * 1.3m);
                var power = decimal.ToInt32(skill.Power * 1.3m);
                skill.Update(chance, power);
            }
        }

        private void UpdateOptionsV2(Random random, EnhancementCostSheetV2.Row row, bool isGreatSuccess)
        {
            foreach (var statMapEx in StatsMap.GetAdditionalStats())
            {
                var rand = isGreatSuccess
                    ? row.ExtraStatGrowthMax
                    : random.Next(row.ExtraStatGrowthMin, row.ExtraStatGrowthMax + 1);
                var ratio = rand.NormalizeFromTenThousandths();
                var addValue = statMapEx.AdditionalValue * ratio;
                if (addValue > 0)
                {
                    addValue = Math.Max(1.0m, addValue);
                }

                StatsMap.SetStatAdditionalValue(statMapEx.StatType, statMapEx.AdditionalValue + addValue);
            }

            var skills = new List<Skill.Skill>();
            skills.AddRange(Skills);
            skills.AddRange(BuffSkills);
            foreach (var skill in skills)
            {
                var chanceRand = isGreatSuccess ? row.ExtraSkillChanceGrowthMax
                    : random.Next(row.ExtraSkillChanceGrowthMin, row.ExtraSkillChanceGrowthMax + 1);
                var chanceRatio = chanceRand.NormalizeFromTenThousandths();
                var addChance = skill.Chance * chanceRatio;
                if (addChance > 0)
                {
                    addChance = Math.Max(1.0m, addChance);
                }

                var damageRand = isGreatSuccess ? row.ExtraSkillDamageGrowthMax
                    : random.Next(row.ExtraSkillDamageGrowthMin, row.ExtraSkillDamageGrowthMax + 1);
                var damageRatio = damageRand.NormalizeFromTenThousandths();
                var addPower = skill.Power * damageRatio;
                if (addPower > 0)
                {
                    addPower = Math.Max(1.0m, addPower);
                }

                skill.Update(skill.Chance + (int)addChance, skill.Power + (int)addPower);
            }
        }

        protected bool Equals(Equipment other)
        {
            return base.Equals(other) && equipped == other.equipped && level == other.level &&
                   Equals(Stat, other.Stat) && SetId == other.SetId && SpineResourcePath == other.SpineResourcePath;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Equipment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ equipped.GetHashCode();
                hashCode = (hashCode * 397) ^ level;
                hashCode = (hashCode * 397) ^ (Stat != null ? Stat.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SetId;
                hashCode = (hashCode * 397) ^ (SpineResourcePath != null ? SpineResourcePath.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}