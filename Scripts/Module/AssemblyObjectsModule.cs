﻿using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using System.IO;
using LitJson;
using System;


public class JsonVector3
{
    public double x;
    public double y;
    public double z;

    public JsonVector3()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    public JsonVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public JsonVector3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public JsonVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }
}

public class JsonWorldTransform
{
    public JsonVector3 JsonWorldPos;
    public JsonVector3 JsonWorldRot;
    public JsonVector3 JsonWorldScal;
    public JsonWorldTransform()
    {
        JsonWorldPos = new JsonVector3();
        JsonWorldRot = new JsonVector3();
        JsonWorldScal = new JsonVector3();
    }

    public JsonWorldTransform(Vector3 Pos, Vector3 rot, Vector3 scal)
    {
        JsonWorldPos = new JsonVector3(Pos);
        JsonWorldRot = new JsonVector3(rot);
        JsonWorldScal = new JsonVector3(scal);
    }

    public JsonWorldTransform(Transform transform)
    {
        JsonWorldPos = new JsonVector3(transform.position);
        JsonWorldRot = new JsonVector3(transform.rotation.eulerAngles);
        JsonWorldScal = new JsonVector3(transform.lossyScale);
    }
}

public class JsonChildPosInfo
{
    public int childID;
    public JsonVector3 JsonChildPos;
    public JsonVector3 JsonChildRot;
    public JsonVector3 JsonChildScal;

    public JsonChildPosInfo()
    {
        childID = -1;
        JsonChildPos = new JsonVector3();
        JsonChildRot = new JsonVector3();
        JsonChildScal = new JsonVector3();
    }

    public JsonChildPosInfo(int childID, Vector3 pos, Vector3 rot, Vector3 scal)
    {
        this.childID = childID;
        JsonChildPos = new JsonVector3(pos);
        JsonChildRot = new JsonVector3(rot);
        JsonChildScal = new JsonVector3(scal);
    }

    public JsonChildPosInfo(int childID, Transform transform)
    {
        this.childID = childID;
        JsonChildPos = new JsonVector3(transform.localPosition);
        JsonChildRot = new JsonVector3(transform.localEulerAngles);
        JsonChildScal = new JsonVector3(transform.localScale);
    }
}

public class JsonAssemblyObject
{
    public int ID;
    public string Name;
    public JsonWorldTransform[] jsonWorldTransforms;
    public JsonChildPosInfo[] jsonChildPosInfos;
    public int[] parentIDs;

    public JsonAssemblyObject()
    {
        ID = -1;
        Name = null;
        jsonWorldTransforms = null;
        jsonChildPosInfos = null;
        parentIDs = null;
    }
}

public class AssemblyObjectsModule : BaseModule
{
    private const string LABOBJFILE = "AssemblyObjectJsonFile";

    private Dictionary<int, JsonAssemblyObject> _ID_JsonAssembyObject_Dic = new Dictionary<int, JsonAssemblyObject>();
    public Dictionary<int, List<Transform>> _ID_TransformList_Dic = new Dictionary<int, List<Transform>>();

    private void SortDic(Dictionary<int, JsonAssemblyObject> dic)
    {
        if (dic.Count > 0)
        {
            List<KeyValuePair<int, JsonAssemblyObject>> list = new List<KeyValuePair<int, JsonAssemblyObject>>(dic);
            list.Sort(
                (KeyValuePair<int, JsonAssemblyObject> k1, KeyValuePair<int, JsonAssemblyObject> k2) => { return k1.Key.CompareTo(k2.Key); }
                     );
            dic.Clear();
            foreach (var pair in list)
            {
                dic.Add(pair.Key, pair.Value);
            }
        }
    }

    protected override void OnLoad()
    {
        ReadJsonDataFromLocal(LABOBJFILE);
    }

    protected override void OnRelease()
    {

    }

