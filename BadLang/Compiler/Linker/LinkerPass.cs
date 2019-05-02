using System;

namespace BadLang
{
    public class LinkerPass
    {
        public Binary Process(UnlinkedAST ast)
        {
            foreach (var link in ast.Unlinked)
            {
                int symbol = ast.Symbols[link.Item1];
                switch ((Instruction)ast.Instructions[link.Item2].Bits)
                {
                    case Instruction.CALL:
                        ast.Instructions[link.Item2 + 1] = new Variant(symbol);
                        break;

                    case Instruction.JMP_IF_NOT:
                        ast.Instructions[link.Item2 + 1] = new Variant(symbol);
                        break;
                }
            }
            return new Binary(ast.Instructions, ast.Symbols["main"], ast.Heap);
        }
    }
}
