using System;

public class Reindeer {
    private int _speed;
    private int _flyTime;
    private int _restTime;

    public Reindeer(int speed, int flyTime, int restTime) {
        _speed = speed;
        _flyTime = flyTime;
        _restTime = restTime;
    }

    public int GetDistance(int time) {
        int cycleTime = _flyTime + _restTime;
        int distPerCycle = _speed * _flyTime;

        int dist = (time / cycleTime) * distPerCycle;
        
        int remain = time % cycleTime;
        dist += Math.Min(remain, _flyTime) * _speed;

        return dist;
    }
}
