using System;

namespace AdventOfCode {
    public enum CoordSystem {
        YUp,  // X+ = right, Y+ = up
        YDown // X+ = right, Y+ = down
    }

    public static class CoordUtil {
        public static CoordSystem? system;
        private static CoordSystem _system {
            get {
                if (system == null) {
                    throw new Exception($"{nameof(CoordSystem)} must be set before using {nameof(CoordUtil)} functions");
                }
                return system.Value;
            }
        }

        public static Point ToPoint(this Direction dir) {
            Point p = Point.zero;
            switch (dir) {
                case Direction.Right: p = new Point( 1, 0); break;
                case Direction.Left:  p = new Point(-1, 0); break;
                case Direction.Up:    p = new Point( 0, 1); break;
                case Direction.Down:  p = new Point( 0,-1); break;
            }

            if (_system == CoordSystem.YDown) p.y *= -1;

            return p;
        }

        public static Direction ToDirection(this Point p) {
            if (_system == CoordSystem.YDown) p.y *= -1;

            if (p == new Point( 1, 0)) return Direction.Right;
            if (p == new Point(-1, 0)) return Direction.Left;
            if (p == new Point( 0, 1)) return Direction.Up;
            if (p == new Point( 0,-1)) return Direction.Down;

            throw new Exception($"Failed to convert {p} to Direction, must be unit vector.");
        }

        public static Point Rotate90(this Point p) => new Point(-p.y, p.x);
        public static Point Rotate270(this Point p) => new Point(p.y, -p.x);

        public static Point RotateCW(this Point p) => (_system == CoordSystem.YUp ? Rotate270(p) : Rotate90(p));
        public static Point RotateCCW(this Point p) => (_system == CoordSystem.YUp ? Rotate90(p) : Rotate270(p));
    }
}
