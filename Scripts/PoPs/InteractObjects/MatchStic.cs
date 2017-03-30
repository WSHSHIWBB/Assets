using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MatchStic : VRTK_InteractableObject
{
    protected override void Awake()
    {
        base.Awake();
  
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
      
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = false;
        StartCoroutine(DestoryDelay(3f));
    }

    private IEnumerator DestoryDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyImmediate(gameObject);
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
