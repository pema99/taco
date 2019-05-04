using System;
using System.Collections.Generic;

namespace TacoCompiler
{
    public class Scope
    {
        public Scope Parent { get; private set; }
        public Dictionary<string, int> Symbols { get; private set; }
        public int StackPtr { get; private set; }

        public Scope(Scope parent, Dictionary<string, int> symbols, int stackPtr)
        {
            this.Parent = parent;
            this.Symbols = symbols;
            this.StackPtr = stackPtr;
        }

        public void AddSymbol(string identifier)
        {
            Symbols.Add(identifier, StackPtr++);
        }

        public int GetSymbol(string identifier)
        {
            if (Symbols.ContainsKey(identifier))
            {
                return Symbols[identifier];
            }
            else if (Parent != null)
            {
                return Parent.GetSymbol(identifier);
            }
            return -1;
        }
    }
}
