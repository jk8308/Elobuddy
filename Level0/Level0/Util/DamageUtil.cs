﻿using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using LevelZero.Model;

namespace LevelZero.Util
{
    static class DamageUtil
    {
        public static List<SpellDamage> SpellsDamage { get; set; }

        public static float GetComboDamage(Obj_AI_Base enemy)
        {
            return SpellsDamage.Where(spellBase => spellBase.Spell.IsReady()).Sum(spellBase => Player.Instance.CalculateDamageOnUnit(enemy, spellBase.DamageType, spellBase.SpellDamageValue[spellBase.Spell.Level] + spellBase.SpellDamageModifier[spellBase.Spell.Level] * (spellBase.DamageType == DamageType.Magical ? Player.Instance.FlatMagicDamageMod : Player.Instance.FlatPhysicalDamageMod)));
        }

        public static bool Killable(Obj_AI_Base enemy, SpellSlot slot, float damageDecrease = 0)
        {
            var spellBase = SpellsDamage.Find(s => s.Spell.Slot == slot);
            var possibleDamage = Player.Instance.CalculateDamageOnUnit(enemy, spellBase.DamageType,
                spellBase.SpellDamageValue[spellBase.Spell.Level] +
                spellBase.SpellDamageModifier[spellBase.Spell.Level]*
                (spellBase.DamageType == DamageType.Magical
                    ? Player.Instance.FlatMagicDamageMod
                    : Player.Instance.FlatPhysicalDamageMod));

            return enemy.Health < (possibleDamage - damageDecrease);
        }

        public static float GetSpellDamage(SpellSlot slot, Obj_AI_Base enemy, bool IsAbility = true, bool IsAAOrTargetted = false)
        {
            float damage = new float();

            foreach (var dmg in SpellsDamage.Where(it => it.Spell.Slot == slot))
            {
                if (dmg.DamageType == DamageType.Magical)
                {
                    damage += Player.Instance.CalculateDamageOnUnit(enemy, dmg.DamageType, dmg.SpellDamageValue[dmg.Spell.Level] + dmg.SpellDamageModifier[dmg.Spell.Level] * Player.Instance.FlatMagicDamageMod, IsAbility, IsAAOrTargetted);
                    continue;
                }

                if (dmg.DamageType == DamageType.Physical)
                {
                    damage += Player.Instance.CalculateDamageOnUnit(enemy, dmg.DamageType, dmg.SpellDamageValue[dmg.Spell.Level] + dmg.SpellDamageModifier[dmg.Spell.Level] * Player.Instance.FlatPhysicalDamageMod, IsAbility, IsAAOrTargetted);
                    continue;
                }

                if (dmg.DamageType == DamageType.True)
                {
                    damage += Player.Instance.CalculateDamageOnUnit(enemy, dmg.DamageType, dmg.SpellDamageValue[dmg.Spell.Level] + dmg.SpellDamageModifier[dmg.Spell.Level] * Player.Instance.FlatPhysicalDamageMod, IsAbility, IsAAOrTargetted);
                    continue;
                }
            }

            return damage;
        }
    }
}
