using System.Diagnostics.CodeAnalysis;
using TinyJSON;

namespace AdventOfCode.Year2015.Day21
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local", Justification = "Property setters used by TinyJSON")]
    public struct Equipment
    {
        public enum Type
        {
            Weapon,
            Armor,
            Ring
        }

        [Include] public Type type { get; private set; }
        [Include] public string name { get; private set; }
        [Include] public int cost { get; private set; }
        [Include] public int damage { get; private set; }
        [Include] public int armor { get; private set; }
    }
}
