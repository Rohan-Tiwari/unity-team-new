using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Voice;
using TMPro;
using System;


public class VoiceIntentController : MonoBehaviour
{
    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI fullTranscritpText;

    [SerializeField]
    private TextMeshProUGUI partialTranscritpText;

    private GameObject gameObject;
    //private ShapeController[] controllers;

    private bool appVoiceActive;

    private void Awake(){
       // controllers = FindObjectsOfType<ShapeController>();
        /*fullTranscritpText.text = partialTranscritpText.text = string.Empty;

        appVoiceExperience.events.onFullTranscription.AddListener((transcription)=>{
            fullTranscritpText.text = transcription;
        });

        
        appVoiceExperience.events.onPartialTranscritpText.AddListener((transcription)=>{
            fullTranscritpText.text = transcription;
        });*/
    }
    // Start is called before the first frame update
    void Start()
    {
          gameObject = GameObject.Find("example_mesh_0");
    }
    // Update is called once per frame
    void Update()
    {
            appVoiceExperience.Activate();
   }

    public void setColor(String[] info){
        Debug.Log("hi");
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
