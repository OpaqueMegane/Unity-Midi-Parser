using UnityEngine;
using System.Collections;
using Uniduino;
using System.IO;
using System.Collections.Generic;
using Midi;
using Midi.Util;
using Midi.Events;
using Midi.Chunks;


        public struct RelayBlender
        {
            public int pin;
            public int note ;
            float minOnDuration;

            public RelayBlender(int _pin, int _note)
            {
                pin = _pin;
                note = _note;
                minOnDuration = .04f;
            }
        }

    public class Game : MonoBehaviour
    {

        List<Midi.Events.MidiEvent> midiEvents;// = new List<Midi.Events.MidiEvent>(tc.events);

        public AudioClip clip;
        public AudioClip hitClip;
        byte NOTE_ON = 0x90;
        byte NOTE_OFF = 0x80;


        float matchNoteTime = float.NegativeInfinity;
        bool matchingNoteAvailable = false;
        int lastReceivedMatchNote;
        
        float lastInputTime = float.NegativeInfinity;
        KeyCode lastInput;


        RelayBlender[] blenders = 
        {
            new RelayBlender(8, 60),
            new RelayBlender(9, 61),
            new RelayBlender(10, 62),
            new RelayBlender(11, 63)

        };
        Dictionary<int, RelayBlender> noteToBlender = new Dictionary<int, RelayBlender>();

        int matchKey = 67;

        KeyCode[] inKeys = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow };


        // Use this for initialization
        void Start()
        {
            Arduino.global.Setup(ConfigurePins);

            foreach (RelayBlender rb in blenders)
            {
                noteToBlender.Add(rb.note, rb);
            }

            //load a midi
            FileStream file = System.IO.File.OpenRead(Application.dataPath + "/midi_test.mid");
            MidiData midiData = FileParser.parse(file);
            print(midiData.header.time_division);
            
            foreach (Midi.Chunks.TrackChunk tc in midiData.tracks)
            {
                midiEvents = new List<Midi.Events.MidiEvent>(tc.events);
              
                break;
            }

        }

        void ConfigurePins()
        {
            for (int i = 0; i < blenders.Length; i++)
            {
                Arduino.global.pinMode(blenders[i].pin, PinMode.OUTPUT);
            }
                
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(100, 100, Screen.width - 100, Screen.height - 100));
            if (GUILayout.Button("Press me", GUILayout.Width(200)))
            {
                //StartCoroutine(BlinkButtonDelay());
                StartCoroutine(BlenderSong());
            }
            GUILayout.EndArea();

        }


		void processMidiEvent(MidiEvent mEvent)
		{
		if (mEvent.event_type == NOTE_ON)
		{
			//audio.Pause();
			Midi.Events.ChannelEvents.NoteOnEvent noe = (Midi.Events.ChannelEvents.NoteOnEvent)mEvent;
			//print(noe.note_number + " _ " + noe.delta_time);
			//print (midiEventIndex + ": " + noe.note_number + " _ " + noe.delta_time);
			
			if (noe.note_number > 63)
			{
				if (lastInputTime != float.NegativeInfinity) //note was hit early
				{
					float diff = (audio.time - lastInputTime);
					print("early! : " + diff);
					
					
					
					acceptTimeDifference(diff, noe.note_number, lastInput);
					
					lastInputTime = float.NegativeInfinity;
				}
				else
				{
					matchNoteTime = audio.time;
					matchingNoteAvailable = true;
					lastReceivedMatchNote = noe.note_number;
				}
			}
			else
			{
				//AudioSource.PlayClipAtPoint(hitClip, Vector3.zero);
				RelayBlender rb = noteToBlender[noe.note_number];
				relayWrite(rb.pin, Arduino.HIGH);
				
				//print("tick! " + rb.note);
				StartCoroutine(turnOffAfter(rb, .0125f));
			}
			
		}
		else if (mEvent.event_type == NOTE_OFF)
		{
			
			Midi.Events.ChannelEvents.NoteOffEvent noe = (Midi.Events.ChannelEvents.NoteOffEvent) mEvent;
			if (noteToBlender.ContainsKey(noe.note_number))
			{
				RelayBlender rb = noteToBlender[noe.note_number];
				relayWrite(rb.pin, Arduino.LOW);
			}
			
			//print(noe.parameter_1);
			//noe.
		}

		}

        IEnumerator BlenderSong()
        {
            int midiEventIndex = 0;
            //yield return new WaitForSeconds(1);
      
            int nextEventTimeTicks = 0;
            //float startTime = Time.time;

  
            
            //float tickLength = (60000 / (120 * 192)); //milliseconds

            //AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            audio.Play();
            //audio.
            

           while (midiEventIndex < midiEvents.Count)
           {

			float timeInTicks = ((audio.time) * (120.0f * 96.0f)) / 60.0f;
               //print(timeInTicks + " " + nextEventTimeTicks);
               if (timeInTicks >= nextEventTimeTicks)
               {

				bool forceProcess = true;
				while (forceProcess || midiEvents[midiEventIndex].delta_time == 0)
				{
					forceProcess = false;

					nextEventTimeTicks += midiEvents[midiEventIndex].delta_time;
					midiEventIndex++;
					if (midiEventIndex >= 0 && midiEventIndex < midiEvents.Count)
					{
						processMidiEvent(midiEvents[midiEventIndex]);
					}	
				}
            
					
                   
                 

                       //nextEventTime += (midiEvents[midiEventIndex].delta_time * tickLength) / 1000; 
                   

          
               }

			yield return null;
           }

            //return null;
        }

		void relayWrite(int pin, int val)
		{
			//Arduino.global.digitalWrite(pin, val);
			if (val == Arduino.HIGH)
			{
				AudioSource.PlayClipAtPoint(hitClip, Vector3.zero);
			}
		}

        IEnumerator turnOffAfter(RelayBlender rb, float time)
        {
            yield return new WaitForSeconds(time);
			relayWrite(rb.pin, Arduino.LOW);
        }

        IEnumerator BlinkButtonDelay()
        {
            for (int j = 0; j < 1; j ++ )
            {
                for (int i = 0; i < 12; i++)
                {
                    int blenderPin = i % blenders.Length;
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
				relayWrite(blenders[blenderPin].pin, Arduino.HIGH);
                    yield return new WaitForSeconds(.025f);
                    gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
				relayWrite(blenders[blenderPin].pin, Arduino.LOW);
                    yield return new WaitForSeconds(.1f);
                    if (i % 2 == 1)
                    {
                        yield return new WaitForSeconds(.05f);
                    }
                }
                yield return new WaitForSeconds(.07f * 5);
            }
            

        }

        // Update is called once per frame
        void Update()
        {

			if (Input.GetKeyDown(KeyCode.P))
			{
				audio.Play();
			}

            KeyCode curInput = KeyCode.None;

            KeyCode[] inps = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow};
            for (int i = 0; i < inps.Length; i ++ )
            {
                if (Input.GetKeyDown(inps[i]))
                {
                    curInput = inps[i];
                    lastInput = curInput;
                }
            }



            if (curInput != KeyCode.None)
            {
                if (matchingNoteAvailable) // late
                {
                    float diff = (audio.time - matchNoteTime);

                    acceptTimeDifference(diff, lastReceivedMatchNote, curInput);
                    //print("late! : " + diff);
                    matchNoteTime = float.NegativeInfinity;
                    matchingNoteAvailable = false;
                }
                else
                {
                    lastInputTime = audio.time;
                    lastInput = curInput;
                }


            }

            if (Mathf.Abs(lastInputTime - audio.time) >= .5f)
            {
                //float way off!
                lastInputTime = float.NegativeInfinity;
            }
        }

        void acceptTimeDifference(float diff, int inNote, KeyCode inKey)
        {
            if (inKeys[inNote - matchKey] == inKey)
            {
                print("ddddd");
                if (Mathf.Abs(diff) < .15f)
                {
                    AudioSource.PlayClipAtPoint(hitClip, Vector3.zero);
                }
            }
            else
            {
                print("wanted " + inKeys[inNote - matchKey] + " got " + inKey);
            }
      
        }

        void OnApplicationQuit()
        {
            for (int i = 0; i < blenders.Length; i++)
            {
			relayWrite(blenders[i].pin, Arduino.LOW);
            }
        }

    }
