using System;
using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day23
{
    /// <summary>
    /// Network Interface Computer, runs IntCode and sends/receives information packets.
    /// </summary>
    public class NIC : INetworkDevice
    {
        public event Action<int, Packet> OnSend;

        public bool isActive => _intCode.state != IntCode.State.Complete;
        public int inputQueueCount => _inputQueue.Count;

        private IntCode _intCode;

        private Queue<Packet> _inputQueue = new Queue<Packet>();
        private Queue<long> _outputQueue = new Queue<long>();

        public NIC(string intCodeMemory)
        {
            _intCode = new IntCode(intCodeMemory);
            _intCode.OnOutput += HandleOutput;
            _intCode.Begin();
        }

        public void Init(int address)
        {
            _intCode.Input(address);
        }

        public void Update()
        {
            if (_intCode.state == IntCode.State.Complete) return;

            if (_inputQueue.Count > 0)
            {
                while (_inputQueue.Count > 0)
                {
                    Packet packet = _inputQueue.Dequeue();
                    _intCode.Input(packet.x);
                    _intCode.Input(packet.y);
                }
            }
            else
            {
                _intCode.Input(-1);
            }
        }

        private void HandleOutput(long output)
        {
            _outputQueue.Enqueue(output);

            if (_outputQueue.Count == 3)
            {
                OnSend?.Invoke((int)_outputQueue.Dequeue(), new Packet(_outputQueue.Dequeue(), _outputQueue.Dequeue()));
            }
        }

        public void Receive(Packet packet) => _inputQueue.Enqueue(packet);
    }
}
