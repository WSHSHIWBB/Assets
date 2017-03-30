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

    private int _originalPosindex = -1;
    private List<KeyValuePair<int, bool>> _ID_HasPos_List;

    private void Awake()
    {
        _objectConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        _assemblyObjectModule = ModuleManager.Instance.Get<AssemblyObjectsModule>();

        _leftGrab = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_InteractGrab>();
        _rightGrab = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_InteractGrab>();
        string name = gameObject.name.Replace("(Clone)", "");
        _id = _objectConfigModule.GetObjectInfoIDByName(name);
        _assemblyObjectModule.RegisterLabObjectTransform(_id, transform);
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
        _jsonAssemblyObject = _assemblyObjectModule.GetJsonAssemblyObjectByID(_id);
        InitialChildPos(_jsonAssemblyObject, out _ID_HasPos_List);
        SetToOriginalPos();
    }

    private void LeftGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            AssemblyObject assemObject = e.target.GetComponent<AssemblyObject>();
            if (assemObject != null && assemObject.CompareID(assemObject._id))
            {

            }
        }
    }

    private void RightGrabHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            AssemblyObject assemObject = e.target.GetComponent<AssemblyObject>();
            if (assemObject != null && assemObject.CompareID(assemObject._id))
            {

            }
        }
    }

    private void LeftGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            AssemblyObject assemObject = e.target.GetComponent<AssemblyObject>();
            if (assemObject != null && assemObject.CompareID(assemObject._id))
            {

            }
        }
    }

    private void RightGrabRealseHandler(object sender, ObjectInteractEventArgs e)
    {
        if (e.target != null)
        {
            AssemblyObject assemObject = e.target.GetComponent<AssemblyObject>();
            if (assemObject != null && assemObject.CompareID(assemObject._id))
            {

            }
        }
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

    private void Update()
    {

    }

    public int GetOriginalPosIndex()
    {
        return _originalPosindex;
    }

    private void InitialChildPos(JsonAssemblyObject jsonAssemblyObject, out List<KeyValuePair<int, bool>> id_hasPos_List)
    {
        if (jsonAssemblyObject.jsonChildPosInfos != null)
        {
            id_hasPos_List = new List<KeyValuePair<int, bool>>();
            for (int i = 0; i < jsonAssemblyObject.jsonChildPosInfos.Length; ++i)
            {
                int childID = jsonAssemblyObject.jsonChildPosInfos[i].childID;
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
        Debug.Log(_id);
        if (_ID_HasPos_List == null)
        {
            Debug.Log(111);
            return false;
        }
        List<KeyValuePair<int, bool>> validList =
        _ID_HasPos_List.FindAll((KeyValuePair<int, bool> pair) => { return (pair.Key == childID && pair.Value); });
        if (validList.Count == 0)
        {
            Debug.Log(222);
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

    private void SetToOriginalPos()
    {
        if (_jsonAssemblyObject.parentIDs == null)
        {
            if (_jsonAssemblyObject.jsonWorldTransforms.Length == 1)
            {
                transform.position = _jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldPos.ToVector3();
                transform.eulerAngles = _jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldRot.ToVector3();
                transform.localScale = _jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldScal.ToVector3();
            }
            else
            {
                AssemblyObject[] sameIDs = _assemblyObjectModule.GetAssemblyOfSameID(_id, transform);
                if (sameIDs != null)
                {
                    List<int> posindexList = new List<int>();
                    for (int j = 0; j < _jsonAssemblyObject.jsonWorldTransforms.Length; ++j)
                    {
                        posindexList.Add(j);
                    }
                    for (int i = 0; i < sameIDs.Length; ++i)
                    {
                        if (sameIDs[i].GetOriginalPosIndex() != -1)
                        {
                            posindexList.Remove(sameIDs[i].GetOriginalPosIndex());
                        }
                    }

                    if (posindexList.Count > 0)
                    {
                        JsonWorldTransform validJsonPOs = _jsonAssemblyObject.jsonWorldTransforms[posindexList[0]];
                        transform.position = validJsonPOs.JsonWorldPos.ToVector3();
                        transform.eulerAngles = validJsonPOs.JsonWorldRot.ToVector3();
                        transform.localScale = validJsonPOs.JsonWorldScal.ToVector3();
                        _originalPosindex = posindexList[0];
                    }
                }
                else
                {
                    Debug.LogError("Some thing wrong");
                }
            }
        }
        else
        {
            Transform parent = _assemblyObjectModule.GetOneHasPosParentTransform(_id);
            if (parent != null)
            {
                JsonChildPosInfo jsonChildPosInfo = parent.GetComponent<AssemblyObject>().GetAChildPos(_id);
                transform.SetParent(parent);
                transform.localPosition = jsonChildPosInfo.JsonChildPos.ToVector3();
                transform.localEulerAngles = jsonChildPosInfo.JsonChildRot.ToVector3();
                transform.localScale = jsonChildPosInfo.JsonChildScal.ToVector3();
            }
            else
            {
                Debug.LogError("SomeThing Wrong");
            }
        }
    }
}
