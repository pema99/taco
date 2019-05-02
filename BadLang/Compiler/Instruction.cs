using System;

namespace BadLang
{
    public enum Instruction : int
    {
        VAR_ASSIGN,
        PRINT,
        CONST,
        RETURN,
    }

    public static class InstructionLength
    {
        public static int[] Length = new int[]
        {
            1,
            0,
            1,
            0,
        };
    }
}
