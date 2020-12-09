using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day21
{
    public abstract class Actor
    {
        protected abstract int hitPoints { get; }

        protected abstract int damage { get; }
        protected abstract int armor { get; }

        public int TurnsToDefeat(Actor opponent)
        {
            int damagePerTurn = Math.Max(1, damage - opponent.armor);
            return (opponent.hitPoints - 1) / damagePerTurn + 1;
        }
    }

    public class Boss : Actor
    {
        protected override int hitPoints => 103;

        protected override int damage => 9;
        protected override int armor => 2;
    }

    public class Player : Actor
    {
        protected override int hitPoints => 100;

        protected override int damage { get; }
        protected override int armor { get; }

        public int equipmentCost { get; }

        public Player(IReadOnlyList<Equipment> equipment)
        {
            damage = equipment.Sum(e => e.damage);
            armor = equipment.Sum(e => e.armor);
            equipmentCost = equipment.Sum(e => e.cost);
        }
    }
}
