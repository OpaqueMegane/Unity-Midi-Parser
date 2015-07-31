using UnityEngine;
using System.Collections;
using Uniduino;


public class SimonSays : MonoBehaviour {


    //enum {giving};
    AudioClip[] directions;
    int[] pins = { 8, 9, 10, 11 };
    KeyCode[] directionKeys = { };
    KeyCode lastKey = KeyCode.None;
	KeyCode[] inps = { KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow };


	// Use this for initialization
	void Start () {
        Arduino.global.Setup(ConfigurePins);
        
	}

    void ConfigurePins()
    {
        for (int i = 0; i < 4; i++)
        {
            Arduino.global.pinMode(pins[i], PinMode.OUTPUT);
        }

    }

	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("dddd");
            StartCoroutine(simonSays());
        }
        
        for (int i = 0; i < inps.Length; i++)
        {
            if (Input.GetKeyDown(inps[i]))
            {
                lastKey = inps[i];
            }
        }
	}
   
    int keyToInt(KeyCode kc)
    {
        for (int i = 0; i < inps.Length; i++)
        {
            if (inps[i] == kc)
            {
                return i;
            }
        }
        return -1;
    }

    IEnumerator simonSays()
    {
        int nCodesCompleted = 0;
        bool lost = false;
        int codeLength = 4;
        float speed = 1;

        while (!lost)
        {
     

            //form prompt
            int[] code = new int[codeLength];
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = Random.Range(0, 4);
            }

            //give prompt
            for (int i = 0; i < code.Length; i++)
            {
                print(inps[code[i]]);
                Arduino.global.digitalWrite(pins[code[i]], Arduino.HIGH); // led ON			
                yield return new WaitForSeconds(.15f);

                Arduino.global.digitalWrite(pins[code[i]], Arduino.LOW); // led OFF
                yield return new WaitForSeconds(1.25f - speed * 0.85f);		
            }

            //wait for response
            print("ok@");
            int codeIdx = 0;

            while (codeIdx < code.Length)
            {
                if (lastKey != KeyCode.None)
                {
                    
                    StartCoroutine(onOff(keyToInt(lastKey)));

                    print("_________" + lastKey);
                    if (lastKey != inps[code[codeIdx]])
                    {
                        
                        lost = true;
                    }

                    codeIdx++;
                    

                    lastKey = KeyCode.None;
                }
               
                yield return null;
            }

            

            //tell result
            if (lost)
            {
                //lose routine
                yield return StartCoroutine(loseRoutine());
                print("done");
             
                
            }
            else
            {
                yield return StartCoroutine(winRoutine());
                nCodesCompleted++;

                if (nCodesCompleted % 4 == 3)
                {
                    codeLength++;
                }
                speed = (nCodesCompleted % 11) / 11.0f * Random.Range(0, 1.2f);
            }

            yield return (5);
            yield return null;
        }
       
    }

     IEnumerator onOff(int i)
    {
        Arduino.global.digitalWrite(pins[i], Arduino.HIGH); // led ON		
        yield return new WaitForSeconds(.05f);
        Arduino.global.digitalWrite(pins[i], Arduino.LOW); // led OFF
        yield return new WaitForSeconds(.1f);
    }


    IEnumerator winRoutine()
    {
        for (int i = 0; i < 5; i ++)
        {
            for (int j = 0; j < 4; j++)
            {
                Arduino.global.digitalWrite(pins[j], Arduino.HIGH); // led ON		
                yield return new WaitForSeconds(.05f);
                Arduino.global.digitalWrite(pins[j], Arduino.LOW); // led OFF
                yield return new WaitForSeconds(.1f);
            }
        }
        
    }

    IEnumerator loseRoutine()
    {
        for (int j = 0; j < 3; j ++ )
        {
            for (int i = 0; i < 4; i++)
            {
                Arduino.global.digitalWrite(pins[i], Arduino.HIGH); // led ON			

            }
            yield return new WaitForSeconds(.2f);

            for (int i = 0; i < 4; i++)
            {
                Arduino.global.digitalWrite(pins[i], Arduino.LOW); // led OFF

            }
            yield return new WaitForSeconds(.25f);
        }
         
       

        for (int i = 0; i < 4; i++)
        {
            Arduino.global.digitalWrite(pins[i], Arduino.HIGH); // led ON			
            
        }
        yield return new WaitForSeconds(.75f);

        for (int i = 0; i < 4; i++)
        {
            Arduino.global.digitalWrite(pins[i], Arduino.LOW); // led OFF
            
        }
        yield return new WaitForSeconds(.5f);
        print("lose!");
    }

    void OnApplicationQuit()
    {

    }
}
