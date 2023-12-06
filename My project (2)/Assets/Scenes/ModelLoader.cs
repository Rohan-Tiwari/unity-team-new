using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using System.IO;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

public class ModelLoader : MonoBehaviour
{
    public string modelName = "horseBlended"; // replace with your model name
    private string apiUrl = "http://192.168.50.96:5000/home/";
    //private string apiUrl = "http://127.0.0.1:5000/home/";

 
    void Start()
    {

        //GameObject meshObject = Resources.Load<GameObject>("car_0.obj");
        StartCoroutine(GetFileFromAPI("bottle"));
        //LoadModel();
          //GameObject instance = Instantiate(Resources.Load("car_0", typeof(GameObject))) as GameObject;
    }

    void LoadModel()
    {
        string filePath = Path.Combine(Application.persistentDataPath, modelName + ".obj");
        OBJLoader objloader= new OBJLoader();
        objloader.Load("C:\\Demo\\horseBlended.obj");
    
        //GameObject modelObject = LoadModelFromFile(filePath);
    }

    IEnumerator GetFileFromAPI(string param)
    {
        string filePath = Path.Combine(Application.persistentDataPath, param + ".obj");
        Debug.Log(Application.persistentDataPath);
        Debug.Log("trying to fetch model");
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

                System.IO.File.WriteAllBytes(filePath,www.downloadHandler.data);
                // Writing files directly to the asset bundle folder
                //System.IO.File.WriteAllBytes("C:\\Users\\FRA-UAS MR-Labor\\My project (2)\\Assets\\BundledAssets\\mynewbundle\\"+param+".obj",www.downloadHandler.data); 
                //string assetPath = AssetDatabase.GetAtPath("C:\\Users\\FRA-UAS MR-Labor\\My project (2)\\Assets\\BundledAssets\\mynewbundle\\test.obj");
                
                //Uncomment below for trying automation - Look into CreateAssetBundle.cs file
                // BuildAllAssetBundles();
                byte[] fileBytes = www.downloadHandler.data;
                Debug.Log(">>"+ www.downloadHandler.data);
                Stream stream = new MemoryStream(fileBytes);
                //OBJLoader objloader= new OBJLoader();
                //objloader.Load(stream);
            }
        }
        byte[] readData = ReadBytesFromFile(param);
        
        if (readData != null)
        {
            // Instantiate a GameObject and assign the read data
             Stream stream = new MemoryStream(readData);
             OBJLoader objloader= new OBJLoader();
             objloader.Load(stream);
          
        }
    }

    private byte[] ReadBytesFromFile(string param)
    {
        string filePath = Path.Combine(Application.persistentDataPath, param+".obj");
        return File.ReadAllBytes(filePath);
    }

}
