using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day22 {
    public enum DamageType { Physical, Magical }

    public abstract class Actor<T> : IDeepCloneable<T> where T : Actor<T> {
        public abstract int startHitPoints { get; }

        public int hitPoints { get; protected set; }
        public abstract int armor { get; }

        public bool isDead => hitPoints == 0;

        protected Actor() {
            hitPoints = startHitPoints;
        }

        protected Actor(Actor<T> toCopy) {
            hitPoints = toCopy.hitPoints;
        }

        public abstract T DeepClone();

        public virtual void HandleTurnEnd() { }

        public void TakeHit(DamageType type, int amount) {
            Debug.Assert(amount >= 0, $"Cannot take negative damage: {amount}");
            if (type == DamageType.Physical) {
                amount = Math.Max(1, amount - armor);
            }
        
            hitPoints = Math.Max(0, hitPoints - amount);
        }

        public void Heal(int amount) {
            Debug.Assert(amount >= 0, $"Cannot heal negative hitPoints: {amount}");
            hitPoints += amount;
        }
    }

    public class Player : Actor<Player> {
        public const int StartMana = 500;

        public override int startHitPoints => 50;
        public override int armor => _temporaryArmor; 
        public int mana { get; private set; } = StartMana;

        private int _temporaryArmor;

        public Player() { }
        private Player(Player toCopy) : base(toCopy) {
            mana = toCopy.mana;
        }

        public override Player DeepClone() => new Player(this);

        public void ConsumeMana(int amount) {
            Debug.Assert(amount >= 0, $"Cannot consume negative mana: {amount}");
            Debug.Assert(amount <= mana, $"Insufficient mana: {mana}/{amount}");
            mana -= amount;
        }

        public void RestoreMana(int amount) {
            Debug.Assert(amount >= 0, "Cannot restore negative mana");
            mana += amount;
        }

        public void ApplyTemporaryArmor(int armor) {
            Debug.Assert(armor >= 0, "Cannot apply negative armor");
            _temporaryArmor += armor;
        }

        public override void HandleTurnEnd() {
            _temporaryArmor = 0;
        }
    }

    public class Boss : Actor<Boss> {
        public override int startHitPoints => 71;
        public override int armor => 999;
        public int damage => 10;

        public Boss() { }
        private Boss(Boss toCopy) : base(toCopy) { }

        public override Boss DeepClone() => new Boss(this);
    }
}
