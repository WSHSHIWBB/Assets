using UnityEngine;
using VRFrameWork;
using System.Collections.Generic;
using System.Collections;
using System;

public class SetUpLabObjects : MonoBehaviour
{
    public const string LabObjectTag = "LabObject";
    private static int id = 0;
    private LabObjectsModule labObjectModule;
    public GameObject[] particles;

    public GameObject[] fires;

    public GameObject Iron;
    private Color IronStartColor=new Color(0.7f,0.7f,0.7f,1);
    private bool control = false;
    //public LabObject[] list;

    private void Awake()
    {
        labObjectModule = ModuleManager.Instance.Get<LabObjectsModule>();
        SetUpLabObject();
        //IgnoreCollision();
    }

    private void Start()
    {
        //list = GetComponentsInChildren<LabObject>();
        //StartCoroutine(CheckIsValid());
        //Iron.GetComponent<Renderer>().sharedMaterial.color= IronStartColor;
        //CheckIsValid();
    }

    private IEnumerator CheckIsValid()
    {
        while (true)
        {
            bool isValid = IsAllValid();
            if (isValid && !control)
            {
                ShowEffect(true);
            }
            else if (!isValid && control)
            {
                ShowEffect(false);
            }
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator ChangeIron()
    {
        Material mat= Iron.GetComponent<Renderer>().sharedMaterial;
        while(mat.color.r>0.2f)
        {
            yield return new WaitForSeconds(2);
            mat.color -= new Color(0.02f, 0.02f, 0.02f, 0);
        }
        
    }

    private void ShowEffect(bool isActive)
    {
        if (isActive)
        {
            for (int i = 0; i < particles.Length; ++i)
            {
                particles[i].SetActive(true);
                StartCoroutine("ChangeIron");
            }
            control = true;
        }
        else
        {
            for (int i = 0; i < particles.Length; ++i)
            {
                particles[i].SetActive(false);
                StopCoroutine("ChangeIron");
                Iron.GetComponent<Renderer>().sharedMaterial.color = IronStartColor;
            }
            control = false;
        }
    }

    private bool IsAllValid()
    {
        for (int j = 0; j < fires.Length; ++j)
        {
            if (fires[j].activeSelf)
                continue;
            else
                return false;
        }

        for (int i = 0; i < 6; ++i)
        {
            LabObject child = transform.GetChild(i).GetComponent<LabObject>();
            bool isValid = child.IsPosValid;
            if (child.ID == 15)
                continue;
            else
            {
                if (!child.IsPosValid)
                    return false;
            }
        }
        return true;

    }

    private void SetUpLabObject()
    {
        List<Transform> list_1 = new List<Transform>();
        List<Transform> list_2 = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            if (child.tag == LabObjectTag || child.GetComponent<LabObject>() != null)
            {
                list_1.Add(child);
            }
        }

        if (list_1.Count != 0)
        {
            LevelTraverse(list_1, list_2);
        }
    }

    private void LevelTraverse(List<Transform> list_1, List<Transform> list_2)
    {
        list_2.Clear();

        foreach (var element in list_1)
        {
            var labObject = element.GetOrAddComponent<LabObject>();
            labObject.ID = id;
            //labObjectModule.RegisterLabObjectTransform(id, element);
            ++id;
            for (int i = 0; i < element.childCount; ++i)
            {
                var child = element.GetChild(i);
                if (child.tag == LabObjectTag || child.GetComponent<LabObject>() != null)
                {
                    list_2.Add(child);
                }
            }
        }

        if (list_2.Count == 0)
        {
            return;
        }
        else
        {
            LevelTraverse(list_2, list_1);
        }

    }

    private void IgnoreCollision()
    {
        //Rigidbody[] rigids = transform.GetComponentsInChildren<Rigidbody>();
        /*
        foreach(var element in rigids)
        {
            element.isKinematic = true;
        }
        */
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length - 1; ++i)
        {
            for (int j = i + 1; j < colliders.Length; ++j)
            {
                if (colliders[i].name == "Particle")
                    continue;
                else
                {
                    if (colliders[i].name == "JiuJingDenGai" && colliders[j].name == "Particle")
                        continue;
                    else
                        Physics.IgnoreCollision(colliders[i], colliders[j]);
                }
            }
        }
    }

}
