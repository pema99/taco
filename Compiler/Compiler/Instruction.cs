using System;

namespace TacoCompiler
{
    public enum Instruction : int
    {
        VAR_ASSIGN,
        VAR_LOOKUP,
        CONST,

        CALL,

        PRINT,
        PRINTLN,
       
        ADD,
        SUB,
        MUL,
        DIV,

        LESS,
        GREATER,
        EQUAL,

        JMP,
        JMP_COND,

        CLS,
        INPUT_KEY,
        INPUT_LINE,

        PUSH_PARAMS,
        RETURN,
    }

    public static class InstructionLength
    {
        public static int[] Length = new int[]
        {
            1,
            1,
            1,

            1,

            0,
            0,

            0,
            0,
            0,
            0,

            0,
            0,
            0,

            1,
            1,

            0,
            0,
            0,

            1,
            0,
        };
    }
}
