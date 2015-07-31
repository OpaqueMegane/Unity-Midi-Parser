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
using TrackEventsIEnumerable = System.Collections.Generic.IEnumerable<Midi.Events.MidiEvent>;
using TrackEvents = System.Collections.ObjectModel.ReadOnlyCollection<Midi.Events.MidiEvent>;
using System.Linq;
using MidiEvent = Midi.Events.MidiEvent;

namespace Midi.Chunks
{
    public sealed class TrackChunk : Chunk
    {
        // Lazy loading mechanism
        private TrackEventsIEnumerable _events;

        public TrackEventsIEnumerable events
        {
            get
            {
                switch (_events.GetType() == typeof(TrackEvents))
                {
                    case false:
                        _events = _events.ToList().AsReadOnly();
                        break;
                }
                return _events;
            }
            private set { }
        }

        public TrackChunk(TrackEventsIEnumerable events)
            : base("MTrk")
        {
            this._events = events;
        }

        override public string ToString()
        {
            var events_string = events.Aggregate("", (string a, MidiEvent b) => a + b + ", ");
            events_string = events_string.Remove(events_string.Length - 2);

            return "TrackChunk(" + base.ToString() + ", events: [" + events_string + "])";
        }
    }
}
