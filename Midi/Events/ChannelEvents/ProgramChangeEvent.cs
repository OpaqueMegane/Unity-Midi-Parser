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

namespace Midi.Events.ChannelEvents
{
    public sealed class ProgramChangeEvent : ChannelEvent
    {
        public byte program_number
        {
            get
            {
                return this.parameter_1;
            }
        }

        public ProgramChangeEvent(int delta_time, byte midi_channel, byte program_number)
            : base(delta_time, 0xC0, midi_channel, program_number, 0x00)
        {
        }

        public override string ToString()
        {
            return "ProgramChangeEvent(" + base.ToString() + ", program_number: " + program_number + ")";
        }
    }
}

