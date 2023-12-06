using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dummiesman;
using System.IO;

public class New_ModelRuntime : MonoBehaviour
{
    public string url = "http://192.168.50.96:5000/home/car";

    IEnumerator Start()
    {
        Debug.Log("Starting to load obj ");
        GameObject objectToCreate = new GameObject("ObjectToCreate::name");
        objectToCreate.AddComponent<Slider>();
        Slider sliderComponent = objectToCreate.GetComponent<Slider>();
        sliderComponent.minValue = 0;
        sliderComponent.maxValue = 1;
        sliderComponent.value = 0;

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            sliderComponent.value = www.downloadProgress;
            yield return null;
        }
Debug.Log("Done Laoding");
        Destroy(sliderComponent);

        OBJLoader objLoader = new OBJLoader();
        Stream ms = new MemoryStream(www.downloadHandler.data);
        objectToCreate = objLoader.Load(ms);
        //objectToCreate.GetComponent<Renderer>().material = material;
    }
}