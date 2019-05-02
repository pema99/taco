using System;
using System.Collections.Generic;
using System.Text;

namespace BadLang
{
    public class CheckedAST
    {
        public Dictionary<string, FuncDecl> Functions { get; private set; }
        public Dictionary<string, int> Identifiers { get; private set; }

        public CheckedAST(Dictionary<string, FuncDecl> functions, Dictionary<string, int> identifiers)
        {
            this.Functions = functions;
            this.Identifiers = identifiers;
        }
    }
}
