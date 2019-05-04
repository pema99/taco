#![feature(int_to_from_bytes)]
#![feature(duration_as_u128)] 
#[allow(non_camel_case_types)]
#[allow(unused_variables)]
#[allow(dead_code)]

use std::env;

mod taco;
use self::taco::{ VM, Binary };

fn main() {
    let args: Vec<String> = env::args().collect();
    if args.len() != 2 {
        println!("Usage: taco-vm <binary path>");
    }
    else {
        VM::new().execute(Binary::from_file(&args[1]).unwrap());
    }
}
