using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day10 {
     public class Challenge : BaseChallenge {
        private struct Asteroid {
            public Point position;
            public double angle;

            public Asteroid(Point position, double angle) {
                this.position = position;
                this.angle = angle;
            }
        }

        private int?[,] _map;
        private int _width;
        private int _height;

        private Point _stationPos;

        public override void InitPart1() {
            _width = inputSet[0].Length;
            _height = inputSet.Length;
            _map = new int?[_width, _height];
            
            for (int y = 0; y < _height; ++y) {
                string line = inputSet[y];
                for (int x = 0; x < _width; ++x) {
                    if (line[x] == '#') {
                        _map[x, y] = 0;
                    }
                }
            }

            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    if (_map[x, y] != null) {
                        _map[x, y] = CountVisible(new Point(x, y));
                    }
                }
            }
        }

        public override string part1Answer => "282";
        public override (string, object) SolvePart1() {
            int maxVisible = 0;
            Point bestPos = new Point(-1, -1);
            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    if (_map[x, y] != null && _map[x, y].Value > maxVisible) {
                        maxVisible = _map[x, y].Value;
                        bestPos = new Point(x, y);
                    }
                }
            }
            _stationPos = bestPos;
            return ($"Max visible asteroids: {{0}} {_stationPos}", maxVisible);
        }
        
        public override string part2Answer => "1008";
        public override (string, object) SolvePart2() {
            const int ASTEROID_NUM = 200;

            List<Asteroid> asteroids = new List<Asteroid>();
            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    Point pos = new Point(x, y);
                    if (_map[x, y] == null || _stationPos == pos) continue;

                    if (CanSee(_stationPos, pos)) {
                        double angle = GetAngle(pos - _stationPos);
                        asteroids.Add(new Asteroid(pos, angle));
                    }
                }
            }
            
            asteroids.Sort((a, b) => a.angle.CompareTo(b.angle));

            Point outputPos = asteroids[ASTEROID_NUM-1].position;
            return ($"#{ASTEROID_NUM} = {outputPos} -> {{0}}", outputPos.x * 100 + outputPos.y);
        }

        private int CountVisible(Point p1) {
            int visible = 0;
            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    Point p2 = new Point(x, y);
                    if (_map[x, y] == null || p1 == p2) continue;

                    if (CanSee(p1, p2)) ++visible;
                }
            }
            return visible;
        }

        private bool CanSee(Point p1, Point p2) {
            Point d = p2 - p1;
            int gcd = MathUtil.GCD(d.x, d.y);

            Point step = d / gcd;
            Point p3 = p1;
            for (int i = 1; i < gcd; ++i) {
                p3 += step;
                if (_map[p3.x, p3.y] != null) return false;
            }

            return true;
        }

        private double GetAngle(Point p) => (Math.Atan2(-p.x, p.y) + Math.PI) % (2 * Math.PI);
    }
}
