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
    public abstract class ChannelEvent : MidiEvent
    {
        public readonly byte midi_channel;
        public readonly byte parameter_1;
        public readonly byte parameter_2;

        public ChannelEvent(int delta_time, byte event_type, byte midi_channel, byte parameter_1, byte parameter_2)
            : base(delta_time, event_type)
        {
            this.midi_channel = midi_channel;
            this.parameter_1 = parameter_1;
            this.parameter_2 = parameter_2;
        }

        public override string ToString()
        {
            return "ChannelEvent(" + base.ToString() + ", midi_channel: " + midi_channel + ")";
        }
    }
}
