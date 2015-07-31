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
using TracksIn = System.Collections.Generic.IEnumerable<Midi.Chunks.TrackChunk>;
using Tracks = System.Collections.Generic.List<Midi.Chunks.TrackChunk>;
using HeaderChunk = Midi.Chunks.HeaderChunk;
using TrackChunk = Midi.Chunks.TrackChunk;
using System.Linq;

namespace Midi
{
    public class MidiData
    {
        public readonly HeaderChunk header;
        public readonly TracksIn tracks;

        public MidiData(HeaderChunk header, TracksIn tracks)
        {
            this.header = header;
            this.tracks = tracks.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            var tracks_string = tracks.Aggregate("", (string a, TrackChunk b) => a + b + ", ");
            tracks_string = tracks_string.Remove(tracks_string.Length - 2);

            return "MidiFile(header: " + header + ", tracks: [" + tracks_string + "])";
        }
    }
}
