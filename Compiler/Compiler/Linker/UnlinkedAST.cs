using System.Collections.Generic;

namespace TacoCompiler
{
    public class UnlinkedAST
    {
        public Variant[] Instructions { get; private set; }
        public Dictionary<string, int> Symbols { get; private set; }
        public List<(string, int)> Unlinked { get; private set; }
        public byte[] Heap { get; private set; }

        public UnlinkedAST(Variant[] instructions, Dictionary<string, int> symbols, List<(string, int)> unlinked, byte[] heap)
        {
            this.Instructions = instructions;
            this.Symbols = symbols;
            this.Unlinked = unlinked;
            this.Heap = heap;
        }
    }
}
