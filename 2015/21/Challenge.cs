using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TinyJSON;

namespace AdventOfCode.Year2015.Day21
{
    public class Challenge : BaseChallenge
    {
        private readonly Equipment[] _allRings;
        private readonly Equipment[] _allWeapons;
        private readonly Equipment[] _allArmor;

        public Challenge()
        {
            JSON.Decode(LoadFile("equipment.txt"), out List<Equipment> equipment);

            _allRings   = equipment.Where(e => e.type == Equipment.Type.Ring).ToArray();
            _allWeapons = equipment.Where(e => e.type == Equipment.Type.Weapon).ToArray();
            _allArmor   = equipment.Where(e => e.type == Equipment.Type.Armor).ToArray();
        }

        public override object part1ExpectedAnswer => 121;
        public override (string message, object answer) SolvePart1()
        {
            Equipment[] bestEquipment = FindBestEquipment(playerWins:true, leastGold:true);

            string equipmentStr = bestEquipment.Select(e => e.name).Aggregate((a, b) => $"{a}, {b}");

            return ($"Won by spending {{0}}g on {equipmentStr}", bestEquipment.Sum(e => e.cost));
        }
        
        public override object part2ExpectedAnswer => 201;
        public override (string message, object answer) SolvePart2()
        {
            Equipment[] bestEquipment = FindBestEquipment(playerWins:false, leastGold:false);

            string equipmentStr = bestEquipment.Select(e => e.name).Aggregate((a, b) => $"{a}, {b}");

            return ($"Lost by spending {{0}}g on {equipmentStr}", bestEquipment.Sum(e => e.cost));
        }

        private Equipment[] FindBestEquipment(bool playerWins, bool leastGold)
        {
            Boss boss = new Boss();

            int bestCost = (leastGold ? int.MaxValue : 0);
            Equipment[] bestEquipment = null;

            foreach (Equipment[] rings in DataUtil.GetAllCombinations(_allRings, new Range(0, 2)))
            {
                for (int w = 0; w < _allWeapons.Length; w++)
                {
                    for (int a = -1; a < _allArmor.Length; a++)
                    {
                        IEnumerable<Equipment> equipment = rings.Append(_allWeapons[w]);
                        if (a > -1) equipment = equipment.Append(_allArmor[a]);
                        Player player = new Player(equipment);

                        bool desiredWinner = (playerWins == (player.TurnsToDefeat(boss) <= boss.TurnsToDefeat(player)));
                        bool betterCost = (leastGold ? player.equipmentCost < bestCost : player.equipmentCost > bestCost);

                        if (desiredWinner && betterCost)
                        {
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
