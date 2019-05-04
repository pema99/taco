using System;
using System.Collections.Generic;
using System.Text;

namespace TacoCompiler
{
    public class Binary
    {
        public Variant[] Instructions { get; private set; }
        public int EntryPoint { get; private set; }
        public byte[] Heap { get; private set; }

        public Binary(Variant[] instructions, int entryPoint, byte[] heap)
        {
            this.Instructions = instructions;
            this.EntryPoint = entryPoint;
            this.Heap = heap;
        }
    }
}
