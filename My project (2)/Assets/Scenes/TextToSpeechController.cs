using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
public class TextToSpeechController: MonoBehaviour
{
    [SerializeField]
    public TTSSpeaker speaker;
    // Start is called before the first frame update
    void Start()
    {
        speaker = new TTSSpeaker();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        speaker.Speak("Welcome, Rohan. How are you doing?");
    }

}
