use std::mem::transmute;
use std::io::prelude::*;
use std::fs::File;
use byteorder::{LittleEndian, ReadBytesExt};
use num_derive::{FromPrimitive, ToPrimitive};
use num;

#[repr(i32)]
#[derive(FromPrimitive, ToPrimitive)]
#[derive(Debug)]
enum Instruction {
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

#[repr(i32)]
#[derive(Clone)]
enum VariantType {
    Number,
    Boolean,
    Pointer
}

#[derive(Copy)]
#[derive(Clone)]
union VariantValue {
    number: f32,
    boolean: bool,
    pointer: i32
}

#[repr(C)]
#[derive(Clone)]
struct Variant {
    value_type: VariantType,
    value: VariantValue
}

impl Variant {
    fn from_number(number: f32) -> Self {
        Self {
            value_type: VariantType::Number,
            value: VariantValue { number: number }
        }
    }

    fn to_number(&self) -> f32 {
        unsafe {
            self.value.number
        }
    }

    fn from_boolean(boolean: bool) -> Self {
        Self {
            value_type: VariantType::Boolean,
            value: VariantValue { pointer: if boolean { 1 } else { 0 } }
        }
    }

    fn to_boolean(&self) -> bool {
        unsafe {
            self.value.boolean
        }
    }

    fn from_pointer(pointer: i32) -> Self {
        Self {
            value_type: VariantType::Pointer,
            value: VariantValue { pointer: pointer }
        }
    }

    fn to_pointer(&self) -> i32 {
        unsafe {
            self.value.pointer
        }
    }


    fn from_long(long: i64) -> Self {
        return unsafe {
            transmute (long)
        }
    }

    fn to_long(&self) -> i64 {
        return unsafe {
            transmute(self.clone())
        }
    }
}

pub struct Binary {
    instructions: Box<Vec<i64>>,
    entry: usize,
    heap: [u8; HEAP_SIZE]
}

impl Binary {
    pub fn from_file(path: &str) -> Result<Self, String> {
        let mut f = File::open(path).unwrap();

        let entry = f.read_i32::<LittleEndian>().unwrap();

        let mut heap = [0; HEAP_SIZE];
        f.read_exact(&mut heap).unwrap();

        let mut instructions : Vec<i64> = Vec::new();
        let mut buffer : [u8; 8] = [0; 8];
        
        while let Ok(x) = f.read_exact(&mut buffer) {          
            instructions.push(i64::from_le_bytes(buffer));
        }

        Ok(Binary {
            instructions: Box::new(instructions),
            entry: entry as usize,
            heap: heap
        })
    }
}

const VAR_STACK_SIZE : usize = 1000;
const HEAP_SIZE : usize = 1000;

pub struct VM {
    stack: Vec<i64>,
    call_stack: Vec<usize>,
    parameter_stack: Vec<usize>,
    var_stack: [i64; VAR_STACK_SIZE],
    var_stack_offset: usize,
    heap: [u8; HEAP_SIZE]
}

impl VM {
    pub fn new() -> Self {
        Self {
            stack: Vec::new(),
            call_stack: Vec::new(),
            parameter_stack: Vec::new(),
            var_stack: [0; VAR_STACK_SIZE],
            var_stack_offset: 0,
            heap: [0; HEAP_SIZE]
        }
    }

    pub fn execute(&mut self, bin: Binary) {
        self.heap = bin.heap;
        let mut pc = bin.entry;

        while pc < bin.instructions.len() {
            let opcode = bin.instructions[pc];
            let current : Instruction = num::FromPrimitive::from_i64(opcode).unwrap();

            match current {
                Instruction::PRINT | Instruction::PRINTLN => {
                    let operand = Variant::from_long(self.stack.pop().unwrap());
                    let to_print = match operand.value_type {
                        VariantType::Boolean => operand.to_boolean().to_string(),
                        VariantType::Number => operand.to_number().to_string(),
                        VariantType::Pointer => {
                            /*let ptr = operand.to_pointer() as usize;
                            let str_len = i32::from_le_bytes(self.heap[ptr..ptr+4]);*/
                            "".to_owned()
                        }
                    };
                    print!("{}", to_print);
                    if let Instruction::PRINTLN = current {
                        println!("");
                    }
                },
                Instruction::CONST => {
                    pc += 1;
                    self.stack.push(bin.instructions[pc]);
                },
                Instruction::VAR_ASSIGN => {
                    pc += 1;
                    self.var_stack[self.var_stack_offset - self.parameter_stack.last().unwrap() + Variant::from_long(bin.instructions[pc]).to_pointer() as usize] = self.stack.pop().unwrap();
                },
                Instruction::VAR_LOOKUP => {
                    pc += 1;
                    self.stack.push(self.var_stack[self.var_stack_offset - self.parameter_stack.last().unwrap() + Variant::from_long(bin.instructions[pc]).to_pointer() as usize]);
                },
                Instruction::ADD | Instruction::SUB | Instruction::MUL | Instruction::DIV |
                Instruction::LESS | Instruction::GREATER | Instruction::EQUAL => {
                    let right = Variant::from_long(self.stack.pop().unwrap());
                    let left = Variant::from_long(self.stack.pop().unwrap());
                    
                    if let VariantType::Number = left.value_type {
                        match current {
                            Instruction::ADD => self.stack.push(Variant::from_number(left.to_number() + right.to_number()).to_long()),
                            Instruction::SUB => self.stack.push(Variant::from_number(left.to_number() - right.to_number()).to_long()),
                            Instruction::MUL => self.stack.push(Variant::from_number(left.to_number() * right.to_number()).to_long()),
                            Instruction::DIV => self.stack.push(Variant::from_number(left.to_number() / right.to_number()).to_long()),
                            Instruction::LESS => self.stack.push(Variant::from_boolean(left.to_number() < right.to_number()).to_long()),
                            Instruction::GREATER => self.stack.push(Variant::from_boolean(left.to_number() > right.to_number()).to_long()),
                            Instruction::EQUAL => self.stack.push(Variant::from_boolean(left.to_number() == right.to_number()).to_long()),
                            _ => {}
                        }
                    }
                },
                Instruction::RETURN => {
                    if self.call_stack.len() == 0 {
                        return;
                    }
                    pc = self.call_stack.pop().unwrap();
                    self.var_stack_offset -= self.parameter_stack.pop().unwrap();
                },
                Instruction::PUSH_PARAMS => {
                    pc += 1;
                    let num_params = Variant::from_long(bin.instructions[pc]).to_pointer() as usize;
                    for i in 0..num_params {
                        self.var_stack[self.var_stack_offset + i] = self.stack.pop().unwrap();
                    }
                    self.var_stack_offset += num_params;
                    self.parameter_stack.push(num_params);
                },
                Instruction::CALL => {
                    self.call_stack.push(pc + 1);
                    pc = Variant::from_long(bin.instructions[pc + 1]).to_pointer() as usize - 1;
                },
                Instruction::JMP_COND => {
                    let cond = Variant::from_long(self.stack.pop().unwrap());
                    if cond.to_pointer() == 0 {
                        pc = Variant::from_long(bin.instructions[pc + 1]).to_pointer() as usize - 1;
                    }
                    else {
                        pc += 1;
                    }
                },
                _ => {}
            }
            pc += 1;

            /*println!("{:?}", current);
            print!("vars = ");
            for i in 0..self.var_stack_offset + 10 {
                print!("{:?} ", self.var_stack[i]);
            }
            println!("");
            print!("stack = ");
            for i in self.stack.iter() {
                print!("{:?} ", i);
            }
            println!("\n");*/
        }       
    }
}