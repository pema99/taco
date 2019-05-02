using System;
using System.Collections.Generic;
using System.Linq;

namespace BadLang
{
    public class Compiler
    {
        private List<Variant> instructions;
        private Dictionary<string, int> symbols;
        private int heapPtr;

        //instructions, symbols, heap starts
        public (Variant[], Dictionary<string, int>, int) Compile(Dictionary<string, FuncDecl> functions)
        {
            instructions = new List<Variant>();
            symbols = new Dictionary<string, int>();
            heapPtr = 0;

            foreach (FuncDecl func in functions.Values)
            {
                symbols.Add(func.Name, instructions.Count); 

                for (int i = 0; i < func.Parameters.Length; i++)
                {
                    AddInstruction(Instruction.VAR_ASSIGN, i);
                }

                foreach (Expression statement in func.Body.Statements)
                {
                    CompileExpression(statement);
                }
            }

            Console.WriteLine("=== Symbols ===");
            foreach (var symbol in symbols)
            {
                Console.WriteLine(symbol.Key + ": " + symbol.Value);
            }
            Console.WriteLine();

            Console.WriteLine("=== Instructions ===");
            for (int i = 0; i < instructions.Count; i++)
            {
                //Display labels
                foreach (var symbol in symbols.Where(x => x.Value == i))
                {
                    Console.WriteLine("{0}:", symbol.Key);
                }

                //Display instruction
                Console.Write("    " + typeof(Instruction).GetEnumName((Instruction)(int)instructions[i]) + " ");

                //Display parameters
                int length = InstructionLength.Length[instructions[i]];
                for (int j = 0; j < length; j++)
                {
                    Console.Write(instructions[++i] + " ");
                }
                Console.WriteLine();
            }

            return (instructions.ToArray(), symbols, heapPtr);
        }

        public void CompileExpression(Expression expression)
        {
            switch (expression)
            {
                //Statements
                case ReturnExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.RETURN);
                    break;

                case PrintExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.PRINT);
                    break;

                case VariableDeclarationExpr e:
                    CompileExpression(e.Expr);
                    AddInstruction(Instruction.VAR_ASSIGN, );
                    break;

                //Expressions
                case ConstantExpr e:
                    switch (e.Data)
                    {
                        case  float f: AddInstruction(Instruction.CONST, new Variant(f)); break;
                        case   bool b: AddInstruction(Instruction.CONST, new Variant(b)); break;
                        case string s:
                            AddInstruction(Instruction.CONST, new Variant(heapPtr));
                            heapPtr += s.Length + 4; //string length + 4 bytes of length
                            break;
                    }                   
                    break;
            }
        }

        public void CompileBlock(Block block)
        {

        }

        public void AddInstruction(Instruction instr)
        {
            instructions.Add((long)instr);
        }

        public void AddInstruction(Instruction instr, Variant operand)
        {
            instructions.Add((long)instr);
            instructions.Add(operand);
        }
    }
}
