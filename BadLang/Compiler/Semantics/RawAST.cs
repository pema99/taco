using System.Collections.Generic;

namespace BadLang
{
    public class RawAST
    {
        public Dictionary<string, FuncDecl> Functions { get; private set; }

        public RawAST(Dictionary<string, FuncDecl> functions)
        {
            this.Functions = functions;
        }
    }
}
