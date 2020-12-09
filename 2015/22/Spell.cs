using System.Collections.Generic;

namespace AdventOfCode.Year2015.Day22
{
    public class Spell
    {
        public string name { get; }
        public int manaCost { get; }
        public int duration { get; }

        private readonly ICollection<ISpellEffect> _effects;

        public Spell(string name, int manaCost, params ISpellEffect[] effects)
            : this(name, manaCost, duration:0, effects) { }

        public Spell(string name, int manaCost, int duration, params ISpellEffect[] effects)
        {
            this.name = name;
            this.manaCost = manaCost;
            this.duration = duration;
            _effects = new List<ISpellEffect>(effects);
        }

        public void Apply(CombatState state)
        {
            foreach (ISpellEffect effect in _effects)
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
        private readonly int _damage;

        public DamageEffect(int damage) => _damage = damage;

        public void Apply(CombatState state)
        {
            state.boss.TakeHit(DamageType.Magical, _damage);
        }
    }

    public class HealEffect : ISpellEffect
    {
        private readonly int _healing;

        public HealEffect(int healing) => _healing = healing;

        public void Apply(CombatState state)
        {
            state.player.Heal(_healing);
        }
    }

    public class ArmorEffect : ISpellEffect
    {
        private readonly int _armor;

        public ArmorEffect(int armor) => _armor = armor;

        public void Apply(CombatState state)
        {
            state.player.ApplyTemporaryArmor(_armor);
        }
    }

    public class ManaRestoreEffect : ISpellEffect
    {
        private readonly int _mana;

        public ManaRestoreEffect(int mana) => _mana = mana;

        public void Apply(CombatState state)
        {
            state.player.RestoreMana(_mana);
        }
    }
}
