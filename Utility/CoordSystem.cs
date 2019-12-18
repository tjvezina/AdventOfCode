using System;

namespace AdventOfCode {
    public enum CoordSystem {
        YUp,  // Positive X = right, positive Y = up
        YDown // Positive X = right, positive Y = down
    }

    public static class SpaceUtil {
        private static CoordSystem? _system;
        public static CoordSystem system {
            private get {
                if (_system == null) {
                    throw new Exception($"{nameof(CoordSystem)} must be set before using {nameof(SpaceUtil)} functions");
                }
                return _system.Value;
            }
            set => _system = value;
        }

        public static Point ToPoint(this Direction dir) {
            Point p = Point.zero;
            switch (dir) {
                case Direction.Right: p = new Point( 1, 0); break;
                case Direction.Left:  p = new Point(-1, 0); break;
                case Direction.Up:    p = new Point( 0, 1); break;
                case Direction.Down:  p = new Point( 0,-1); break;
            }

            if (system == CoordSystem.YDown) p.y *= -1;

            return p;
        }

        public static Direction ToDirection(this Point p) {
            if (system == CoordSystem.YDown) p.y *= -1;

            if (p == new Point( 1, 0)) return Direction.Right;
            if (p == new Point(-1, 0)) return Direction.Left;
            if (p == new Point( 0, 1)) return Direction.Up;
            if (p == new Point( 0,-1)) return Direction.Down;

            throw new Exception($"Failed to convert {p} to Direction, must be unit vector.");
        }

        public static Point Rotate90(this Point p) => new Point(-p.y, p.x);
        public static Point Rotate270(this Point p) => new Point(p.y, -p.x);

        public static Point RotateCW(this Point p) => (system == CoordSystem.YUp ? Rotate270(p) : Rotate90(p));
        public static Point RotateCCW(this Point p) => (system == CoordSystem.YUp ? Rotate90(p) : Rotate270(p));
    }
}
