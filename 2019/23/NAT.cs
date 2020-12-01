using System;
using System.Linq;

namespace AdventOfCode.Year2019.Day23
{
    /// <summary>
    /// Not Always Transmitting, runs int code and blocks certain packets in the network.
    /// </summary>
    public class NAT : INetworkDevice
    {
        public event Action<int, Packet> OnSend;

        private Packet _memory;

        public void Update(int pendingPackets)
        {
            if (pendingPackets == 0)
            {
                OnSend?.Invoke(0, _memory);
            }
        }

        public void Receive(Packet packet) => _memory = packet;
    }
}
