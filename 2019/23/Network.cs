using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day23 {
    public class Network {
        private const int COMPUTER_COUNT = 50;

        public event Action<long> OnNATPacketSent;

        private NIC[] _computers = new NIC[COMPUTER_COUNT];
        private NAT _nat;

        public Queue<(int address, Packet packet)> sendQueue = new Queue<(int address, Packet)>();

        public Network(string intCodeMemory) {
            for (int i = 0; i < _computers.Length; ++i) {
                _computers[i] = new NIC(intCodeMemory);
                _computers[i].OnSend += HandleOnSend;
            }
        }

        public void Init(bool createNAT = false) {
            for (int i = 0; i < _computers.Length; ++i) {
                _computers[i].Init(i);
            }

            if (createNAT) {
                _nat = new NAT();
                _nat.OnSend += HandleNATOnSend;
            }
        }

        public void Flush() {
            while (sendQueue.Count > 0) {
                (int address, Packet packet) = sendQueue.Dequeue();
                if (address == 255) {
                    _nat.Receive(packet);
                } else {
                    _computers[address].Receive(packet);
                }
            }
        }

        public void Update() {
            for (int i = 0; i < _computers.Length; ++i) {
                _computers[i].Update();
            }

            _nat?.Update(sendQueue.Count);
        }

        private void HandleOnSend(int address, Packet packet) {
            sendQueue.Enqueue((address, packet));
        }

        private void HandleNATOnSend(int address, Packet packet) {
            HandleOnSend(address, packet);
            OnNATPacketSent?.Invoke(packet.y);
        }
    }
}
