using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using VRTK;

public class GridItem : VRTK_InteractableObject
{
    [HideInInspector]
    public int Index;
    [HideInInspector]
    public Transform TipsTrans;

    private ObjectsConfigModule _objectsConfigModule;
    private BagModule _bagModule;
    private ObjectInfo _objectInfo;

    protected override void Awake()
    {
        base.Awake();
        _objectsConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        _bagModule = ModuleManager.Instance.Get<BagModule>();
        
    }

    private void Start()
    {
        int id = _bagModule.GetIDByIndex(Index);
        if (id != -1)
        {
            _objectInfo = _objectsConfigModule.GetObjectInfoByID(id);
            string Name = _objectInfo.EnglishName;
            GameObject gridItem3D = Instantiate(Resources.Load<GameObject>("Prefabs/3DUIObject/" + Name), transform, false);
            if (gridItem3D == null)
            {
                Debug.LogError("Can't find gridItem3D at parth Prefabs/3DUIObject/" + Name);
                return;
            }
        }
        OnScrollMove(0);
    }

    public void OnScrollMove(int currentPage)
    {
        
        if(transform.childCount>1)
        {
            Transform GridItem3D = transform.GetChild(1);
            if (Index>=currentPage*9&&Index<(currentPage+1)*9)
            {
                GridItem3D.gameObject.SetActive(true);
            }
            else
            {
                GridItem3D.gameObject.SetActive(false);
            }
        }
    }

    public override void StartTouching(GameObject currentTouchingObject)
    {
        base.StartTouching(currentTouchingObject);
        ShowTips(true);
    }

    public override void StopTouching(GameObject previousTouchingObject)
    {
        base.StopTouching(previousTouchingObject);
        ShowTips(false);
    }

    public override void Grabbed(GameObject currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);
        VRTK_InteractTouch touch = currentGrabbingObject.GetComponent<VRTK_InteractTouch>();
        VRTK_InteractGrab grab = currentGrabbingObject.GetComponent<VRTK_InteractGrab>();
        //StartCoroutine(ForceGrab(touch, grab));
        ForceGrab(touch, grab);
    }

    private void  ForceGrab(VRTK_InteractTouch touch, VRTK_InteractGrab grab)
    {
        //yield return null;
        if (touch && grab)
        {
            string Name = _objectInfo.EnglishName;
            GameObject prefab = Instantiate(Resources.Load<GameObject>("Prefabs/LabObjects/"+Name));
            grab.ForceRelease();
            touch.ForceStopTouching();
            touch.ForceTouch(prefab);
            grab.AttemptGrab();
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        
    }

    private void ShowTips(bool isShow)
    {
        if(!TipsTrans)
        {
            Debug.Log("Tips is null!");
            return;
        }
        if(transform.childCount<2)
        {
            return;
        }
        if(_objectInfo==null)
        {
            Debug.Log("ObjectInfo is null!");
        }
        if (isShow)
        {
            TipsTrans.gameObject.SetActive(true);
            Tips tips = TipsTrans.GetComponent<Tips>();
            string describ = _objectInfo.Description;
            describ = System.Text.RegularExpressions.Regex.Replace(describ, @"(\w{6})","$0\n");
            tips.SetTips(transform.position,describ, Index % 9);
        }
        else
        {
            TipsTrans.gameObject.SetActive(false);
        }
    }



}
