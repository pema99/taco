using System;

namespace BadLang
{
    public enum Instruction : int
    {
        VAR_ASSIGN,
        PRINT,
        PRINTLN,
        CONST,
        RETURN,
        ADD,
        SUB,
        MUL,
        DIV,
        VAR_LOOKUP,
    }

    public static class InstructionLength
    {
        public static int[] Length = new int[]
        {
            1,
            0,
            0,
            1,
            0,
            0,
            0,
            0,
            0,
            1,
        };
    }
}
