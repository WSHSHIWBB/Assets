using System;
using UnityEngine;
using VRFrameWork;
using VRTK;
using System.Collections.Generic;
using System.Collections;

public class LabObject : MonoBehaviour
{
    public int ID=-1;
    public bool IsPosValid
    {
        get
        {
            //JudgeIsPosValid();
            return _isPosValid;
        }
    }
    private bool _isPosValid = true;
    private LabObjectsModule _labObjectModule;
    private JsonAssemblyObject _jsonLabObj;
    private VRTK_InteractGrab _leftGrab;
    private VRTK_InteractGrab _rightGrab;

    private void Awake()
    {
        _labObjectModule = ModuleManager.Instance.Get<LabObjectsModule>();
        _leftGrab = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_InteractGrab>();
        _rightGrab = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_InteractGrab>();
        //_jsonLabObj = _labObjectModule.GetJsonLabObj(ID);
    }

    public void Start()
    {
        
        //SetToOrigianlPos();
    }
    
    private void OnEnable()
    {
        if (_leftGrab)
        {
            _leftGrab.ControllerGrabInteractableObject += new ObjectInteractEventHandler(LeftGrabHandler);
            _leftGrab.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(LeftGrabRealseHandler);
        }
        if (_rightGrab)
        {
            _rightGrab.ControllerGrabInteractableObject += new ObjectInteractEventHandler(RightGrabHandler);
            _rightGrab.ControllerUngrabInteractableObject += new ObjectInteractEventHandler(RightGrabRealseHandler);
        }
    }

    private void OnDisable()
    {
        if (_leftGrab)
        {
            _leftGrab.ControllerGrabInteractableObject -= new ObjectInteractEventHandler(LeftGrabHandler);
            _leftGrab.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(LeftGrabRealseHandler);
        }
        if (_rightGrab)
        {
            _rightGrab.ControllerGrabInteractableObject -= new ObjectInteractEventHandler(RightGrabHandler);
            _rightGrab.ControllerUngrabInteractableObject -= new ObjectInteractEventHandler(RightGrabRealseHandler);
        }
    }

