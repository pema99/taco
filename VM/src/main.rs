#![feature(int_to_from_bytes)]
#![feature(duration_as_u128)] 
#[allow(non_camel_case_types)]
#[allow(unused_variables)]
#[allow(dead_code)]

use std::time::Instant;

pub mod vm;

fn main() {
    let mut x = vm::VM::new();
    let b = vm::Binary::from_file("out.bin").unwrap();
    let now = Instant::now();
    x.execute(b);
    println!("");
    println!("Execution took {}ms", now.elapsed().as_millis());
}
