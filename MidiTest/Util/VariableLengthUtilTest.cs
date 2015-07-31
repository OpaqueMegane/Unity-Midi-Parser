using System;
using System.Linq;
using Xunit;
using VariableLengthUtil = Midi.Util.VariableLengthUtil;
using ByteList = System.Collections.Generic.List<byte>;

namespace MidiTest.Util
{
    public class VariableLengthUtilTest
    {
        [Fact]
        public static void decode()
        {
            Assert.Equal(new ByteList() { 0x00 }, VariableLengthUtil.decode(new ByteList() { 0x00 }));
            Assert.Equal(new ByteList() { 0x40 }, VariableLengthUtil.decode(new ByteList() { 0x40 }));
            Assert.Equal(new ByteList() { 0x7F }, VariableLengthUtil.decode(new ByteList() { 0x7F }));
            Assert.Equal(new ByteList() { 0x80 }, VariableLengthUtil.decode(new ByteList() { 0x81, 0x00 }));
            Assert.Equal(new ByteList() { 0x20, 0x00 }, VariableLengthUtil.decode(new ByteList() { 0xC0, 0x00 }));
            Assert.Equal(new ByteList() { 0x3F, 0xFF }, VariableLengthUtil.decode(new ByteList() { 0xFF, 0x7F }));
            Assert.Equal(new ByteList() { 0x1F, 0xFF, 0xFF }, VariableLengthUtil.decode(new ByteList() { 0xFF, 0xFF, 0x7F }));
            Assert.Equal(new ByteList() { 0x08, 0x00, 0x00, 0x00 }, VariableLengthUtil.decode(new ByteList() { 0xC0, 0x80, 0x80, 0x00 }));
            Assert.Equal(new ByteList() { 0x0F, 0xFF, 0xFF, 0xFF }, VariableLengthUtil.decode(new ByteList() { 0xFF, 0xFF, 0xFF, 0x7F }));
        }

        [Fact]
        public static void decode_to_int()
        {
            Assert.Equal(0, VariableLengthUtil.decode_to_int(new ByteList() { 0x00 }));
            Assert.Equal(0x40 , VariableLengthUtil.decode_to_int(new ByteList() { 0x40 }));
            Assert.Equal(0x7F, VariableLengthUtil.decode_to_int(new ByteList() { 0x7F }));
            Assert.Equal(0x80, VariableLengthUtil.decode_to_int(new ByteList() { 0x81, 0x00 }));
            Assert.Equal(0x2000, VariableLengthUtil.decode_to_int(new ByteList() { 0xC0, 0x00 }));
            Assert.Equal(0x3FFF, VariableLengthUtil.decode_to_int(new ByteList() { 0xFF, 0x7F }));
            Assert.Equal(0x1FFFFF, VariableLengthUtil.decode_to_int(new ByteList() { 0xFF, 0xFF, 0x7F }));
            Assert.Equal(0x08000000, VariableLengthUtil.decode_to_int(new ByteList() { 0xC0, 0x80, 0x80, 0x00 }));
            Assert.Equal(0x0FFFFFFF, VariableLengthUtil.decode_to_int(new ByteList() { 0xFF, 0xFF, 0xFF, 0x7F }));
        }
    }
}
