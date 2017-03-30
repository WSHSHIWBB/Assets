using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OculusGrabGestureManager : MonoBehaviour
{
    private VRTK_InteractGrab _interactGrab;
    private static OvrAvatar _ovrAvatar;

    void Awake()
    {
        _interactGrab = GetComponent<VRTK_InteractGrab>();
        _ovrAvatar = GameObject.Find("LocalAvatar").GetComponent<OvrAvatar>();
    }

    private void OnEnable()
    {
        if (VRTK_DeviceFinder.GetControllerHand(gameObject) == SDK_BaseController.ControllerHand.Left)
        {
            _interactGrab.ControllerGrabInteractableObject += new ObjectInteractEventHandler(OnLeftGrabHandler);
            _interactGrab.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(OnLeftReleaseHandler);
        }
        else if (VRTK_DeviceFinder.GetControllerHand(gameObject) == SDK_BaseController.ControllerHand.Right)
        {
            _interactGrab.ControllerGrabInteractableObject += new ObjectInteractEventHandler(OnRightGrabHandler);
            _interactGrab.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(OnRightReleaseHandler);
        }
    }

    private void OnDisable()
    {
        if (VRTK_DeviceFinder.GetControllerHand(gameObject) == SDK_BaseController.ControllerHand.Left)
        {
            _interactGrab.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(OnLeftGrabHandler);
            _interactGrab.ControllerGrabInteractableObject -= new ObjectInteractEventHandler(OnLeftReleaseHandler);
        }
        else if (VRTK_DeviceFinder.GetControllerHand(gameObject) == SDK_BaseController.ControllerHand.Right)
        {
            _interactGrab.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(OnRightGrabHandler);
            _interactGrab.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(OnRightReleaseHandler);
        }
    }

    private void OnLeftGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target!=null)
        {
            string name = e.target.name;
            GameObject GrabPos = (GameObject)ObjPoolManager.Instance.Get("OculusGestures/Left/" + name + "L", false);
            if (GrabPos != null)
            {
                _ovrAvatar.LeftHandCustomPose = GrabPos.transform;
            }
        }
    }

    private void OnRightGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            string name = e.target.name;
            GameObject GrabPos = (GameObject)ObjPoolManager.Instance.Get("OculusGestures/Right/" + name + "R", false);
            if (GrabPos != null)
            {
                _ovrAvatar.RightHandCustomPose = GrabPos.transform;
            }
        }
    }

    private void OnLeftReleaseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            if (_ovrAvatar.LeftHandCustomPose != null)
            {
                ObjPoolManager.Instance.Set(_ovrAvatar.LeftHandCustomPose.gameObject);
                _ovrAvatar.LeftHandCustomPose = null;
            }
        }
    }

    private void OnRightReleaseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            if (_ovrAvatar.RightHandCustomPose != null)
            {
                ObjPoolManager.Instance.Set(_ovrAvatar.RightHandCustomPose.gameObject);
                _ovrAvatar.RightHandCustomPose = null;
            }
        }
    }

    public static void ForeGrabGesture(GameObject grabbingObject, GameObject grabbedObject)
    {
        string name = grabbedObject.name;
        if (VRTK_DeviceFinder.IsControllerLeftHand(grabbingObject))
        {
            GameObject GrabPos = (GameObject)ObjPoolManager.Instance.Get("OculusGestures/Left/" + name + "L", false);
            if (GrabPos != null)
            {
                _ovrAvatar.LeftHandCustomPose = GrabPos.transform;
            }
        }
        else if(VRTK_DeviceFinder.IsControllerRightHand(grabbingObject))
        {
            GameObject GrabPos = (GameObject)ObjPoolManager.Instance.Get("OculusGestures/Right/" + name + "R", false);
            if (GrabPos != null)
            {
                _ovrAvatar.RightHandCustomPose = GrabPos.transform;
            }
        }
    }
}
