using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
 using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Meta.WitAi.TTS.Utilities;
public class GPT_Controller : MonoBehaviour
{
     private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string apiKey = "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";
    
    [SerializeField]
    private TTSSpeaker speaker;

    void Start()
    {
        
    }

     public void CallGPTAPI()
    {
        StartCoroutine(CallAsyncMethod());

    }
    IEnumerator CallAsyncMethod()
    {
        // Call your asynchronous method
        yield return MakeOpenAIRequestAsync();

        // Continue with other logic after the asynchronous method completes
        Debug.Log("Async method completed. Continue with other logic...");
    }
    /*
    * Configure this method to include speech to text in the system. So as to create a service which will run in the background 
    */
    async Task MakeOpenAIRequestAsync()
{
    ChatRequest requestData = new ChatRequest
    {
        model = "gpt-4-1106-preview",
        messages = new List<Message>
        {
            new Message { role = "system", content = "You are a helpful assistant." },
            new Message { role = "user", content = "tell me something about a car, only in 3 sentences" },
        }
    };

    // Convert request data to JSON
    string jsonData = JsonUtility.ToJson(requestData);
    Debug.Log(jsonData);
    speaker.Speak("waiting for G P T to reply");

    // Create and send the HTTP request
    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

        var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Parse and handle the response
                string responseJson = await response.Content.ReadAsStringAsync();
                ChatCompletionResponse chatResponse = JsonUtility.FromJson<ChatCompletionResponse>(responseJson);
                string assistantResponse = chatResponse.choices[0].message.content;
                Debug.Log("Assistant's Response: " + chatResponse.choices[0].message.content);
                speaker.Speak("i can speak");
            }
            else
            {
                Debug.LogError("OpenAI Request Failed. Status Code: " + response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.LogError("Exception during OpenAI request: " + ex.Message);
        }
    }
}

}

// For input parsing to Json utility
 [System.Serializable]
    public class ChatRequest
    {
        public string model;
        public List<Message> messages;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

// For output parsing from open ai response from chat 
[System.Serializable]
public class ChatCompletionResponse
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public Choice[] choices;
    public Usage usage;
}

[System.Serializable]
public class Choice
{
    public int index;
    public Message message;
    public string finish_reason;
}

[System.Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}