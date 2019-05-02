using System;
using System.Collections.Generic;
using System.Text;

namespace BadLang
{
    public class VM
    {
        private Stack<Variant> stack;
        private Stack<int> callStack;
        private Variant[] variables;
        private byte[] heap;

        public void Execute(Binary b)
        {
            this.stack = new Stack<Variant>();
            this.callStack = new Stack<int>();
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
                    case Instruction.LESS:
                    case Instruction.GREATER:
                    case Instruction.EQUAL:
                        Variant right = stack.Pop();
                        Variant left = stack.Pop();
                        if (right.Type != left.Type)
                        {
                            throw new Exception("Invalid binary operation");
                        }
                        if (right.Type == VariantType.Number)
                        {
                            switch (current)
                            {
                                case Instruction.ADD:
                                    stack.Push(new Variant(left.Number + right.Number));
                                    break;

                                case Instruction.SUB:
                                    stack.Push(new Variant(left.Number - right.Number));
                                    break;

                                case Instruction.MUL:
                                    stack.Push(new Variant(left.Number * right.Number));
                                    break;

                                case Instruction.DIV:
                                    stack.Push(new Variant(left.Number / right.Number));
                                    break;

                                case Instruction.LESS:
                                    stack.Push(new Variant(left.Number < right.Number));
                                    break;

                                case Instruction.GREATER:
                                    stack.Push(new Variant(left.Number > right.Number));
                                    break;

                                case Instruction.EQUAL:
                                    stack.Push(new Variant(left.Number == right.Number));
                                    break;
                            }
                        }
                        break;

                    case Instruction.RETURN:
                        if (callStack.Count == 0)
                        {
                            return;
                        }
                        pc = callStack.Pop();
                        break;

                    case Instruction.CALL:
                        callStack.Push(pc + 1);
                        pc = b.Instructions[pc + 1].Pointer - 1;
                        break;

                    case Instruction.JMP_IF_NOT:
                        Variant cond = stack.Pop();
                        if (cond.Pointer == 0)
                        {
                            pc = b.Instructions[pc + 1].Pointer - 1;
                        }
                        else
                        {
                            pc++;
                        }
                        break;
                }
                pc++;
            }
        }
    }
}
