using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyJSON;

namespace AdventOfCode.Year2015.Day21 {
    public class Challenge : BaseChallenge {
        private Equipment[] _allRings;
        private Equipment[] _allWeapons;
        private Equipment[] _allArmor;

        public override void InitPart1() {
            JSON.Decode(LoadFile("equipment.txt"), out List<Equipment> equipment);

            _allRings   = equipment.Where(e => e.type == Equipment.Type.Ring).ToArray();
            _allWeapons = equipment.Where(e => e.type == Equipment.Type.Weapon).ToArray();
            _allArmor   = equipment.Where(e => e.type == Equipment.Type.Armor).ToArray();
        }

        public override string part1Answer => "121";
        public override (string, object) SolvePart1() {
            Equipment[] bestEquipment = FindBestEquipment(playerWins:true, leastGold:true);

            string equipmentStr = bestEquipment.Select(e => e.name).Aggregate((a, b) => $"{a}, {b}");

            return ($"Won by spending {{0}}g on {equipmentStr}", bestEquipment.Sum(e => e.cost));
        }
        
        public override string part2Answer => "201";
        public override (string, object) SolvePart2() {
            Equipment[] bestEquipment = FindBestEquipment(playerWins:false, leastGold:false);

            string equipmentStr = bestEquipment.Select(e => e.name).Aggregate((a, b) => $"{a}, {b}");

            return ($"Lost by spending {{0}}g on {equipmentStr}", bestEquipment.Sum(e => e.cost));
        }

        private Equipment[] FindBestEquipment(bool playerWins, bool leastGold) {
            Boss boss = new Boss();

            int bestCost = (leastGold ? int.MaxValue : 0);
            Equipment[] bestEquipment = null;

            foreach (Equipment[] rings in DataUtil.GetAllCombinations(_allRings, new Range(0, 2))) {
                for (int w = 0; w < _allWeapons.Length; ++w) {
                    for (int a = -1; a < _allArmor.Length; ++a) {
                        IEnumerable<Equipment> equipment = rings.Append(_allWeapons[w]);
                        if (a > -1) equipment = equipment.Append(_allArmor[a]);
                        Player player = new Player(equipment);

                        bool desiredWinner = (playerWins == (player.TurnsToDefeat(boss) <= boss.TurnsToDefeat(player)));
                        bool betterCost = (leastGold ? player.equipmentCost < bestCost : player.equipmentCost > bestCost);

                        if (desiredWinner && betterCost) {
                            bestCost = player.equipmentCost;
                            bestEquipment = equipment.ToArray();
                        }
                    }
                }
            }

            Debug.Assert(bestEquipment != null, "Failed to find any equipment set to meet conditions!");

            return bestEquipment;
        }
    }
}
