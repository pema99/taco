using System;
using System.Runtime.InteropServices;

namespace BadLang
{
    //4 byte type tag, 4 byte payload
    [StructLayout(LayoutKind.Explicit)]
    public struct Variant
    {
        [FieldOffset(0)]
        public VariantType Type;

        [FieldOffset(4)]
        public float Number;

        [FieldOffset(4)]
        public bool Boolean;

        [FieldOffset(4)]
        public int Pointer;

        [FieldOffset(0)]
        public long Bits;

        public Variant(float number)
        {
            this.Bits = 0;
            this.Type = VariantType.Number;         
            this.Boolean = false;
            this.Pointer = 0;
            this.Number = number;
        }

        public Variant(bool boolean)
        {
            this.Bits = 0;
            this.Type = VariantType.Boolean;
            this.Pointer = 0;
            this.Number = 0;
            this.Boolean = boolean;
        }

        public Variant(int pointer)
        {
            this.Bits = 0;
            this.Type = VariantType.Pointer;
            this.Number = 0;
            this.Boolean = false;
            this.Pointer = pointer;
        }

        public Variant(long bits)
        {
            this.Type = VariantType.Pointer;
            this.Number = 0;
            this.Boolean = false;
            this.Pointer = 0;
            this.Bits = bits;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case VariantType.Boolean:
                    return Boolean.ToString();

                case VariantType.Number:
                    return Number.ToString();

                case VariantType.Pointer:
                    return "$" + Pointer.ToString();

                default:
                    return "Invalid variant";
            }
        }

        public static implicit operator Variant(long l)
        {
            return new Variant(l);
        }

        public static implicit operator long(Variant v)
        {
            return v.Bits;
        }
    }
}
