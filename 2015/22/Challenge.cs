using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day22 {
    public enum Difficulty { Easy, Hard }

    public class Challenge : BaseChallenge {
        public static readonly ImmutableList<Spell> Spells = ImmutableList.Create(new[] {
            new Spell("Magic Missile", manaCost:53, new DamageEffect(4)),
            new Spell("Drain",         manaCost:73, new DamageEffect(2), new HealEffect(2)),
            new Spell("Shield",        manaCost:113, duration:6, new ArmorEffect(7)),
            new Spell("Poison",        manaCost:173, duration:6, new DamageEffect(3)),
            new Spell("Recharge",      manaCost:229, duration:5, new ManaRestoreEffect(101))
        });

        public override void InitPart1() { }

        public override string part1Answer => "1824";
        public override (string, object) SolvePart1() {
            Play(Difficulty.Easy, out int leastMana);
            return ("Least-mana victory: ", leastMana);
        }
        
        public override string part2Answer => "1937";
        public override (string, object) SolvePart2() {
            Play(Difficulty.Hard, out int leastMana);
            return ("Least-mana victory: ", leastMana);
        }

        private void Play(Difficulty difficulty, out int leastMana) {
            List<CombatState> victoryStates = new List<CombatState>();

            Queue<CombatState> stateQueue = new Queue<CombatState>();
            stateQueue.Enqueue(new CombatState(difficulty));

            while (stateQueue.TryDequeue(out CombatState prevState)) {
                foreach (Spell spell in Spells) {
                    Profiler.Start("DeepClone");
                    CombatState nextState = prevState.DeepClone();
                    Profiler.Stop();
                    Profiler.Start("CastSpell");
                    nextState.CastSpell(spell);
                    Profiler.Stop();
                    Profiler.Start("Sort");
                    switch (nextState.result) {
                        case CombatState.Result.InProgress:
                            stateQueue.Enqueue(nextState);
                            break;
                        case CombatState.Result.Victory:
                            victoryStates.Add(nextState);
                            break;
                    }
                    Profiler.Stop();
                }
            }
            Profiler.PrintResults();

            Debug.Assert(victoryStates.Count > 0, "Failed to find any sequence of spells to defeat the boss!");

            Console.WriteLine($" -- Difficulty {difficulty} -- ");
            Console.WriteLine($"Victory states: {victoryStates.Count}");
            IOrderedEnumerable<CombatState> victoryBySpellsCast = victoryStates.OrderBy(s => s.spellsCast.Count);
            Console.WriteLine($"Least spells to victory: {victoryBySpellsCast.First().spellsCast.Count}");
            Console.WriteLine($"Most spells to victory: {victoryBySpellsCast.Last().spellsCast.Count}");

            CombatState leastManaVictory = victoryStates.OrderBy(s => s.spellsCast.Sum(s => s.manaCost)).First();

            Console.WriteLine("Least mana victory:");
            CombatState state = new CombatState(difficulty);
            foreach (Spell spell in leastManaVictory.spellsCast) {
                state.CastSpell(spell);
                Console.Write($"{spell.name,-15}");
                Console.Write($"Boss: {state.boss.hitPoints,2}  ");
                Console.Write($"Player: {state.player.hitPoints,2}  ");
                Console.Write($"Mana: {state.player.mana,3}");
                Console.WriteLine();
            }

            leastMana = leastManaVictory.spellsCast.Sum(s => s.manaCost);
        }
    }
}
