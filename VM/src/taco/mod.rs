mod vm;
mod instructions;
mod variant;
mod binary;

pub use self::vm::VM;
pub use self::binary::Binary;

pub use self::vm::{ HEAP_SIZE, VAR_STACK_SIZE };