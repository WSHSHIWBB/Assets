using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using VRTK;

public class AssemblyObject : MonoBehaviour
{
    private int _id;
    private ObjectsConfigModule _objectConfigModule;
    private AssemblyObjectsModule _assemblyObjectModule;
    private JsonAssemblyObject _jsonAssemblyObject;

    private VRTK_InteractGrab _leftGrab;
    private VRTK_InteractGrab _rightGrab;

    private List<bool> _Index_IsNotUsed_List;
    private List<KeyValuePair<int, bool>> _ID_HasPos_List;

    private void Awake()
    {
        _objectConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        _assemblyObjectModule = ModuleManager.Instance.Get<AssemblyObjectsModule>();

        _leftGrab = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_InteractGrab>();
        _rightGrab = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_InteractGrab>();
        string name = gameObject.name.Replace("(Clone)", "");
        _id = _objectConfigModule.GetObjectInfoIDByName(name);
        _assemblyObjectModule.RegisterLabObjectTransform(_id, transform);      //Register
        _jsonAssemblyObject = _assemblyObjectModule.GetJsonAssemblyObjectByID(_id);
        InitialWorldPos(out _Index_IsNotUsed_List);
        InitialChildPos(out _ID_HasPos_List);
    }

    private void OnDestroy()
    {
        //to be                                                                //UnRegister
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

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void LeftGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target == gameObject)
        {
            //to be Set _Index_IsUsed_List and _ID_HasPos_List
            ResetWorldPosNotUsedList();
        }
    }

    private void RightGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target == gameObject)
        {
            //to be Set _Index_IsUsed_List and _ID_HasPos_List
            ResetWorldPosNotUsedList();

        }
    }

    private void LeftGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target==gameObject)
        {
            SetToValidPositon();
        }
    }

    private void RightGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target==gameObject)
        {
            SetToValidPositon();
        }
    }

    private void SetToValidPositon()
    {
        if (_jsonAssemblyObject.parentIDs == null)
        {
            SetToValidWorldPosition();
        }
        else
        {
            SetToValidParentPosition();
        }
    }

    private void SetToValidWorldPosition()
    {
        if (_Index_IsNotUsed_List == null)
        {
            Debug.LogError("The _Index_IsNotUsed_List is null!");
            return;
        }
        var rigidBody = transform.GetComponent<Rigidbody>();
        for (int i = 0; i < _jsonAssemblyObject.jsonWorldTransforms.Length; ++i)
        {
            Vector3 jsonSelfPos = _jsonAssemblyObject.jsonWorldTransforms[i].JsonWorldPos.ToVector3();
            Quaternion jsonSelfQua = Quaternion.Euler(_jsonAssemblyObject.jsonWorldTransforms[i].JsonWorldRot.ToVector3());
            JsonAssemblyObject[] brotherJsonAssemblyObjs = _assemblyObjectModule.GetAllWorldPosBrotherJsonAssemblyObj(_id);
            for (int j = 0; j < brotherJsonAssemblyObjs.Length; ++j)
            {
                Transform[] brotherTrans = _assemblyObjectModule.GetTransformWithoutSelfByID(brotherJsonAssemblyObjs[j].ID,transform);
                if (brotherTrans != null)
                {
                    for (int k = 0; k < brotherTrans.Length; ++k)
                    {
                        JsonWorldTransform[] jsonBrotherPosArray = brotherJsonAssemblyObjs[j].jsonWorldTransforms;
                        for (int m = 0; m < jsonBrotherPosArray.Length; ++m)
                        {
                            if (brotherTrans[k].GetComponent<AssemblyObject>().IsWorldPosNotUsed(m))
                            {
                                Vector3 oldDir = jsonSelfPos - jsonBrotherPosArray[m].JsonWorldPos.ToVector3();
                                Quaternion oldBrotherQua = Quaternion.Euler(jsonBrotherPosArray[m].JsonWorldRot.ToVector3());
                                Quaternion brotherFixQUa = brotherTrans[k].rotation * Quaternion.Inverse(oldBrotherQua);
                                Vector3 newDir = brotherFixQUa * oldDir;
                                Vector3 relatePos = newDir.normalized * oldDir.magnitude + brotherTrans[k].position;
                                Quaternion relateQua = brotherFixQUa * jsonSelfQua;

                                if (Vector3.Distance(relatePos, transform.position) < 0.1f)
                                {
                                    UseWorldPos(i);
                                    brotherTrans[k].GetComponent<AssemblyObject>().UseWorldPos(m);
                                    transform.position = relatePos;
                                    transform.rotation = relateQua;
                                    transform.localScale = jsonBrotherPosArray[m].JsonWorldScal.ToVector3();
                                    rigidBody.isKinematic = true;
                                    Debug.LogError("Function ok!!!!!!!!!!!");
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        rigidBody.isKinematic = false;
    }

    private void SetToValidParentPosition()
    {

    }

    public bool CompareID(int id)
    {
        if (id == this._id)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InitialWorldPos(out List<bool> list)
    {
        if (_jsonAssemblyObject.jsonWorldTransforms != null)
        {
            list = new List<bool>();
            for (int i = 0; i < _jsonAssemblyObject.jsonWorldTransforms.Length; ++i)
            {
                list.Add(true);
            }
        }
        else
        {
            list = null;
        }
    }

    public bool IsWorldPosNotUsed(int index)
    {
        if (_Index_IsNotUsed_List == null)
        {
            return false;
        }
        if (index > _Index_IsNotUsed_List.Count || index < 0)
        {
            Debug.LogError("The index is illegal");
            return false;
        }
        return _Index_IsNotUsed_List[index];
    }

    public bool UseWorldPos(int index)
    {
        if (_Index_IsNotUsed_List == null)
        {
            Debug.LogError("The _Index_IsNotUsed_List is null,you can't use this function!");
            return false;
        }
        if (index > _Index_IsNotUsed_List.Count || index < 0)
        {
            Debug.LogError("The index is illegal");
            return false;
        }
        _Index_IsNotUsed_List[index] = false;
        return true;
    }

    public void ResetWorldPosNotUsedList()
    {
        if (_Index_IsNotUsed_List == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < _Index_IsNotUsed_List.Count; ++i)
            {
                _Index_IsNotUsed_List[i] = true;
            }
        }
    }

    private void InitialChildPos(out List<KeyValuePair<int, bool>> id_hasPos_List)
    {
        if (_jsonAssemblyObject.jsonChildPosInfos != null)
        {
            id_hasPos_List = new List<KeyValuePair<int, bool>>();
            for (int i = 0; i < _jsonAssemblyObject.jsonChildPosInfos.Length; ++i)
            {
                int childID = _jsonAssemblyObject.jsonChildPosInfos[i].childID;
                id_hasPos_List.Add(new KeyValuePair<int, bool>(childID, true));
            }
        }
        else
        {
            id_hasPos_List = null;
        }
    }

    public bool IsHasChildPos(int childID)
    {
        if (_ID_HasPos_List == null)
        {
            return false;
        }
        List<KeyValuePair<int, bool>> validList =
        _ID_HasPos_List.FindAll((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && pair.Value); });
        if (validList.Count == 0)
        {
            return false;
        }
        return true;
    }

    public JsonChildPosInfo GetAChildPos(int childID)
    {
        if (IsHasChildPos(childID))
        {
            int index = -1;
            if (RandomUtil.Bool())
            {
                index = _ID_HasPos_List.FindIndex((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && pair.Value); });
            }
            else
            {
                index = _ID_HasPos_List.FindLastIndex((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && pair.Value); });
            }
            if (index == -1)
            {
                return null;
            }
            else
            {
                _ID_HasPos_List[index] = new KeyValuePair<int, bool>(_ID_HasPos_List[index].Key, false);
                return _jsonAssemblyObject.jsonChildPosInfos[index];
            }
        }
        else
        {
            return null;
        }
    }

    public bool ReleaseAChildPos(int childID)
    {
        if (IsHasChildPos(childID))
        {
            int index = -1;
            if (RandomUtil.Bool())
            {
                index = _ID_HasPos_List.FindIndex((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && !pair.Value); });
            }
            else
            {
                index = _ID_HasPos_List.FindLastIndex((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && !pair.Value); });
            }
            if (index == -1)
            {
                return false;
            }
            else
            {
                _ID_HasPos_List[index] = new KeyValuePair<int, bool>(_ID_HasPos_List[index].Key, true);
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
