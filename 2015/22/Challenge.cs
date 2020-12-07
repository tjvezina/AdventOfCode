using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day22
{
    public enum Difficulty { Easy, Hard }

    public class Challenge : BaseChallenge
    {
        public static readonly ImmutableList<Spell> Spells = ImmutableList.Create(new[]
        {
            new Spell("Magic Missile", manaCost:53, new DamageEffect(4)),
            new Spell("Drain",         manaCost:73, new DamageEffect(2), new HealEffect(2)),
            new Spell("Shield",        manaCost:113, duration:6, new ArmorEffect(7)),
            new Spell("Poison",        manaCost:173, duration:6, new DamageEffect(3)),
            new Spell("Recharge",      manaCost:229, duration:5, new ManaRestoreEffect(101))
        });

        public override object part1ExpectedAnswer => 1824;
        public override (string message, object answer) SolvePart1()
        {
            Play(Difficulty.Easy, out int leastMana);
            return ("Least-mana victory: ", leastMana);
        }
        
        public override object part2ExpectedAnswer => 1937;
        public override (string message, object answer) SolvePart2()
        {
            Play(Difficulty.Hard, out int leastMana);
            return ("Least-mana victory: ", leastMana);
        }

        private void Play(Difficulty difficulty, out int leastMana)
        {
            CombatState victoryState = null;

            Comparer<CombatState> stateComparer = Comparer<CombatState>.Create((a, b) => a.manaSpent.CompareTo(b.manaSpent));
            BinaryMinHeap<CombatState> activeStates = new BinaryMinHeap<CombatState>(stateComparer);
            activeStates.Insert(new CombatState(difficulty));

            while (activeStates.Count > 0)
            {
                CombatState prevState = activeStates.Extract();

                if (victoryState != null && victoryState.manaSpent <= prevState.manaSpent) break;

                foreach (Spell spell in Spells)
                {
                    if (victoryState != null && victoryState.manaSpent <= prevState.manaSpent + spell.manaCost)
                    {
                        continue;
                    }

                    CombatState nextState = prevState.DeepClone();
                    nextState.CastSpell(spell);

                    switch (nextState.result)
                    {
                        case CombatState.Result.InProgress:
                            activeStates.Insert(nextState);
                            break;
                        case CombatState.Result.Victory:
                            if (victoryState == null || victoryState.manaSpent > nextState.manaSpent)
                            {
                                victoryState = nextState;
                            }
                            break;
                    }
                }
            }

            Debug.Assert(victoryState != null, "Failed to find any sequence of spells to defeat the boss!");

            Console.WriteLine($" -- Difficulty {difficulty} -- ");

            CombatState state = new CombatState(difficulty);
            foreach (Spell spell in victoryState.spellsCast)
            {
                state.CastSpell(spell);
                Console.Write($"{spell.name,-15}");
                Console.Write($"Boss: {state.boss.hitPoints,2}  ");
                Console.Write($"Player: {state.player.hitPoints,2}  ");
                Console.Write($"Mana: {state.player.mana,3}");
                Console.WriteLine();
            }

            leastMana = victoryState.spellsCast.Sum(s => s.manaCost);
        }
    }
}
