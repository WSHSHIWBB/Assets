using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;

public class CreatAssemblyObjsJson : Editor
{
    private const string LabObjectTag = "LabObject";
    private static Dictionary<string, int> _Name_ID_Dic = new Dictionary<string, int>();
    private static Dictionary<int, JsonAssemblyObject> _ID_JsonAssemObject_Dic = new Dictionary<int, JsonAssemblyObject>();

    private static void ReadObjectsConfigFile()
    {
        _Name_ID_Dic.Clear();
        string ObjectsConfigFileParth = Application.dataPath + "/ObjectsConfig/ObjectsConfig.json";
        if (File.Exists(ObjectsConfigFileParth))
        {
            FileStream fs = new FileStream(ObjectsConfigFileParth, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string content = sr.ReadToEnd();
            content = content.Replace("\r\n", "\n");
            string[] lines = content.Split('\n');
            for (int i = 0; i < lines.Length - 1; ++i)
            {
                ObjectInfo configedObject = JsonMapper.ToObject<ObjectInfo>(lines[i]);
                _Name_ID_Dic.Add(configedObject.EnglishName, configedObject.ID);
            }
            fs.Close();
            fs.Dispose();
        }
        else
        {
            Debug.LogError("Can't find ObjectsConfig.json file at " + ObjectsConfigFileParth);
        }
    }

    [MenuItem("Tools/CreateAssemblyObjectJsonFile")]
    private static void CreateLabObjectJsonFile()
    {
        ReadObjectsConfigFile();
        _ID_JsonAssemObject_Dic.Clear();

        GameObject[] labobjects = GameObject.FindGameObjectsWithTag(LabObjectTag);
        for (int i = 0; i < labobjects.Length; ++i)
        {
            if (_Name_ID_Dic.ContainsKey(labobjects[i].name) && labobjects[i].CompareTag(LabObjectTag) && labobjects[i].activeInHierarchy)
            {
                int id = _Name_ID_Dic[labobjects[i].name];

                string parentName = (labobjects[i].transform.parent == null ? null : labobjects[i].transform.parent.CompareTag(LabObjectTag) ? labobjects[i].transform.parent.name : null);
                int parentID = (parentName==null ? -1 : _Name_ID_Dic.ContainsKey(parentName) ? _Name_ID_Dic[parentName] : -1);

                if (_ID_JsonAssemObject_Dic.ContainsKey(id))
                {
                    if (parentID == -1)
                    {
                        List<JsonWorldTransform> list = new List<JsonWorldTransform>(_ID_JsonAssemObject_Dic[id].jsonWorldTransforms);
                        list.Add(new JsonWorldTransform(labobjects[i].transform));
                        _ID_JsonAssemObject_Dic[id].jsonWorldTransforms = list.ToArray();
                    }
                    else
                    {
                        HashSet<int> set = new HashSet<int>(_ID_JsonAssemObject_Dic[id].parentIDs);
                        set.Add(parentID);
                        _ID_JsonAssemObject_Dic[id].parentIDs = new int[set.Count];
                        set.CopyTo(_ID_JsonAssemObject_Dic[id].parentIDs);
                    }
                    

                }
                else
                {
                    JsonAssemblyObject jsonAssemObj = new JsonAssemblyObject();
                    jsonAssemObj.ID = id;
                    jsonAssemObj.Name = labobjects[i].name;

                    if (parentID != -1)
                    {
                        List<JsonWorldTransform> list = new List<JsonWorldTransform>(_ID_JsonAssemObject_Dic[id].jsonWorldTransforms);
                        list.Add(new JsonWorldTransform(labobjects[i].transform));
                        jsonAssemObj.jsonWorldTransforms = list.ToArray();
                    }
                    else
                    {
                        jsonAssemObj.parentIDs = new int[1] { parentID };
                    }
                    _ID_JsonAssemObject_Dic.Add(id,jsonAssemObj);
                }

                List<JsonChildPosInfo> jsonChildPosInfo = new List<JsonChildPosInfo>();
                for (int j = 0; j < labobjects[i].transform.childCount; ++j)
                {
                    Transform child = labobjects[i].transform.GetChild(j);
                    if (_Name_ID_Dic.ContainsKey(child.name) && child.CompareTag(LabObjectTag) && child.gameObject.activeInHierarchy)
                    {
                        int childID = _Name_ID_Dic[child.name];
                        jsonChildPosInfo.Add(new JsonChildPosInfo(childID, child.transform));
                    }
                }

                if(_ID_JsonAssemObject_Dic[id].jsonChildPosInfos==null)
                {
                    _ID_JsonAssemObject_Dic[id].jsonChildPosInfos = jsonChildPosInfo.ToArray();
                }
                else
                {
                    if(_ID_JsonAssemObject_Dic[id].jsonChildPosInfos.Length==jsonChildPosInfo.Count)
                    {
                        for(int j=0;j<jsonChildPosInfo.Count;++i)
                        {
                            if(_ID_JsonAssemObject_Dic[id].jsonChildPosInfos[j].childID!=jsonChildPosInfo[j].childID)
                            {
                                    Debug.LogError("The same id AssemblyObject has different childRen,this is not permit！");
                                    return;
                            }
                            else
                            {
                                if(Vector3.Distance(_ID_JsonAssemObject_Dic[id].jsonChildPosInfos[j].JsonChildPos.ToVector3(),jsonChildPosInfo[j].JsonChildPos.ToVector3())>0.0000001f)
                                {
                                    Debug.LogError("The same id AssemblyObject has different childRen,this is not permit！");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("The same id AssemblyObject has different childRen,this is not permit！");
                        return;
                    }
                }
            }
        }

        SaveToJson("AssemblyObjectJsonFile");
    }

    /// <summary>
    /// Sort the Dictionary ascend 
    /// </summary>
    /// <param name="dic"></param>
    private static void SortDic(Dictionary<int, JsonAssemblyObject> dic)
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

    private static void SaveToJson(string fileName)
    {
        string parth = Application.streamingAssetsPath + '/' + fileName + ".json";
        FileStream fs = new FileStream(parth, FileMode.Create);
        StreamWriter sr = new StreamWriter(fs);
        SortDic(_ID_JsonAssemObject_Dic);
        foreach (var pair in _ID_JsonAssemObject_Dic)
        {

            sr.WriteLine(JsonMapper.ToJson(pair.Value));
        }

        sr.Close();
        sr.Dispose();
        fs.Dispose();
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    /*    
   private static Dictionary<string, List<PosInfom>> _Name_PosInformList_Dic = new Dictionary<string, List<PosInfom>>();    
   [MenuItem("Tools/CreateLabObjectJsonFile")]
   private static void CreateLabObjectJsonFile()
   {
       _ID_JsonLabObject_Dic.Clear();
       MonoBehaviour[] scripts = (MonoBehaviour[])FindObjectsOfType(typeof(MonoBehaviour));
       foreach(MonoBehaviour script in scripts)
       {
           if(typeof(LabObject).IsAssignableFrom(script.GetType()))
           {
               LabObject labObject = (LabObject)script;
               JsonLabObject jsonLabObject = new JsonLabObject();
               jsonLabObject.ID = labObject.ID;
               jsonLabObject.Name = labObject.gameObject.name;
               //jsonLabObject.JsonLocalTransform = new JsonTransform(labObject.transform);
               LabObject parent = null;
               //jsonLabObject.ParentID = (parent = labObject.transform.parent==null?null:labObject.transform.parent.GetComponent<LabObject>()) == null ? -1 : parent.ID;

               List<int> children = new List<int>();
               for(int i=0;i<labObject.transform.childCount;++i)
               {
                   var child = labObject.transform.GetChild(i).GetComponent<LabObject>();
                   if(child!=null)
                   {
                       children.Add(child.ID);
                   }
               }
               if (children.Count != 0)
               {
                   //jsonLabObject.ChildrenIDs = children.ToArray();
               }
               _ID_JsonLabObject_Dic.Add(jsonLabObject.ID, jsonLabObject);
               //SortDic(_ID_JsonLabObject_Dic);
           }
       }
       SaveToJson("LabObjectJsonFile");
   }

   private class PosInfom
   {
       public string ParentName;
       public Transform ParentTrans;
       public string[] ChildRenNames;
       public JsonTransform JsonTransform;
       public PosInfom(string parentName, string[] childRenNames, JsonTransform jsontranform)
       {
           ParentName = parentName;
           ChildRenNames = childRenNames;
           JsonTransform = jsontranform;
       }
       public PosInfom()
       {
           ParentName = null;
           ChildRenNames = null;
           JsonTransform = new JsonTransform();
       }
   }

   [MenuItem("Tools/CreateLabObjectJsonFile")]
   private static void CreateLabObjectJsonFile()
   {
       _Name_PosInformList_Dic.Clear();
       _ID_JsonLabObject_Dic.Clear();

       GameObject[] labobjects = GameObject.FindGameObjectsWithTag(LabObjectTag);
       for (int i = 0; i < labobjects.Length; ++i)
       {
           string name = labobjects[i].name;


           PosInfom posInform = new PosInfom();
           posInform.JsonTransform = new JsonTransform(labobjects[i].transform);
           posInform.ParentName = labobjects[i].transform.parent == null ? null : labobjects[i].transform.parent.tag == LabObjectTag ? labobjects[i].transform.parent.name : null;
           if(posInform.ParentName != null)
           {
               posInform.ParentTrans = labobjects[i].transform.parent;
           }
           List<string> children = new List<string>();
           for (int j = 0; j < labobjects[i].transform.childCount; ++j)
           {
               Transform child = labobjects[i].transform.GetChild(j);
               if (child.tag == LabObjectTag)
                   children.Add(child.name);
           }
           if (children.Count != 0)
           {
               posInform.ChildRenNames = children.ToArray();
           }


           if (_Name_PosInformList_Dic.ContainsKey(name))
           {
               _Name_PosInformList_Dic[name].Add(posInform);
           }
           else
           {
               List<PosInfom> posInformList = new List<PosInfom>();
               posInformList.Add(posInform);
               _Name_PosInformList_Dic.Add(name, posInformList);
           }
       }

       List<string> nameList = new List<string>();
       foreach (var pair in _Name_PosInformList_Dic)
       {
           nameList.Add(pair.Key);
       }

       for(int i=0;i< nameList.Count;++i)
       {
           JsonLabObject jsonLabObject = new JsonLabObject();
           jsonLabObject.ID = i;
           jsonLabObject.Name = nameList[i];

           List<JsonPosInform> JsonPosInforms = new List<JsonPosInform>();
           for (int j = 0; j < _Name_PosInformList_Dic[nameList[i]].Count; ++j)
           {
               JsonPosInform jsonPosInform = new JsonPosInform();
               jsonPosInform.JsonTransform = _Name_PosInformList_Dic[nameList[i]][j].JsonTransform;
               string parentName = _Name_PosInformList_Dic[nameList[i]][j].ParentName;
               Transform parentTrans = _Name_PosInformList_Dic[nameList[i]][j].ParentTrans;
               if (parentName!=null)
               {
                   jsonPosInform.ParentID = nameList.IndexOf(parentName);
                   PosInfom[] parentPosInforms = _Name_PosInformList_Dic[parentName].ToArray();
                   if(parentPosInforms.Length==1)
                   {
                       jsonPosInform.ParentPosindex = 0;
                   }
                   else
                   {
                       for(int k=0;k< parentPosInforms.Length;++k)
                       {
                           if(Vector3.Distance(parentTrans.position, parentPosInforms[k].JsonTransform.JsonWorldPos.ToVector3())<0.00000000000000000001f)
                           {
                               jsonPosInform.ParentPosindex = k;
                               break;
                           }
                       }
                       if(jsonPosInform.ParentPosindex==-1)
                       {
                           Debug.LogError("Can't find parentPosIndex when parent is not null!");
                       }
                   }
               }
               string[] ChildRenNames = _Name_PosInformList_Dic[nameList[i]][j].ChildRenNames;
               if (ChildRenNames != null)
               {
                   jsonPosInform.ChildRenIDs = new int[ChildRenNames.Length];
                   for (int k = 0; k < ChildRenNames.Length; ++k)
                   {
                       jsonPosInform.ChildRenIDs[k] = nameList.IndexOf(ChildRenNames[k]);
                   }
               }

               JsonPosInforms.Add(jsonPosInform);
           }

           jsonLabObject.JsonPosInforms = JsonPosInforms.ToArray();
           _ID_JsonLabObject_Dic.Add(i,jsonLabObject);
       }

       SaveToJson("LabObjectJsonFile");
   }
   */
}
