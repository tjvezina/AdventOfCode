using System;

namespace AdventOfCode.Year2019.Day23
{
    public interface INetworkDevice
    {
        event Action<int, Packet> OnSend;
        void Receive(Packet packet);
    }
}
