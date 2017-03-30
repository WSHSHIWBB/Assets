using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRFrameWork;

public class MatchBox: VRTK_InteractableObject
{
    private Animator animat;
    private VRTK_InteractableObject MatchStickSpawner;

    protected override void Awake()
    {
        base.Awake();

        animat = GetComponent<Animator>();
        MatchStickSpawner = transform.GetComponentByPath<VRTK_InteractableObject>("HuoChaiReal/huochai01/huochai02");
    }

    public override void StartTouching(GameObject currentTouchingObject)
    {
        base.StartTouching(currentTouchingObject);

    }

    public override void StopTouching(GameObject previousTouchingObject)
    {
        base.StopTouching(previousTouchingObject);
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);

        animat.SetBool("IsOpen", true);
        MatchStickSpawner.gameObject.SetActive(true);
    }

    

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);

        animat.SetBool("IsOpen", false);
        MatchStickSpawner.gameObject.SetActive(false);
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);

    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

    }
}
