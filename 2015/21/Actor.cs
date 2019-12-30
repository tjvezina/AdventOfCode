using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day21 {
    public abstract class Actor {
        public abstract int hitPoints { get; }
        
        public abstract int damage { get; }
        public abstract int armor { get; }

        public int TurnsToDefeat(Actor opponent) {
            int damagePerTurn = Math.Max(1, damage - opponent.armor);
            return (opponent.hitPoints - 1) / damagePerTurn + 1;
        }
    }

    public class Boss : Actor {
        public override int hitPoints => 103;

        public override int damage => 9;
        public override int armor => 2;
    }

    public class Player : Actor {
        public override int hitPoints => 100;

        public override int damage => _equipmentDamage;
        public override int armor => _equipmentArmor;

        public int equipmentCost { get; }

        private List<Equipment> _equipment;
        private int _equipmentDamage;
        private int _equipmentArmor;

        public Player(IEnumerable<Equipment> equipment) {
            _equipment = new List<Equipment>(equipment);

            _equipmentDamage = _equipment.Sum(e => e.damage);
            _equipmentArmor = _equipment.Sum(e => e.armor);
            equipmentCost = _equipment.Sum(e => e.cost);
        }
    }
}
