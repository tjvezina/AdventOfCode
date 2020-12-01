using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public Difficulty difficulty;
        public Result result;
        public Player player;
        public Boss boss;
        public IList<(Spell spell, int turnsLeft)> activeSpells;
        public IList<Spell> spellsCast;
        public int manaSpent;

        public CombatState(Difficulty difficulty)
        {
            this.difficulty = difficulty;
            result = Result.InProgress;
            player = new Player();
            boss = new Boss();
            activeSpells = new List<(Spell, int)>();
            spellsCast = new List<Spell>();
        }

        private CombatState(CombatState toCopy)
        {
            difficulty = toCopy.difficulty;
            result = toCopy.result;
            player = toCopy.player.DeepClone();
            boss = toCopy.boss.DeepClone();
            activeSpells = toCopy.activeSpells.ToList();
            spellsCast = toCopy.spellsCast.ToList();
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

            spellsCast.Add(spellToCast);
            manaSpent += spellToCast.manaCost;

            // --- PLAYER TURN ---
            if (difficulty == Difficulty.Hard)
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
                if (activeSpells.Any(a => a.spell == spellToCast))
                {
                    result = Result.EffectAlreadyActive;
                    return;
                }

                activeSpells.Add((spellToCast, spellToCast.duration));
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
            for (int i = activeSpells.Count - 1; i >= 0; i--)
            {
                (Spell spell, int turnsLeft) = activeSpells[i];

                spell.Apply(this);

                if (--turnsLeft == 0)
                {
                    activeSpells.RemoveAt(i);
                } else
                {
                    activeSpells[i] = (spell, turnsLeft);
                }
            }
        }
    }
}
