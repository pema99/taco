using System;
using System.Collections.Generic;
using System.Text;

namespace BadLang
{
    public class UnlinkedAST
    {
        public Variant[] Instructions { get; private set; }
        public Dictionary<string, int> Symbols { get; private set; }
        public Dictionary<string, int> Identifiers { get; private set; }
        public byte[] Heap { get; private set; }

        public UnlinkedAST(Variant[] instructions, Dictionary<string, int> symbols, Dictionary<string, int> identifiers, byte[] heap)
        {
            this.Instructions = instructions;
            this.Symbols = symbols;
            this.Identifiers = identifiers;
            this.Heap = heap;
        }
    }
}
