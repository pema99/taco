use std::mem::transmute;

#[repr(i32)]
#[derive(Clone)]
pub enum VariantType {
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
pub struct Variant {
    pub value_type: VariantType,
    value: VariantValue
}

impl Variant {
    pub fn from_number(number: f32) -> Self {
        Self {
            value_type: VariantType::Number,
            value: VariantValue { number: number }
        }
    }

    pub fn to_number(&self) -> f32 {
        unsafe {
            self.value.number
        }
    }

    pub fn from_boolean(boolean: bool) -> Self {
        Self {
            value_type: VariantType::Boolean,
            value: VariantValue { pointer: if boolean { 1 } else { 0 } }
        }
    }

    pub fn to_boolean(&self) -> bool {
        unsafe {
            self.value.boolean
        }
    }

    pub fn from_pointer(pointer: i32) -> Self {
        Self {
            value_type: VariantType::Pointer,
            value: VariantValue { pointer: pointer }
        }
    }

    pub fn to_pointer(&self) -> i32 {
        unsafe {
            self.value.pointer
        }
    }

    pub fn from_long(long: i64) -> Self {
        return unsafe {
            transmute (long)
        }
    }

    pub fn to_long(&self) -> i64 {
        return unsafe {
            transmute(self.clone())
        }
    }
}