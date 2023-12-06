 using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Microsoft.MixedReality.Toolkit;

 public static class GestureUtils
    {
        private const float PinchThreshold = 0.7f;
        private const float GrabThreshold = 0.4f;
        private const float PoseThreshold = 0.2f;
        
        public static bool IsPinching(Handedness trackedHand)
        {
           // Debug.Log(HandPoseUtils.ThumbFingerCurl(trackedHand));
            return HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
        }

        public static bool IsGrabbing(Handedness trackedHand)
        {
            
            return !IsPinching(trackedHand) &&
                   HandPoseUtils.MiddleFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.RingFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.PinkyFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.ThumbFingerCurl(trackedHand) > GrabThreshold;
        }

//works well
        public static bool IsPoseDetected(Handedness trackedHand){
            
            return !IsGrabbing(trackedHand) && 
                   (HandPoseUtils.MiddleFingerCurl(trackedHand)  > GrabThreshold)&&  
                   (HandPoseUtils.RingFingerCurl(trackedHand)  > GrabThreshold)&&
                   (HandPoseUtils.PinkyFingerCurl(trackedHand)   > GrabThreshold)&&
                   (HandPoseUtils.ThumbFingerCurl(trackedHand)  > GrabThreshold);
        }

        public static bool IsOnePoseDetected(Handedness trackedHand){
           /* Debug.Log("index "+HandPoseUtils.IndexFingerCurl(trackedHand));
            Debug.Log("mid "+HandPoseUtils.MiddleFingerCurl(trackedHand));
            Debug.Log("ring "+HandPoseUtils.RingFingerCurl(trackedHand));
            Debug.Log("pnik "+HandPoseUtils.PinkyFingerCurl(trackedHand));
            Debug.Log("ithumb "+HandPoseUtils.ThumbFingerCurl(trackedHand));
            */
            
            return (HandPoseUtils.IndexFingerCurl(trackedHand) <= 0.03) &&
                   (HandPoseUtils.MiddleFingerCurl(trackedHand)  > GrabThreshold)&&  
                   (HandPoseUtils.RingFingerCurl(trackedHand)  > GrabThreshold)&&
                   (HandPoseUtils.PinkyFingerCurl(trackedHand)   > GrabThreshold)&&
                   (HandPoseUtils.ThumbFingerCurl(trackedHand)  > 0.5);
        }

         public static bool IsTwoPoseDetected(Handedness trackedHand){
            Debug.Log("index "+HandPoseUtils.IndexFingerCurl(trackedHand));
            Debug.Log("mid "+HandPoseUtils.MiddleFingerCurl(trackedHand));
            Debug.Log("ring "+HandPoseUtils.RingFingerCurl(trackedHand));
            Debug.Log("pnik "+HandPoseUtils.PinkyFingerCurl(trackedHand));
            Debug.Log("ithumb "+HandPoseUtils.ThumbFingerCurl(trackedHand));
            
            
            return (HandPoseUtils.IndexFingerCurl(trackedHand) <= 0.03) &&
                   (HandPoseUtils.MiddleFingerCurl(trackedHand)  <= GrabThreshold)&&  
                   (HandPoseUtils.RingFingerCurl(trackedHand)  > GrabThreshold)&&
                   (HandPoseUtils.PinkyFingerCurl(trackedHand)   > GrabThreshold)&&
                   (HandPoseUtils.ThumbFingerCurl(trackedHand)  > 0.5);
        }
    }