/*
Copyright (c) 2013 Christoph Fabritz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System.Linq;
using ByteEnumerable = System.Collections.Generic.IEnumerable<byte>;
using ByteList = System.Collections.Generic.List<byte>;
using BoolList = System.Collections.Generic.List<bool>;
using BoolEnumerable = System.Collections.Generic.IEnumerable<bool>;
using System;

namespace Midi.Util
{
    public sealed class VariableLengthUtil
    {
        private static BoolEnumerable convert_byte_to_bools(byte byte_in)
        {
            Func<byte, BoolEnumerable> convert_hex_to_bools = hex_in =>
            {
                var temp = new bool[0];

                switch (hex_in)
                {
                    case 0x00:
                        temp = new bool[] { false, false, false, false };
                        break;
                    case 0x01:
                        temp = new bool[] { false, false, false, true };
                        break;
                    case 0x02:
                        temp = new bool[] { false, false, true, false };
                        break;
                    case 0x03:
                        temp = new bool[] { false, false, true, true };
                        break;
                    case 0x04:
                        temp = new bool[] { false, true, false, false };
                        break;
                    case 0x05:
                        temp = new bool[] { false, true, false, true };
                        break;
                    case 0x06:
                        temp = new bool[] { false, true, true, false };
                        break;
                    case 0x07:
                        temp = new bool[] { false, true, true, true };
                        break;
                    case 0x08:
                        temp = new bool[] { true, false, false, false };
                        break;
                    case 0x09:
                        temp = new bool[] { true, false, false, true };
                        break;
                    case 0x0A:
                        temp = new bool[] { true, false, true, false };
                        break;
                    case 0x0B:
                        temp = new bool[] { true, false, true, true };
                        break;
                    case 0x0C:
                        temp = new bool[] { true, true, false, false };
                        break;
                    case 0x0D:
                        temp = new bool[] { true, true, false, true };
                        break;
                    case 0x0E:
                        temp = new bool[] { true, true, true, false };
                        break;
                    case 0x0F:
                        temp = new bool[] { true, true, true, true };
                        break;
                }

                return temp;
            };

            /*return
                convert_hex_to_bools((byte)((byte_in & 0xF0) >> 4));*/
            return new byte[] { (byte)((byte_in & 0xF0) >> 4), (byte)(byte_in & 0x0F) }
                .SelectMany(bb => convert_hex_to_bools(bb));
        }

        private static byte convert_bools_to_byte(BoolEnumerable bools_in)
        {
            Func<BoolEnumerable, byte> convert_bools_to_hex = b_in =>
            {
                var temp = new byte();

                switch (b_in.ElementAt(0))
                {
                    case true:
                        switch (b_in.ElementAt(1))
                        {
                            case true:
                                switch (b_in.ElementAt(2))
                                {
                                    case true:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x0F;
                                                break;
                                            case false:
                                                temp = 0x0E;
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x0D;
                                                break;
                                            case false:
                                                temp = 0x0C;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case false:
                                switch (b_in.ElementAt(2))
                                {
                                    case true:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x0B;
                                                break;
                                            case false:
                                                temp = 0x0A;
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x09;
                                                break;
                                            case false:
                                                temp = 0x08;
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case false:
                        switch (b_in.ElementAt(1))
                        {
                            case true:
                                switch (b_in.ElementAt(2))
                                {
                                    case true:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x07;
                                                break;
                                            case false:
                                                temp = 0x06;
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x05;
                                                break;
                                            case false:
                                                temp = 0x04;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case false:
                                switch (b_in.ElementAt(2))
                                {
                                    case true:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x03;
                                                break;
                                            case false:
                                                temp = 0x02;
                                                break;
                                        }
                                        break;
                                    case false:
                                        switch (b_in.ElementAt(3))
                                        {
                                            case true:
                                                temp = 0x01;
                                                break;
                                            case false:
                                                temp = 0x00;
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                }

                return temp;
            };

            bools_in = bools_in.ToList();
            /*var result1 = (byte)(
                (convert_bools_to_hex_L(bools_in) << 4)
                +
                 convert_bools_to_hex_L(bools_in.Skip(4).ToList())
                );*/
            var result1 = (byte)((convert_bools_to_hex(bools_in) << 4) + convert_bools_to_hex(bools_in.Skip(4).ToList()));

            return result1;
        }

        private static ByteEnumerable decode_V1(ByteEnumerable bytes_in)
        {
            var bytes_list_in = bytes_in.ToList();

            // Convert all bytes into a list of booleans and skip MSB
            var bits = bytes_list_in
                .SelectMany(b => convert_byte_to_bools(b).Skip(1)).ToList();

            // Prepend as many boolean falses as bits were kicked out
            bits.InsertRange(0, new bool[bytes_list_in.Count]);

            // Partition the list of booleans into groups of eight convert them to bytes
            var bytes_out = Enumerable.Range(0, bits.Count / 8).Select(i => convert_bools_to_byte(bits.Skip(i * 8).Take(8))).ToList().SkipWhile(b => b == 0x00).ToList();
            switch (bytes_out.Count)
            {
                case 0:
                    bytes_out = new ByteList() { 0x00 };
                    break;
            }
            return bytes_out;
        }

        private static ByteEnumerable decode_V2(ByteEnumerable bytes_in)
        {
            var bytes_out = new ByteList();
            {
                bytes_in = bytes_in.ToList();
                var bits = new bool[bytes_in.Count() * 8];

                for (var i = 0; i < bytes_in.Count(); i += 1)
                {
                    var b = bytes_in.ElementAt(i);
                    var bools = convert_byte_to_bools(b).Skip(1).ToArray();
                    bits.CopyTo(bools, i * 7);
                }

            }

            return bytes_out;
        }

        public static ByteEnumerable decode(ByteEnumerable bytes_in)
        {
            return decode_V1(bytes_in);
        }

        public static int decode_to_int(ByteEnumerable bytes_in)
        {
            var bytes = decode(bytes_in).ToList();

            switch (bytes.Count < 4)
            {
                case true:
                    var diff = 4 - bytes.Count;
                    bytes.InsertRange(0, new byte[diff]);
                    break;
            }

            bytes.Reverse();

            return BitConverter.ToInt32(bytes.ToArray(), 0);
        }
    }
}
