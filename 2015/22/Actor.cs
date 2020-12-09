using System;
using System.Diagnostics;

namespace AdventOfCode.Year2015.Day22
{
    public enum DamageType { Physical, Magical }

    public abstract class Actor<T> : IDeepCloneable<T> where T : Actor<T>
    {
        public int hitPoints { get; private set; }
        protected abstract int armor { get; }

        public bool isDead => hitPoints == 0;

        protected Actor(int startHitPoints)
        {
            hitPoints = startHitPoints;
        }

        protected Actor(Actor<T> toCopy)
        {
            hitPoints = toCopy.hitPoints;
        }

        public abstract T DeepClone();

        public void TakeHit(DamageType type, int amount)
        {
            Debug.Assert(amount >= 0, $"Cannot take negative damage: {amount}");
            if (type == DamageType.Physical)
            {
                amount = Math.Max(1, amount - armor);
            }
        
            hitPoints = Math.Max(0, hitPoints - amount);
        }

        public void Heal(int amount)
        {
            Debug.Assert(amount >= 0, $"Cannot heal negative hitPoints: {amount}");
            hitPoints += amount;
        }
    }

    public class Player : Actor<Player>
    {
        private const int StartMana = 500;

        protected override int armor => _temporaryArmor;
        public int mana { get; private set; } = StartMana;

        private int _temporaryArmor;

        public Player() : base(startHitPoints: 50) { }
        private Player(Player toCopy) : base(toCopy)
        {
            mana = toCopy.mana;
        }

        public override Player DeepClone() => new Player(this);

        public void ConsumeMana(int amount)
        {
            Debug.Assert(amount >= 0, $"Cannot consume negative mana: {amount}");
            Debug.Assert(amount <= mana, $"Insufficient mana: {mana}/{amount}");
            mana -= amount;
        }

        public void RestoreMana(int amount)
        {
            Debug.Assert(amount >= 0, "Cannot restore negative mana");
            mana += amount;
        }

        public void ApplyTemporaryArmor(int temporaryArmor)
        {
            Debug.Assert(temporaryArmor >= 0, "Cannot apply negative armor");
            _temporaryArmor += temporaryArmor;
        }

        public void HandleTurnEnd()
        {
            _temporaryArmor = 0;
        }
    }

    public class Boss : Actor<Boss>
    {
        protected override int armor => 999;
        public int damage => 10;

        public Boss() : base(startHitPoints: 71) { }
        private Boss(Boss toCopy) : base(toCopy) { }

        public override Boss DeepClone() => new Boss(this);
    }
}
