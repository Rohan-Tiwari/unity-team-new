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

public class TapToCreateHandler : MonoBehaviour, IMixedRealityFocusHandler, IMixedRealityPointerHandler
{
    private Color color_IdleState = Color.cyan;
    private Color color_OnHover = Color.red;
    private Color color_OnSelect = Color.blue;
    private Material material;


    public string url ="http://127.0.0.1:5000/home/car";
private IProgressIndicator indicator;
private int flag = 0;
// default scale of object 
private float scale = 0.3f;
[SerializeField]
private GameObject indicatorObject;

[SerializeField]
private TTSSpeaker speaker;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void IMixedRealityFocusHandler.OnFocusEnter(FocusEventData eventData)
    {
        material.color = color_OnHover;
    }

    void IMixedRealityFocusHandler.OnFocusExit(FocusEventData eventData)
    {
        material.color = color_IdleState;
    }

    void IMixedRealityPointerHandler.OnPointerDown(
         MixedRealityPointerEventData eventData) { }

    void IMixedRealityPointerHandler.OnPointerDragged(
         MixedRealityPointerEventData eventData) { }

    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        //material.color = color_OnSelect;
        getPointerClickedPosition(eventData);
    }
    public void getPointerClickedPosition(MixedRealityPointerEventData eventData){
        var result = eventData.Pointer.Result;
        var spawnPosition = result.Details.Point;
        var spawnRotation = Quaternion.LookRotation(result.Details.Normal);
        Debug.Log("result"+result);
        Debug.Log("spawnPosition "+spawnPosition);
        Debug.Log("spawnRotation "+spawnRotation);
        StartCoroutine(generateAndGetModel(spawnPosition));
    }

    /*public void findPositionofPointer(){
         foreach(var source in MixedRealityToolkit.InputSystem.DetectedInputSources)
         {
            foreach (var p in source.Pointers)
            {
                if (p is IMixedRealityNearPointer)
                {
                    // Ignore near pointers, we only want the rays
                    continue;
                }
                
                if (p.Result != null)
                {
                    var startPoint = p.Position;
                    var endPoint = p.Result.Details.Point;
                    var hitObject = p.Result.Details.Object;
                    Debug.Log("ray endPoint"+endPoint);
                    if (hitObject)
                    {
                        //StartCoroutine(generateAndGetModel());
                        /*var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.localScale = Vector3.one * 1f;
                        sphere.transform.position = endPoint;
                    }
                }
            }
         }
    }*/
    void IMixedRealityPointerHandler.OnPointerUp(
         MixedRealityPointerEventData eventData) { }

/*
*method to call py server responsible for creating objects using shap-e
*/
    public IEnumerator generateAndGetModel(Vector3 t)
    {
        indicator = indicatorObject.GetComponent<IProgressIndicator>();
        indicatorObject.transform.localPosition = t;
        indicatorObject.transform.localScale = new Vector3(scale*15f, scale*15f, scale*15f);
        Debug.Log("Starting to load obj "+url);
        speaker.Speak("Creating Object. Please wait for it to Load");
        UnityWebRequest www = UnityWebRequest.Get(url);
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
        objectToCreate.transform.localPosition = t;
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