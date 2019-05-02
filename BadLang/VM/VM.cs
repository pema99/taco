using System;
using System.Collections.Generic;
using System.Text;

namespace BadLang
{
    public class VM
    {
        private Stack<Variant> stack;
        private Variant[] variables;
        private byte[] heap;

        public void Execute(Binary b)
        {
            this.stack = new Stack<Variant>();
            this.heap = b.Heap;
            this.variables = new Variant[1000];

            Console.WriteLine("=== Execution start ===");

            int pc = b.EntryPoint;
            while (pc < b.Instructions.Length)
            {
                Instruction current = (Instruction)b.Instructions[pc].Bits;
                switch (current)
                {
                    case Instruction.PRINTLN:
                    case Instruction.PRINT:
                        Variant operand = stack.Pop();
                        string toPrint = "";
                        switch (operand.Type)
                        {
                            case VariantType.Boolean:
                                toPrint = operand.Boolean.ToString();
                                break;

                            case VariantType.Number:
                                toPrint = operand.Number.ToString();
                                break;

                            case VariantType.Pointer:
                                for (int i = 0; i < BitConverter.ToInt32(heap, operand.Pointer); i++)
                                {
                                    toPrint += (char)heap[operand.Pointer + 4 + i];
                                }
                                break;
                        }
                        Console.Write(toPrint);
                        if (current == Instruction.PRINTLN)
                        {
                            Console.WriteLine();
                        }
                        break;

                    case Instruction.CONST:
                        stack.Push(b.Instructions[++pc]);
                        break;

                    case Instruction.VAR_ASSIGN:
                        variables[b.Instructions[++pc].Pointer] = stack.Pop();
                        break;

                    case Instruction.VAR_LOOKUP:
                        stack.Push(variables[b.Instructions[++pc].Pointer]);
                        break;

                    case Instruction.ADD:
                    case Instruction.SUB:
                    case Instruction.MUL:
                    case Instruction.DIV:
                        Variant right = stack.Pop();
                        Variant left = stack.Pop();
                        if (right.Type != left.Type)
                        {
                            throw new Exception("Invalid binary operation");
                        }
                        if (right.Type == VariantType.Number)
                        {
                            float result = 0;
                            switch (current)
                            {
                                case Instruction.ADD:
                                    result = left.Number + right.Number;
                                    break;

                                case Instruction.SUB:
                                    result = left.Number - right.Number;
                                    break;

                                case Instruction.MUL:
                                    result = left.Number * right.Number;
                                    break;

                                case Instruction.DIV:
                                    result = left.Number / right.Number;
                                    break;
                            }
                            stack.Push(new Variant(result));
                        }
                        break;

                    case Instruction.RETURN:
                        return; //TODO: Calling
                        break;
                }
                pc++;
            }
        }
    }
}
