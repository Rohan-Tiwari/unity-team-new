using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
 using System.Text;
public class GPT_Controller : MonoBehaviour
{
     private string apiUrl = "https://api.openai.com/v1/completions";
    private string apiKey = "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";
     public const string API_URL = "https://api.openai.com/v1/completions";
    private string inputText = "hi tell me about the global warming";
    public string prompt="Hello, how are you today?";
    public int maxTokens = 2048;
    public float temperature = 0.7f;
    void Start()
    {
        
    }

     public void CallGPTAPI()
    {
        StartCoroutine(SendGPTRequest());
    }

    IEnumerator SendGPTRequest()
    {
       UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
     byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"model\":\"text-davinci-002\", \"prompt\":\"" + prompt + "\", \"max_tokens\":" + maxTokens + ", \"temperature\":" + temperature + "}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

    // Check for errors
    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.DataProcessingError)
    {
        Debug.LogError(request.error);
        //ErrorText.text = request.error;
        // ErrorText.text = request.error.ToString();
    }
    else
    {
        // Filtering  Response Text To Get the response  
        string responseText = request.downloadHandler.text; // Fetch the response
        int startIndex = responseText.IndexOf("text\":\"") + "text\":\"".Length;  
        int endIndex = responseText.IndexOf("\",", startIndex);
        int length = endIndex - startIndex;
        string response = responseText.Substring(startIndex, length);
        Debug.Log(response);
        //ResultText.text = response;
    
    
    }
    }
}
