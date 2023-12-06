 using System;
 using UnityEngine;
 using Oculus.Voice;

 public class ColorChanger : MonoBehaviour
 {
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
        //rend = GetComponent<Renderer>();
     }

     void Update()
    {
            appVoiceExperience.Activate();
    }

     private void SetColor(Transform trans, Color color)
     {
         trans.GetComponent<Renderer>().material.color = color;
     }

     private void SetColor(Color color){
        //rend.material.color = color;
     }
     /// <summary>
     /// Updates the color of GameObject with the names specified in the input values.
     /// </summary>
     /// <param name="values"></param>
     public void UpdateColor(string[] values)
     {
        Debug.Log("CALLEDSSS");
         var colorString = values[0];
         Debug.Log("colorStirng "+colorString);
        // var shapeString = values[1];
        
         if (!ColorUtility.TryParseHtmlString(colorString, out var color)) return;
         foreach(var rend in GetComponentsInChildren<Renderer>(true))
        {
            rend.material.color = color;
        }
        //re
         //SetColor(color);
         //if (string.IsNullOrEmpty(shapeString)) return;

         /*foreach (Transform child in transform) // iterate through all children of the gameObject.
         {
             if (child.name.IndexOf(shapeString, StringComparison.OrdinalIgnoreCase) != -1) // if the name exists
             {
                 SetColor(child, color);
                 return;
             }
         }*/
     }

     
 }