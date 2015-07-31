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

namespace Midi.Events.MetaEvents
{
    public sealed class TimeSignatureEvent : MetaEvent
    {
        public readonly byte numerator;
        public readonly byte denominator;
        public readonly byte metronome_pulse;
        public readonly byte number_of_32nd_notes_per_MIDI_quarter_note;

        public TimeSignatureEvent(int delta_time, byte numerator, byte denominator, byte metronome_pulse, byte number_of_32nd_notes_per_MIDI_quarter_note)
            : base(delta_time, 0x58)
        {
            this.numerator = numerator;
            this.denominator = denominator;
            this.metronome_pulse = metronome_pulse;
            this.number_of_32nd_notes_per_MIDI_quarter_note = number_of_32nd_notes_per_MIDI_quarter_note;
        }

        public override string ToString()
        {
            return "TimeSignatureEvent(" + base.ToString() + ", numerator: " + numerator + ", denominator: " + denominator + ", metronome_pulse: " + metronome_pulse + ", number_of_32nd_notes_per_MIDI_quarter_note: " + number_of_32nd_notes_per_MIDI_quarter_note + ")";
        }
    }
}

