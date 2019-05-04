using System;
using System.Collections.Generic;
using System.Linq;

namespace TacoCompiler
{
    public class Compiler
    {
        private SemanticPass semantic;
        private CodeGenerationPass codeGenerator;
        private LinkerPass linker;

        public Compiler()
        {
            this.semantic = new SemanticPass();
            this.codeGenerator = new CodeGenerationPass();
            this.linker = new LinkerPass();
        }

        public Binary Compile(RawAST ast)
        {
            UnlinkedAST ir = codeGenerator.Process(semantic.Process(ast));           
            Binary b = linker.Process(ir);

            /*Console.WriteLine("=== Symbols ===");
            foreach (var symbol in ir.Symbols)
            {
                Console.WriteLine(symbol.Key + ": " + symbol.Value);
            }
            Console.WriteLine();

            Console.WriteLine("=== Instructions ===");
            for (int i = 0; i < b.Instructions.Length; i++)
            {
                //Display labels
                foreach (var symbol in ir.Symbols.Where(x => x.Value == i))
                {
                    Console.WriteLine("{0}:", symbol.Key);
                }

                //Display instruction
                Console.Write("    " + typeof(Instruction).GetEnumName((Instruction)(int)b.Instructions[i]) + " ");

                //Display parameters
                int length = InstructionLength.Length[b.Instructions[i]];
                for (int j = 0; j < length; j++)
                {
                    Console.Write(b.Instructions[++i] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();*/

            return b;
        }
    }
}
