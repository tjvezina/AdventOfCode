using System;

namespace AdventOfCode
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CoordSystemAttribute : Attribute
    {
        public CoordSystem coordSystem { get; }

        public CoordSystemAttribute(CoordSystem coordSystem) => this.coordSystem = coordSystem;
    }
}
