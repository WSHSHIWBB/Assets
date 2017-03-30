using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using VRTK;

public class AssemblyObject : MonoBehaviour
{
    private int _id;
    private ObjectsConfigModule objectConfigModule;
    private AssemblyObjectsModule assemblyObjectModule;
    private JsonAssemblyObject jsonAssemblyObject;

    private VRTK_InteractGrab _leftGrab;
    private VRTK_InteractGrab _rightGrab;

    private int OriginalPosindex = -1;
    

    private void Awake()
    {
        objectConfigModule = ModuleManager.Instance.Get<ObjectsConfigModule>();
        assemblyObjectModule = ModuleManager.Instance.Get<AssemblyObjectsModule>();

        _leftGrab = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_InteractGrab>();
        _rightGrab = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_InteractGrab>();
        string name = gameObject.name.Replace("(Clone)", "");
        _id = objectConfigModule.GetObjectInfoIDByName(name);
        assemblyObjectModule.RegisterLabObjectTransform(_id, transform);
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
        jsonAssemblyObject = assemblyObjectModule.GetJsonAssemblyObjectByID(_id);
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

    private JsonWorldTransform[] GetValidWorlTransforms()
    {
        return null;
    }

    private void Update()
    {

    }

    public int GetOriginalPosIndex()
    {
        return OriginalPosindex;
    }

    private void SetToOriginalPos()
    {
        if (jsonAssemblyObject.jsonWorldTransforms != null)
        {
            if (jsonAssemblyObject.jsonWorldTransforms.Length == 1)
            {
                transform.position = jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldPos.ToVector3();
                transform.eulerAngles = jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldRot.ToVector3();
                transform.localScale = jsonAssemblyObject.jsonWorldTransforms[0].JsonWorldScal.ToVector3();
            }
            else
            {
                AssemblyObject[] sameIDs=assemblyObjectModule.GetAssemblyOfSameID(_id, transform);
                if (sameIDs != null)
                {
                    List<int> posindexList = new List<int>();
                    for (int j = 0; j < jsonAssemblyObject.jsonWorldTransforms.Length; ++j)
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
                        JsonWorldTransform validJsonPOs = jsonAssemblyObject.jsonWorldTransforms[posindexList[0]];
                        transform.position = validJsonPOs.JsonWorldPos.ToVector3();
                        transform.eulerAngles = validJsonPOs.JsonWorldRot.ToVector3();
                        transform.localScale = validJsonPOs.JsonWorldScal.ToVector3();
                        OriginalPosindex = posindexList[0];
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
           
        }
    }
}
