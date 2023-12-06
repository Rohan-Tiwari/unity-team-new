using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class ModelDownloader : MonoBehaviour
{
    public string modelName = "default"; // replace with your model name

    void Start()
    {
        StartCoroutine(DownloadModel(modelName));
    }

    IEnumerator DownloadModel(string param)
    {
        string modelURL = "http://10.78.5.18:5000/home/"+modelName; // replace with your model URL
        string filePath = Path.Combine(Application.persistentDataPath, modelName + ".obj");

        using (WWW www = new WWW(modelURL))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                File.WriteAllBytes(filePath, www.bytes);
                Debug.Log("Model downloaded and saved: " + filePath);
            }
            else
            {
                Debug.LogError("Error downloading model: " + www.error);
            }
        }
    }
}
