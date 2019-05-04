using System.Collections.Generic;

namespace TacoCompiler
{
    public class RawAST
    {
        public Dictionary<string, FuncDeclaration> Functions { get; private set; }

        public RawAST(Dictionary<string, FuncDeclaration> functions)
        {
            this.Functions = functions;
        }
    }
}