    private void LeftGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        LabObject labObject = e.target.GetComponent<LabObject>();
        if (labObject != null&&labObject.ID==ID)
        {
            ReleaseValidChildrenWhenGrab();
        }
    }

    private void RightGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            LabObject labObject = e.target.GetComponent<LabObject>();
            if (labObject != null && labObject.ID == ID)
            {
                ReleaseValidChildrenWhenGrab();
            }
        }
    }

    private void LeftGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            LabObject labObject = e.target.GetComponent<LabObject>();
            if (labObject != null && labObject.ID == ID)
            {
                SetToValidPos();
                StartCoroutine(AttachValidChildrenWhenUnGrab());
            }
        }
    }

    private void RightGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            LabObject labObject = e.target.GetComponent<LabObject>();
            if (labObject != null && labObject.ID == ID)
            {
                SetToValidPos();
                StartCoroutine(AttachValidChildrenWhenUnGrab());
            }
        }
    }

    private void ReleaseValidChildrenWhenGrab()
    {
        Transform[] childrenTrans = _labObjectModule.GetChildrenTransform(ID);
        if (childrenTrans != null)
        {
            for (int i = 0; i < childrenTrans.Length; ++i)
            {
                LabObject labObj = childrenTrans[i].GetComponent<LabObject>();
                if (!labObj.IsPosValid)
                {
                    childrenTrans[i].SetParent(null);
                }
            }
        }
    }

    private IEnumerator  AttachValidChildrenWhenUnGrab()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        while(rigid.velocity.magnitude>0.005f)
        {
            yield return null;
        }
        Transform[] childrenTrans = _labObjectModule.GetChildrenTransform(ID);
        if (childrenTrans != null)
        {
            for (int i = 0; i < childrenTrans.Length; ++i)
            {
               childrenTrans[i].SetParent(transform);
            }
        }
    }

    private void JudgeIsPosValid()
    {
        if (ID == 15)
        {
            _isPosValid = true;
            return;
        }
        Transform[] childrenTrans = _labObjectModule.GetChildrenTransform(ID);
        float dis = Vector3.Distance(transform.localPosition, _labObjectModule.GetJsonLabObj(ID).JsonLocalTransform.JsonLocalPos.ToVector3());
        if(_jsonLabObj.ParentID!=-1 && dis > 0.01f)
        {
            _isPosValid = false;
            return;
        }
        else
        {
            if(childrenTrans!=null)
            {
                for(int i=0;i<childrenTrans.Length;++i)
                {
                    LabObject labObj = childrenTrans[i].GetComponent<LabObject>();
                    if (labObj.IsPosValid)
                        continue;
                    else
                    {
                        _isPosValid = false;
                        return;
                    }
                }
                _isPosValid = true;
                return;
            }
            else
            {
                _isPosValid = true;
                return;
            }
        }
    }
        
    private void SetToValidPos()
    {
        var rigidBody = transform.GetComponent<Rigidbody>();
        JsonTransform[] validJsonTransforms = GetValidRelateJsonTrans();
        if(validJsonTransforms!=null)
        {
            float minimamDis =0.07f;
            int validInt = -1;
            for(int i=0;i<validJsonTransforms.Length;++i)
            {
                float dis = Vector3.Distance(transform.localPosition, validJsonTransforms[i].JsonLocalPos.ToVector3());
                if (dis<=minimamDis)
                {
                    minimamDis = dis;
                    validInt = i;
                }
            }
            if(validInt!=-1)
            {
                transform.localPosition = validJsonTransforms[validInt].JsonLocalPos.ToVector3();
                transform.localEulerAngles = validJsonTransforms[validInt].JsonLocalRot.ToVector3();
                rigidBody.isKinematic = true;
            }
            else
            {
                rigidBody.isKinematic = false;
            }
        }
        else
        {
            rigidBody.isKinematic = false;
        }
    }

    private JsonTransform[] GetValidRelateJsonTrans()
    {
        Vector3 selfLocalPos = _jsonLabObj.JsonLocalTransform.JsonLocalPos.ToVector3();
        Quaternion selfQuat = Quaternion.Euler(_jsonLabObj.JsonLocalTransform.JsonLocalRot.ToVector3());
        JsonAssemblyObject[] brothersJsonLabObjs = _labObjectModule.GetBrothersJsonLabObj(ID);
        List<JsonTransform> RelateTrans_List = new List<JsonTransform>();
        if(_jsonLabObj.ParentID!=-1)
        {
            RelateTrans_List.Add(_jsonLabObj.JsonLocalTransform);
        }
        if (brothersJsonLabObjs != null)
        {
            for (int i = 0; i < brothersJsonLabObjs.Length; ++i)
            {
                int brotherID = brothersJsonLabObjs[i].ID;
                Transform brotherTrans = _labObjectModule.GetTransformByID(brotherID);
                Vector3 oldDir = selfLocalPos - brothersJsonLabObjs[i].JsonLocalTransform.JsonLocalPos.ToVector3();
                Quaternion oldQuat = Quaternion.Euler(brothersJsonLabObjs[i].JsonLocalTransform.JsonLocalRot.ToVector3());
                Quaternion fixedQuat = brotherTrans.localRotation * Quaternion.Inverse(oldQuat);
                Vector3 newDir = fixedQuat * oldDir;

                Vector3 relatePos = newDir.normalized * oldDir.magnitude+brotherTrans.localPosition;
                Quaternion relateQua = fixedQuat * selfQuat;

                JsonTransform relateJosonTrans = new JsonTransform(relatePos, relateQua.eulerAngles, _jsonLabObj.JsonLocalTransform.JsonLocalScal.ToVector3());
                RelateTrans_List.Add(relateJosonTrans);
            }
        }
        if (RelateTrans_List.Count <= 0)
        {
            return null;
        }
        else
        {   
            return RelateTrans_List.ToArray();
        }

    }

    public void  SetToOrigianlPos()
    {
        Transform parent = _labObjectModule.GetParentTransform(ID);
        if(parent!=null)
        {
            transform.SetParent(parent);
        }
        transform.localPosition = _jsonLabObj.JsonLocalTransform.JsonLocalPos.ToVector3();
        transform.localEulerAngles = _jsonLabObj.JsonLocalTransform.JsonLocalRot.ToVector3();
        transform.localScale = _jsonLabObj.JsonLocalTransform.JsonLocalScal.ToVector3();
    }

    public bool IsAssemCompletely()
    {
        Vector3 selfWorldPos = _jsonLabObj.JsonLocalTransform.JsonWorldPos.ToVector3();
        Vector3 originalWorldPos = _labObjectModule.GetJsonLabObj(0).JsonLocalTransform.JsonWorldPos.ToVector3();
        Transform originalTrans = _labObjectModule.GetTransformByID(0);
        float oldLength = (selfWorldPos - originalWorldPos).magnitude;
        float newLength = (transform.position - originalTrans.position).magnitude;
        if(Mathf.Abs(oldLength-newLength)<0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
}
