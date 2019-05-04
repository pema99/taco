use num_derive::{FromPrimitive, ToPrimitive};

#[repr(i32)]
#[derive(FromPrimitive, ToPrimitive)]
#[derive(Debug)]
pub enum Instruction {
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
    RETURN
}