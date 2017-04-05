using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.GrabAttachMechanics;
using VRTK;

public class GridItemGrabSpawnerAttachMechanic : VRTK_BaseGrabAttach
{
    
    protected override void Initialise()
    {
        tracked = false;
        climbable = false;
        kinematic = true;
    }

    public override bool StartGrab(GameObject grabbingObject, GameObject givenGrabbedObject, Rigidbody givenControllerAttachPoint)
    {
        return true;
    }

    public override void StopGrab(bool applyGrabbingObjectVelocity)
    {
        
    }

}
