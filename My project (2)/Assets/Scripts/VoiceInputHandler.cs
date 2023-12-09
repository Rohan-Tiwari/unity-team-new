 using System;
 using UnityEngine;
 using Oculus.Voice;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Networking;
using Dummiesman;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dummiesman;
using System.IO;
using TMPro;
using System.Linq;
using System.Text;
using Meta.WitAi.TTS.Utilities;
 public class VoiceInputHandler : MonoBehaviour
 {
    public string apiUrl = " http://192.168.50.96:5000/home/";

    private IProgressIndicator indicator;
private int flag = 0;
// default scale of object 
private float scale = 0.3f;
[SerializeField]
private GameObject indicatorObject;

[SerializeField]
private TTSSpeaker speaker;
     /// <summary>
     /// Sets the color of the specified transform.
     /// </summary>
     /// <param name="trans"></param>
     /// <param name="color"></param>
    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    private Renderer rend;

     void Start(){  
        appVoiceExperience.Activate();
     }

     /// <summary>
     /// Updates the color of GameObject with the names specified in the input values.
     /// </summary>
     /// <param name="values"></param>
     public void UpdateColor(string[] values)
     {
        Debug.Log("CALLEDSSS");
         var colorString = values[0];
         Debug.Log("values "+values[0]);
        // var shapeString = values[1];
        
         if (!ColorUtility.TryParseHtmlString(colorString, out var color)) return;
         foreach(var rend in GetComponentsInChildren<Renderer>(true))
        {
            rend.material.color = color;
        }
     }

//Below method is to map voice commands to create an object
     /// <summary>
     /// Updates the color of GameObject with the names specified in the input values.
     /// </summary>
     /// <param name="values"></param>
     public void createObjectsFromVoice(string[] values){
        Debug.Log("Called For creating objects" + values[0]);
        var objectToCreate = values[0];
        StartCoroutine(generateAndGetModelFromVoice(objectToCreate));
     }

     public IEnumerator generateAndGetModelFromVoice(string objectName)
    {
        indicator = indicatorObject.GetComponent<IProgressIndicator>();
        indicatorObject.transform.localPosition = new Vector3(0,0,0);
        indicatorObject.transform.localScale = new Vector3(scale*15f, scale*15f, scale*15f);
        Debug.Log("Starting to load obj from voice " +apiUrl+objectName);
        speaker.Speak("Creating Object with voice input. Please wait for it to Load");
        UnityWebRequest www = UnityWebRequest.Get(apiUrl+objectName);
        www.SendWebRequest();
        OpenProgressIndicator();
        while(!www.isDone){
            yield return null;
        }
        Debug.Log("Done Laoding");
        
        OBJLoader objLoader = new OBJLoader();
        Stream ms = new MemoryStream(www.downloadHandler.data);
        GameObject objectToCreate = objLoader.Load(ms);
        
        objectToCreate.transform.SetParent(GameObject.Find("GenFromAI").transform);
        objectToCreate.transform.localPosition = new Vector3(0,0,0);
        objectToCreate.transform.localScale = new Vector3(scale, scale, scale);
        objectToCreate.AddComponent<ScriptAddingTest>();
        flag =1;
        CloseProgressIndicator();
        speaker.Speak("Object has been loaded");
        //objectToCreate.GetComponent<Renderer>().material = material;
    }

    private async void OpenProgressIndicator()
    {
        await indicator.OpenAsync();

        float progress = 0;
        while (progress <= 1 && flag!=1)
        {
            progress += Time.deltaTime;
            indicator.Message = "Loading Model...";
            indicator.Progress = progress;
            //await Task.Yield();
        }
    }

    private async void CloseProgressIndicator(){
        await indicator.CloseAsync();
    }
 }