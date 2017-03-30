using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;

public class MatchStickSpawnerAttachMechanic : VRTK_BaseGrabAttach
{
    VRTK_InteractTouch touch;
    VRTK_InteractGrab grab;
    protected override void Initialise()
    {
        tracked = false;
        climbable = false;
        kinematic = true;
    }

    public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        touch = grabbingObject.GetComponent<VRTK_InteractTouch>();
        grab = grabbingObject.GetComponent<VRTK_InteractGrab>();
        StartCoroutine(ForceGrab());
        return false;
    }

    public override void StopGrab(bool applyGrabbingObjectVelocity)
    {
        
    }
    
    private IEnumerator ForceGrab()
    {
        yield return null;
        if (touch && grab)
        {
            GameObject matchStick = Instantiate(Resources.Load<GameObject>("Prefabs/MatchStic"));
            touch.ForceStopTouching();
            grab.ForceRelease();
            touch.ForceTouch(matchStick);
            grab.AttemptGrab();
        }
    }
}
