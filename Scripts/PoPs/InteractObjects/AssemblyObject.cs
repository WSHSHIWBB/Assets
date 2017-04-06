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

    private List<KeyValuePair<int, bool>> _ID_HasPos_List;

    private void Awake()
    {
        _objectConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        _assemblyObjectModule = ModuleManager.Instance.Get<AssemblyObjectsModule>();

        _leftGrab = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_InteractGrab>();
        _rightGrab = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_InteractGrab>();
        string name = gameObject.name.Replace("(Clone)", "");
        _id = _objectConfigModule.GetObjectInfoIDByName(name);
        _assemblyObjectModule.RegisterAssemblyObjectTransform(_id, transform);   
        _jsonAssemblyObject = _assemblyObjectModule.GetJsonAssemblyObjectByID(_id);
        InitialChildPos(out _ID_HasPos_List);
    }

    private void OnDestroy()
    {
        _assemblyObjectModule.UnRegisterAssemblyObjectTransform(_id,transform);
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
            PreProcessBeforeGrab();
        }
    }

    private void RightGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target == gameObject)
        {
            PreProcessBeforeGrab();
        }
    }

    private void LeftGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target == gameObject)
        {
            SetToValidPositon();
        }
    }

    private void RightGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null && e.target == gameObject)
        {
            SetToValidPositon();
        }
    }

    private void PreProcessBeforeGrab()
    {
        Transform parent = transform.parent;
        if(parent&&parent.GetComponent<AssemblyObject>())
        {
            parent.GetComponent<AssemblyObject>().ReleaseChildPos(_id, transform.localPosition);
        }
    }

    private void SetToValidPositon()
    {
        if (_jsonAssemblyObject.jsonWorldTransforms != null)
        {
            SetToValidWorldPosition();
        }
        if(_jsonAssemblyObject.parentIDs!=null)
        {
            SetToValidParentPosition();
        }
    }

    private void SetToValidWorldPosition()
    {
        if (_jsonAssemblyObject.jsonWorldTransforms == null)
        {
            Debug.LogError("The jsonWorldTransforms is null!");
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
                Transform[] brotherTrans = _assemblyObjectModule.GetTransformWithoutSelfByID(brotherJsonAssemblyObjs[j].ID, transform);
                if (brotherTrans != null)
                {
                    for (int k = 0; k < brotherTrans.Length; ++k)
                    {
                        JsonWorldTransform[] jsonBrotherPosArray = brotherJsonAssemblyObjs[j].jsonWorldTransforms;
                        for (int m = 0; m < jsonBrotherPosArray.Length; ++m)
                        {
                            Vector3 oldDir = jsonSelfPos - jsonBrotherPosArray[m].JsonWorldPos.ToVector3();
                            Quaternion oldBrotherQua = Quaternion.Euler(jsonBrotherPosArray[m].JsonWorldRot.ToVector3());
                            Quaternion brotherFixQUa = brotherTrans[k].rotation * Quaternion.Inverse(oldBrotherQua);
                            Vector3 newDir = brotherFixQUa * oldDir;
                            Vector3 relatePos = newDir.normalized * oldDir.magnitude + brotherTrans[k].position;
                            Quaternion relateQua = brotherFixQUa * jsonSelfQua;

                            if (Vector3.Distance(relatePos, transform.position) < 0.03f && !_assemblyObjectModule.IsSameIDInSamePos(_id, transform, relatePos))
                            {
                                transform.position = relatePos;
                                transform.rotation = relateQua;
                                transform.localScale = _jsonAssemblyObject.jsonWorldTransforms[i].JsonWorldScal.ToVector3();
                                rigidBody.isKinematic = true;
                                //to be
                                return;
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
        var rigidBody = transform.GetComponent<Rigidbody>();
        for (int i=0;i<_jsonAssemblyObject.parentIDs.Length;++i)
        {
            int parentID = _jsonAssemblyObject.parentIDs[i];
            JsonAssemblyObject jsonParent = _assemblyObjectModule.GetJsonAssemblyObjectByID(parentID);
            Transform[] parentTrans = _assemblyObjectModule.GetTransformWithoutSelfByID(parentID, transform);
            if(parentTrans==null)
            {
                return;
            }
            for(int j=0;j<parentTrans.Length;++j)
            {
                JsonChildPosInfo[] childPosInfos = jsonParent.jsonChildPosInfos;
                if(childPosInfos==null)
                {
                    Debug.LogError("The childPosInfo in null,This is Error!");
                    return;
                }
                for(int k=0;k<childPosInfos.Length;++k)
                {
                    if (childPosInfos[k].childID == _id && parentTrans[j].GetComponent<AssemblyObject>().IsHasChildPos(_id, k))
                    {
                        Vector3 validPos = parentTrans[j].TransformPoint(childPosInfos[k].JsonChildPos.ToVector3());
                        if (Vector3.Distance(validPos, transform.position) < 0.07f)
                        {
                            transform.parent = parentTrans[j];
                            transform.localPosition = childPosInfos[k].JsonChildPos.ToVector3();
                            transform.localEulerAngles = childPosInfos[k].JsonChildRot.ToVector3();
                            transform.localScale = childPosInfos[k].JsonChildScal.ToVector3();
                            parentTrans[j].GetComponent<AssemblyObject>().GetAChildPos(k, _id);
                            rigidBody.isKinematic = true;
                            return;
                        }
                    }
                }
            }
        }
        transform.parent = null;
        rigidBody.isKinematic = false;
    }

    public bool CompareID(int id)
    {
        if (id ==_id)
        {
            return true;
        }
        else
        {
            return false;
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

    public bool IsHasChildPos(int _id,int index)
    {
        if(index>_ID_HasPos_List.Count-1)
        {
            Debug.LogError("The index is illegal!");
            return false;
        }
        if(_ID_HasPos_List[index].Key!=_id)
        {
            Debug.LogError("The childID of index " + index + " id different form _id " + _id);
            return false;
        }
        return _ID_HasPos_List[index].Value;
    }

    public bool GetAChildPos(int index,int _id)
    {
        if(index>_ID_HasPos_List.Count-1||index<0)
        {
            Debug.LogError("Ilegal index!");
            return false;
        }
        if(_ID_HasPos_List[index].Key!=_id)
        {
            Debug.LogError("The _id is not same!");
            return false;
        }
        if(!_ID_HasPos_List[index].Value)
        {
            Debug.LogError("Dont have child Pos");
            return false;
        }
        _ID_HasPos_List[index] = new KeyValuePair<int, bool>(_ID_HasPos_List[index].Key, false);
        return true;
    }

    public bool ReleaseChildPos(int childID,Vector3 localPos)
    {
        if(_ID_HasPos_List==null)
        {
            Debug.LogError("The _ID_HasPos_List is null!");
            return false;
        }
        for(int i=0;i<_jsonAssemblyObject.jsonChildPosInfos.Length;++i)
        {
            if(_jsonAssemblyObject.jsonChildPosInfos[i].childID==childID
            &&Vector3.Distance(_jsonAssemblyObject.jsonChildPosInfos[i].JsonChildPos.ToVector3(),localPos)<0.00001f)
            {
                if(_ID_HasPos_List[i].Key!=childID||_ID_HasPos_List[i].Value!=false)
                {
                    Debug.LogError("Big Error!");
                    return true;
                }
                _ID_HasPos_List[i] = new KeyValuePair<int, bool>(_ID_HasPos_List[i].Key, true);
                return true;
            }
        }
        return false;
    }
}