    private void ReadJsonDataFromLocal(string fileName)
    {
        string parth = Application.streamingAssetsPath + '/' + fileName + ".json";
        if (File.Exists(parth))
        {
            FileStream fs = new FileStream(parth, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string[] lines = sr.ReadToEnd().Split('\n');
            for (int i = 0; i < lines.Length - 1; ++i)
            {
                JsonAssemblyObject jsonLabObject = JsonMapper.ToObject<JsonAssemblyObject>(lines[i]);
                _ID_JsonAssembyObject_Dic.Add(jsonLabObject.ID, jsonLabObject);
            }
            fs.Close();
            fs.Dispose();
        }
        else
        {
            Debug.LogError("Can't Find " + fileName + ".json in StreamingAssets");
        }
    }

    private void SaveJsonDataToLocal(string fileName)
    {

    }

    #region GetOrSetSomeValue

    /// <summary>
    /// Add LabObjects tranform to the Dic
    /// </summary>
    /// <param name="id"></param>
    /// <param name="transform"></param>
    public void RegisterAssemblyObjectTransform(int id, Transform transform)
    {
        try
        {
            if (_ID_TransformList_Dic.ContainsKey(id))
            {
                _ID_TransformList_Dic[id].Add(transform);
            }
            else
            {
                _ID_TransformList_Dic.Add(id, new List<Transform>() { transform });
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public void UnRegisterAssemblyObjectTransform(int id,Transform trans)
    {
        if(!IsTransformContainsID(id))
        {
            return;
        }
        if(!_ID_TransformList_Dic[id].Contains(trans))
        {
            Debug.LogError("The transform hasn't Registered!");
            return;
        }
        _ID_TransformList_Dic[id].Remove(trans);
    }

    public bool IsJsonContainsID(int id)
    {
        if (_ID_JsonAssembyObject_Dic.ContainsKey(id))
        {
            return true;
        }
        else
        {
            Debug.LogError("The ID-JsonLabObj-Dic don't contains ID of " + id);
            return false;
        }
    }

    public bool IsTransformContainsID(int id)
    {
        if (_ID_TransformList_Dic.ContainsKey(id))
        {
            return true;
        }
        else
        {
            //Debug.LogError("The _ID_TransformList_Dic don't contains ID of " + id);
            return false;
        }
    }

    public Transform[] GetTransformsOfSameID(int id, Transform self)
    {
        if (!_ID_TransformList_Dic.ContainsKey(id))
        {
            return null;
        }
        else
        {
            List<Transform> newList = new List<Transform>();
            for (int i = 0; i < _ID_TransformList_Dic[id].Count; ++i)
            {
                newList.Add(_ID_TransformList_Dic[id][i]);
            }
            newList.Remove(self);
            return newList.ToArray();
        }
    }

    public AssemblyObject[] GetAssemblyOfSameID(int id, Transform self)
    {
        Transform[] transformsOfSameID = GetTransformsOfSameID(id, self);
        if(transformsOfSameID==null)
        {
            return null;
        }
        List<AssemblyObject> list = new List<AssemblyObject>();
        for (int i = 0; i < transformsOfSameID.Length; ++i)
        {
            list.Add(transformsOfSameID[i].GetComponent<AssemblyObject>());
        }
        return list.ToArray();
    }

    public Transform[] GetTransformsByID(int id)
    {
        if (!IsTransformContainsID(id))
        {
            return null;
        }
        return _ID_TransformList_Dic[id].ToArray();
    }

    public Transform[] GetTransformWithoutSelfByID(int id,Transform transform)
    {
        if(GetTransformsByID(id)==null)
        {
            return null;
        }
        List<Transform> list = new List<Transform>(GetTransformsByID(id));
        list.Remove(transform);
        if(list.Count==0)
        {
            return null;
        }
        else
        {
            return list.ToArray();
        }
    }

    public bool IsSameIDInSamePos(int id,Transform transform,Vector3 pos)
    {
        Transform[] transformArry = GetTransformWithoutSelfByID(id, transform);
        if(transformArry==null)
        {
            return false;
        }
        for(int i=0;i<transformArry.Length;++i)
        {
            if(Vector3.Distance(transformArry[i].position,pos)<0.01f)
            {
                return true;
            }
        }
        return false;
    }

    public Transform[] GetParentTransforms(int id)
    {
        if(!IsJsonContainsID(id)||_ID_JsonAssembyObject_Dic[id].parentIDs==null)
        {
            return null;
        }
        List<Transform> list = new List<Transform>();
        int[] parentIDs= _ID_JsonAssembyObject_Dic[id].parentIDs;
        for(int i=0;i<parentIDs.Length;++i)
        {
            if(IsTransformContainsID(parentIDs[i]))
            {
                list.AddRange(_ID_TransformList_Dic[parentIDs[i]]);
            }
        }
        if(list.Count==0)
        {
            return null;
        }
        else
        {
            return list.ToArray();
        }  
    }

    public Transform[] GetAllWorldPosBrother(int id,Transform transform)
    {
        if (!IsWorldPosObject(id))
        {
            Debug.LogError("This JsonAssemblyObject is not a world Positon Object,Should not use this Funtion!");
            return null;
        }

        List<int> brotherIDs = new List<int>();
        foreach (var pair in _ID_JsonAssembyObject_Dic)
        {
            if (pair.Value.jsonWorldTransforms != null)
            {
                brotherIDs.Add(pair.Value.ID);
            }
        }
        List<Transform> brotherTrans = new List<Transform>();
        for (int i = 0; i < brotherIDs.Count; ++i)
        {

            if (_ID_TransformList_Dic.ContainsKey(brotherIDs[i]))
            {
                brotherTrans.AddRange(_ID_TransformList_Dic[brotherIDs[i]]);
            }
        }
        brotherTrans.Remove(transform);
        if (brotherTrans.Count == 0)
        {
            return null;
        }
        else
        {
            return brotherTrans.ToArray();
        }
    }

    public JsonAssemblyObject GetJsonAssemblyObjectByID(int id)
    {
        if (!IsJsonContainsID(id))
        {
            return null;
        }
        return _ID_JsonAssembyObject_Dic[id];
    }

    public bool IsWorldPosObject(int id)
    {
        if (!IsJsonContainsID(id))
        {
            return false;
        }
        if (_ID_JsonAssembyObject_Dic[id].jsonWorldTransforms == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public JsonAssemblyObject[] GetAllWorldPosBrotherJsonAssemblyObj(int id)
    {
        if(!IsJsonContainsID(id))
        {
            return null;
        }
        List<JsonAssemblyObject> list = new List<JsonAssemblyObject>();
        foreach(var pair in _ID_JsonAssembyObject_Dic)
        {
            if(pair.Value.parentIDs==null&&pair.Value.jsonWorldTransforms!=null)
            {
                list.Add(pair.Value);
            }
        }
        if(list.Count==0)
        {
            return null;
        }
        else
        {
            return list.ToArray();
        }

    }

    #endregion
}
