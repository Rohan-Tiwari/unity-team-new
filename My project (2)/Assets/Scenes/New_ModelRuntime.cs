using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dummiesman;
using System.IO;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using System.Linq;
using System.Collections;
using System.Text;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
/*
*author : Rohan Tiwari
* Purpose : Script to connect with the server and load objects on runtime. Currently works only on start, needs to be configured to include voice commands to create objects. 
* Should run all the time 
*/
public class New_ModelRuntime : MonoBehaviour
{
    //public string url = "http://192.168.246.107:5000/home/car";
    public string url ="http://127.0.0.1:5000/home/car";
private IProgressIndicator indicator;
private int flag = 0;
// default scale of object 
private float scale = 0.2f;
[SerializeField]
private GameObject indicatorObject;
    IEnumerator Start()
    {
        indicator = indicatorObject.GetComponent<IProgressIndicator>();
        Debug.Log("Starting to load obj ");
        //GameObject objectToCreate = new GameObject("ObjectToCreate::name");
        UnityWebRequest www = UnityWebRequest.Get(url);
        //yield return www.SendWebRequest();
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
        objectToCreate.transform.localScale = new Vector3(scale, scale, scale);
        
        flag =1;
        CloseProgressIndicator();
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

    public IEnumerator generateAndGetModel()
    {
        indicator = indicatorObject.GetComponent<IProgressIndicator>();
        Debug.Log("Starting to load obj ");
        //GameObject objectToCreate = new GameObject("ObjectToCreate::name");
        UnityWebRequest www = UnityWebRequest.Get(url);
        //yield return www.SendWebRequest();
        www.SendWebRequest();
        OpenProgressIndicator();
        while(!www.isDone){
            yield return null;
        }
        OBJLoader objLoader = new OBJLoader();
        Stream ms = new MemoryStream(www.downloadHandler.data);
        GameObject objectToCreate = objLoader.Load(ms);
        
        objectToCreate.transform.SetParent(GameObject.Find("GenFromAI").transform);
        objectToCreate.transform.localScale = new Vector3(scale, scale, scale);
        
        flag =1;
        CloseProgressIndicator();
        //objectToCreate.GetComponent<Renderer>().material = material;
    }
}