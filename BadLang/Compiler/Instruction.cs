using System;

namespace BadLang
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
        JMP_IF,
        JMP_IF_NOT,

        CLS,
        INPUT_KEY,
        INPUT_LINE,

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
            1,

            0,
            0,
            0,

            0,
        };
    }
}
