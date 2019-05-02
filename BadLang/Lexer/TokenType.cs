namespace BadLang
{
    public enum TokenType
    {
        Left_Paren, Right_Paren,
        Left_Brace, Right_Brace,
        Comma, Dot,   

        Bang, Bang_Equal,
        Equal, Equal_Equal,
        Greater, Greater_Equal,
        Less, Less_Equal,

        And, And_And, And_Equal,
        Or, Or_Or, Or_Equal,
        Xor, Xor_Xor, Xor_Equal,

        Plus, Plus_Equal, Plus_Plus,
        Minus, Minus_Equal, Minus_Minus,
        Star, Star_Equal,
        Slash, Slash_Equal,

        Identifier, String, Number,

        Else, False, Func, Loop, Break,
        If, Null, Print, PrintLine,
        Return, True, Var, Line,
        Key, Clear,

        EOF
    }
}
