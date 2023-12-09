using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Oculus.Voice;
using System.Collections.Generic;
using System.Linq;
using Dummiesman;
using System.IO;
using System.Text;

public class YourAPIController : MonoBehaviour
{
    //private string apiUrl = "http://127.0.0.1:5000/return-files/";
    //private string apiUrl = "http://192.168.246.107:5000/home/";
    private string apiUrl = "http://10.78.5.54:5000/home/";
    
    private List<string> values = new List<string>();

    [Header("Voice")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;
    public GameObject model;
    public GameObject loadedObj;

    void Start()
    {
        appVoiceExperience.Activate();   
        Debug.Log("Called in starrt");       
    }

    void Update()
    {
        Debug.Log("CALLED IN UPDATE");
        //appVoiceExperience.Activate();
        if(values.Count>0){
            //Debug.Log(values[0]);
            //StartCoroutine(GetFileFromAPI());
        }
    }
    
   
    IEnumerator GetFileFromAPI(string param)
    {

        using (UnityWebRequest www = UnityWebRequest.Get(apiUrl + param))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch data: " + www.error);
            }
            else
            {
                // Assuming your API sends a file as bytes
                byte[] fileBytes = www.downloadHandler.data;
                Debug.Log(">>"+ www.downloadHandler.data);
                System.IO.File.WriteAllBytes("C:\\Users\\FRA-UAS MR-Labor\\My project (1)\\Assets\\Scenes\\"+param+".obj", fileBytes);
                Debug.Log("File downloaded successfully!");
            }
        }
    }

/// <summary>
     /// Updates the color of GameObject with the names specified in the input values.
     /// </summary>
     /// <param name="val"></param>
    public void getValuesFromVoice(string[] val){
        
        values = val.ToList();
        Debug.Log("THIS IS CALLED ONCE?");
        foreach (string param in val){
          Debug.Log("Got Value from Voice Command is > "+param);
          StartCoroutine(GetFileFromAPI(param));  
        }
        //StartCoroutine(GetFileFromAPI());
    }


}
