using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day22
{
    public class CombatState : IDeepCloneable<CombatState>
    {
        public enum Result
        {
            InProgress,
            InsufficientMana,
            EffectAlreadyActive,
            PlayerKilled,
            Victory
        }

        public Player player { get; }
        public Boss boss { get; }

        public Result result { get; private set; }
        public int manaSpent { get; private set; }

        public IEnumerable<Spell> spellsCast => _spellsCast.AsReadOnly();

        private readonly Difficulty _difficulty;
        private readonly List<(Spell spell, int turnsLeft)> _activeSpells;
        private readonly List<Spell> _spellsCast;

        public CombatState(Difficulty difficulty)
        {
            _difficulty = difficulty;
            result = Result.InProgress;

            player = new Player();
            boss = new Boss();

            _activeSpells = new List<(Spell, int)>();
            _spellsCast = new List<Spell>();
        }

        private CombatState(CombatState toCopy)
        {
            _difficulty = toCopy._difficulty;
            result = toCopy.result;

            player = toCopy.player.DeepClone();
            boss = toCopy.boss.DeepClone();

            _activeSpells = new List<(Spell, int)>(toCopy._activeSpells);
            _spellsCast = new List<Spell>(toCopy._spellsCast);
            manaSpent = toCopy.manaSpent;
        }

        public CombatState DeepClone() => new CombatState(this);

        public void CastSpell(Spell spellToCast)
        {
            bool CheckForGameOver()
            {
                if (player.isDead)
                {
                    result = Result.PlayerKilled;
                    return true;
                }
                if (boss.isDead)
                {
                    result = Result.Victory;
                    return true;
                }
                return false;
            }

            _spellsCast.Add(spellToCast);
            manaSpent += spellToCast.manaCost;

            // --- PLAYER TURN ---
            if (_difficulty == Difficulty.Hard)
            {
                player.TakeHit(DamageType.Physical, 1);
                if (CheckForGameOver()) return;
            }

            ApplyActiveEffects();
            if (CheckForGameOver()) return;

            if (player.mana < spellToCast.manaCost)
            {
                result = Result.InsufficientMana;
                return;
            }

            if (spellToCast.duration == 0)
            {
                spellToCast.Apply(this);
                if (CheckForGameOver()) return;
            } else
            {
                // Unable to cast an already active effect
                if (_activeSpells.Any(a => a.spell == spellToCast))
                {
                    result = Result.EffectAlreadyActive;
                    return;
                }

                _activeSpells.Add((spellToCast, spellToCast.duration));
            }
            player.ConsumeMana(spellToCast.manaCost);
            player.HandleTurnEnd();

            // --- BOSS TURN ---
            ApplyActiveEffects();
            if (CheckForGameOver()) return;

            player.TakeHit(DamageType.Physical, boss.damage);
            if (CheckForGameOver()) return;

            player.HandleTurnEnd();
        }

        private void ApplyActiveEffects()
        {
            for (int i = _activeSpells.Count - 1; i >= 0; i--)
            {
                (Spell spell, int turnsLeft) = _activeSpells[i];

                spell.Apply(this);

                if (--turnsLeft == 0)
                {
                    _activeSpells.RemoveAt(i);
                } else
                {
                    _activeSpells[i] = (spell, turnsLeft);
                }
            }
        }
    }
}
