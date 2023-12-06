using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.UI.BoundsControlTypes;


public class ScriptAddingTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        AddMeshCollider();
        AddObjectManipulator();
        AddBoundsControl();
    }

    private void AddMeshCollider()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            //Hole das Mesh aus dem MashFilter
            Mesh mesh = meshFilter.mesh;

            //F�ge den MeshCollider hinzu
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

            //Weise dem Mesh Collider das gleiche Mesh wie dem Mesh Filter zu
            meshCollider.sharedMesh = mesh;

            //Optional: Aktiviere oder deaktiviere die Convex-Eigenschaft des MeshCollider je nach Bedarf
             meshCollider.convex = true;
        }
    }

    private void AddObjectManipulator()
    {
        var AllTransforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var tr in AllTransforms)
        {
            tr.gameObject.AddComponent<ObjectManipulator>();
            ObjectManipulator handler = tr.gameObject.GetComponent<ObjectManipulator>();
            ManipulationEvent newevent = new ManipulationEvent();
            ManipulationEventData eventdata = new ManipulationEventData();
            eventdata.ManipulationSource = tr.gameObject;
            MakeNearDraggable(tr.gameObject);
        }



    }

    public void MakeNearDraggable(GameObject target)
    {
        // Instantiate and add grabbable
        target.AddComponent<NearInteractionGrabbable>();

        // Add ability to drag by re-parenting to pointer object on pointer down
        var pointerHandler = target.AddComponent<PointerHandler>();
        pointerHandler.OnPointerDown.AddListener((e) =>
        {
            if (e.Pointer is SpherePointer)
            {
                target.transform.parent = ((SpherePointer)(e.Pointer)).transform;
            }
        });
        pointerHandler.OnPointerUp.AddListener((e) =>
        {
            if (e.Pointer is SpherePointer)
            {
                target.transform.parent = null;
            }
        });
    }

    private void AddBoundsControl()
    {
        BoundsControl boundsControl;
        boundsControl = gameObject.AddComponent<BoundsControl>();

        // Change activation method
        boundsControl.BoundsControlActivation = BoundsControlActivationType.ActivateByProximityAndPointer;
        // Make the scale handles large
        boundsControl.ScaleHandlesConfig.HandleSize = 0.1f;
        // Hide rotation handles for x axis
        //boundsControl.RotationHandlesConfig.ShowRotationHandleForX = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}