using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day23 {
    public class Challenge : BaseChallenge {
        public override string SolvePart1() {
            Network network = new Network(input);
            network.Init();

            Packet packet;
            while (true) {
                network.Flush();
                network.Update();

                packet = network.sendQueue.FirstOrDefault(p => p.address == 255).packet;
                if (packet != null) {
                    break;
                }
            }

            return $"First packet for address 255: ({packet.x}, {packet.y})";
        }
        
        public override string SolvePart2() {
            HashSet<long> natPackets = new HashSet<long>();

            long? duplicatePacket = null;

            void HandleOnNATPacketSent(long data) {
                if (!natPackets.Add(data)) {
                    duplicatePacket = data;
                }
            }

            Network network = new Network(input);
            network.OnNATPacketSent += HandleOnNATPacketSent;
            network.Init(createNAT:true);

            while (duplicatePacket == null) {
                network.Flush();
                network.Update();
            }

            return $"First duplicate NAT packet sent: {duplicatePacket.Value}";
        }
    }
}
