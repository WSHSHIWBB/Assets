using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GroundObject go = collision.gameObject.GetComponent<GroundObject>();
        if(go!=null)
        {
            StartCoroutine(ResetToOriginal(go));
        }
    }

    private IEnumerator ResetToOriginal(GroundObject go)
    {
        yield return new WaitForSeconds(3);
        go.ResetToOriginal();
    }
}
