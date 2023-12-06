using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObjScript : MonoBehaviour
{
    public string objFileName = "car_0"; // Specify the name of your .obj file without the extension

    void Start()
    {
        LoadObj();
    }

    void LoadObj()
    {
        // Load the .obj file from the "Resources" folder
        GameObject instance = Instantiate(Resources.Load("car_0", typeof(GameObject))) as GameObject;
        instance.transform.position = new Vector3(10, 0, 5);
        GameObject objPrefab = Resources.Load<GameObject>(objFileName);

        if (objPrefab != null)
        {
            // Instantiate the prefab
            GameObject objInstance = Instantiate(objPrefab, Vector3.zero, Quaternion.identity);
            objInstance.name = objFileName; // Optionally rename the instantiated GameObject
        }
        else
        {
            Debug.LogError("Failed to load .obj file. Make sure it is in the 'Resources' folder.");
        }
    }
}
