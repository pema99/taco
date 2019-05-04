use std::io::prelude::*;
use std::fs::File;
use byteorder::{LittleEndian, ReadBytesExt};

use super::HEAP_SIZE;

pub struct Binary {
    pub instructions: Box<Vec<i64>>,
    pub entry: usize,
    pub heap: [u8; HEAP_SIZE]
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