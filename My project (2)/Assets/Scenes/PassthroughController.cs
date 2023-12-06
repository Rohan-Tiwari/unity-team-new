using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Microsoft.MixedReality.Toolkit;

public class PassthroughController : MonoBehaviour
{
 
 public GameObject sphere;
 public GameObject capsule;

 [SerializeField]
        private TrackedHandJoint trackedHandJoint = TrackedHandJoint.IndexMiddleJoint;

        [SerializeField]
        private float grabDistance = 0.1f;

        [SerializeField]
        private Handedness trackedHand = Handedness.Both;

        [SerializeField]
        private bool trackPinch = true;

        [SerializeField]
        private bool trackGrab = true;

        private IMixedRealityHandJointService handJointService;

        private IMixedRealityHandJointService HandJointService =>
            handJointService ??
            (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

        private MixedRealityPose? previousLeftHandPose;

        private MixedRealityPose? previousRightHandPose;

        private void Update()
        {
            var leftHandPose = GetHandPose(Handedness.Left, previousLeftHandPose != null);
            var rightHandPose = GetHandPose(Handedness.Right, previousRightHandPose != null);
            {
                var jointTransform = HandJointService.RequestJointTransform(trackedHandJoint, trackedHand);
                if (rightHandPose != null && previousRightHandPose != null)
                {
                    if (leftHandPose != null && previousLeftHandPose != null)
                    {
                        // fight! pick the closest one
                        var isRightCloser = Vector3.Distance(rightHandPose.Value.Position, jointTransform.position) <
                                            Vector3.Distance(leftHandPose.Value.Position, jointTransform.position);

                        ProcessPoseChange(
                            isRightCloser ? previousRightHandPose : previousLeftHandPose,
                            isRightCloser ? rightHandPose : leftHandPose);
                    }
                    else
                    {
                        ProcessPoseChange(previousRightHandPose, rightHandPose);
                    }
                }
                else if (leftHandPose != null && previousLeftHandPose != null)
                {
                    ProcessPoseChange(previousLeftHandPose, leftHandPose);
                }
            }
            previousLeftHandPose = leftHandPose;
            previousRightHandPose = rightHandPose;
        }

        private MixedRealityPose? GetHandPose(Handedness hand, bool hasBeenGrabbed)
        {
            Debug.Log("In gethand pose");
            if ((trackedHand & hand) == hand)
            {
                
                if (HandJointService.IsHandTracked(hand) &&
                    ((GestureUtils.IsPinching(hand) && trackPinch) ||
                     (GestureUtils.IsGrabbing(hand) && trackGrab)))
                {
                    Debug.Log("grabing andpinch");
                    var jointTransform = HandJointService.RequestJointTransform(trackedHandJoint, hand);
                    var palmTransForm = HandJointService.RequestJointTransform(TrackedHandJoint.Palm, hand);
                    //sphere.SetActive(true);

                    if(hasBeenGrabbed || 
                       Vector3.Distance(gameObject.transform.position, jointTransform.position) <= grabDistance)
                    {
                        return new MixedRealityPose(jointTransform.position, palmTransForm.rotation);
                    }
                }
                if(HandJointService.IsHandTracked(hand) && GestureUtils.IsPoseDetected(hand)){
                     //Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                     Debug.Log("Pose detected");
                     //sphere.SetActive(true); Uncomment this to view the sphere by default
                     //GameObject meshObject = Resources.Load<GameObject>("car_0.obj");
                     //meshObject.transform.Translate(0, 0, 0);
                }
                if(HandJointService.IsHandTracked(hand) && GestureUtils.IsOnePoseDetected(hand)){
                     //Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                     Debug.Log("OnePose Found");
                     capsule.SetActive(true);
                     //GameObject meshObject = Resources.Load<GameObject>("car_0.obj");
                     //meshObject.transform.Translate(0, 0, 0);
                }
                if(HandJointService.IsHandTracked(hand) && GestureUtils.IsTwoPoseDetected(hand)){
                    sphere.SetActive(true);
                }
            }

            return null;
        }
        
        private void ProcessPoseChange(MixedRealityPose? previousPose, MixedRealityPose? currentPose)
        {
            var delta = currentPose.Value.Position - previousPose.Value.Position;
            var deltaRotation = Quaternion.FromToRotation(previousPose.Value.Forward, currentPose.Value.Forward);
            gameObject.transform.position += delta;
            gameObject.transform.rotation *= deltaRotation;
        }
}
