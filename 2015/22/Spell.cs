using System.Collections.Generic;

namespace AdventOfCode.Year2015.Day22
{
    public class Spell
    {
        public string name { get; }
        public int manaCost { get; }
        public int duration { get; }

        public readonly ICollection<ISpellEffect> effects;

        public Spell(string name, int manaCost, params ISpellEffect[] effects)
            : this(name, manaCost, duration:0, effects) { }

        public Spell(string name, int manaCost, int duration, params ISpellEffect[] effects)
        {
            this.name = name;
            this.manaCost = manaCost;
            this.duration = duration;
            this.effects = new List<ISpellEffect>(effects);
        }

        public void Apply(CombatState state)
        {
            foreach (ISpellEffect effect in effects)
            {
                effect.Apply(state);
            }
        }
    }

    public interface ISpellEffect
    {
        void Apply(CombatState state);
    }

    public class DamageEffect : ISpellEffect
    {
        public int damage { get; }

        public DamageEffect(int damage) => this.damage = damage;

        public void Apply(CombatState state)
        {
            state.boss.TakeHit(DamageType.Magical, damage);
        }
    }

    public class HealEffect : ISpellEffect
    {
        public int healing { get; }

        public HealEffect(int healing) => this.healing = healing;

        public void Apply(CombatState state)
        {
            state.player.Heal(healing);
        }
    }

    public class ArmorEffect : ISpellEffect
    {
        public int armor { get; }

        public ArmorEffect(int armor) => this.armor = armor;

        public void Apply(CombatState state)
        {
            state.player.ApplyTemporaryArmor(armor);
        }
    }

    public class ManaRestoreEffect : ISpellEffect
    {
        public int mana { get; }

        public ManaRestoreEffect(int mana) => this.mana = mana;

        public void Apply(CombatState state)
        {
            state.player.RestoreMana(mana);
        }
    }
}
