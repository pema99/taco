use num;

use super::instructions::*;
use super::variant::*;
use super::binary::*;

pub const VAR_STACK_SIZE : usize = 1000;
pub const HEAP_SIZE : usize = 1000;

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
                            let str_len = i32::from_le_bytes(self.heap[ptr..ptr+4]);*/ //FIXME: Implement strings again uwu
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
                    let var_index = self.var_stack_offset - self.parameter_stack.last().unwrap() + Variant::from_long(bin.instructions[pc]).to_pointer() as usize;
                    self.var_stack[var_index] = self.stack.pop().unwrap();
                },
                Instruction::VAR_LOOKUP => {
                    pc += 1;
                    let var_index = self.var_stack_offset - self.parameter_stack.last().unwrap() + Variant::from_long(bin.instructions[pc]).to_pointer() as usize;
                    self.stack.push(self.var_stack[var_index]);
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