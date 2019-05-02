using System;

namespace BadLang
{
    public class LinkerPass
    {
        public Binary Process(UnlinkedAST ast)
        {
            //TODO: Linking
            return new Binary(ast.Instructions, ast.Symbols["main"], ast.Heap);
        }
    }
}
