using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundObject : MonoBehaviour
{
    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    public void ResetToOriginal()
    {
        List<GroundObject> childGroundObject = new List<GroundObject>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var go = transform.GetChild(i).GetComponent<GroundObject>();
            if(go!=null)
            {
                childGroundObject.Add(go);
            }
        }
        for(int i=0;i<childGroundObject.Count;++i)
        {
            childGroundObject[i].transform.parent = null;
        }

        transform.position = originalPos;
        transform.rotation = originalRot;
       for(int i=0;i<childGroundObject.Count;++i)
        {
            childGroundObject[i].transform.parent = transform;
        }
    }
}
