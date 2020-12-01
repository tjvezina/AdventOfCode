using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    public static class Profiler
    {
        private static Stopwatch _stopwatch = new Stopwatch();

        private static Dictionary<string, List<long>> _resultsMap = new Dictionary<string, List<long>>();
        private static List<long> _activeList;
        
        public static void Start(string eventName)
        {
            Debug.Assert(_activeList == null, "Event already being tracked");
            if (!_resultsMap.ContainsKey(eventName)) _resultsMap[eventName] = new List<long>();

            _activeList = _resultsMap[eventName];
            _stopwatch.Restart();
        }

        public static void Stop()
        {
            _stopwatch.Stop();

            if (_activeList != null)
            {
                _activeList.Add(_stopwatch.ElapsedTicks);
                _activeList = null;
            }
        }

        public static void FlushResults()
        {
            foreach ((string eventName, List<long> results) in _resultsMap)
            {
                int count = results.Count();
                long average = (long)results.Average();
                long min = results.Min();
                long max = results.Max();

                Console.WriteLine($"{eventName,-20} {average,5} [{min,6}, {max,6}] x{count:N0}");
            }

            _stopwatch.Reset();
            _resultsMap.Clear();
            _activeList = null;
        }
    }
}
