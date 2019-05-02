using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadLang
{
    public enum TokenType
    {
        //Language stuff
        Left_Paren, Right_Paren, Left_Brace, Right_Brace,
        Left_Square, Right_Square,
        Comma, Dot, Minus, Plus, Slash, Star,

        //Operators
        Bang, Bang_Equal,
        Equal, Equal_Equal,
        Greater, Greater_Equal,
        Less, Less_Equal,
        And, Or, Xor,
        Plus_Equal, Plus_Plus, Minus_Equal, Minus_Minus, Star_Equal, Slash_Equal,

        //Literals n stuff
        Identifier, String, Number,

        //Reserved words
        Else, False, Func, For, If, Null,
        Print, PrintLine, Return, True,
        Var, While, Line, Key, Clear,

        //End of file
        EOF
    }
}
