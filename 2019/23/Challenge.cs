using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day23 {
    public class Challenge : BaseChallenge {
        public override string part1ExpectedAnswer => "24602";
        public override (string message, object answer) SolvePart1() {
            Network network = new Network(inputList[0]);
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

            return ($"First packet for address 255: ({packet.x}, {{0}})", packet.y);
        }
        
        public override string part2ExpectedAnswer => "19641";
        public override (string message, object answer) SolvePart2() {
            HashSet<long> natPackets = new HashSet<long>();

            long? duplicatePacket = null;

            void HandleOnNATPacketSent(long data) {
                if (!natPackets.Add(data)) {
                    duplicatePacket = data;
                }
            }

            Network network = new Network(inputList[0]);
            network.OnNATPacketSent += HandleOnNATPacketSent;
            network.Init(createNAT:true);

            while (duplicatePacket == null) {
                network.Flush();
                network.Update();
            }

            return ("First duplicate NAT packet sent: ", duplicatePacket.Value);
        }
    }
}
