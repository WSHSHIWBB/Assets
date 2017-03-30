using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using System.IO;
using LitJson;

public class ObjectInfo
{
    public int ID;
    public string EnglishName;
    public string ChineseName;
    public string Description;
    public string HandGestureName;
}


public class ObjectsConfigModule : BaseModule
{
    private const string ObjectsConfigFile = "ObjectsConfig.json";

    private Dictionary<string, ObjectInfo> _name_ObjectInfo_Dic = new Dictionary<string, ObjectInfo>();
    private Dictionary<int, ObjectInfo> _id_ObjectInfo_Dic = new Dictionary<int, ObjectInfo>();

    protected override void OnLoad()
    {
        ReadJsonDataFromLocal(ObjectsConfigFile);
    }

    protected override void OnRelease()
    {
        
    }

    private void ReadJsonDataFromLocal(string fileName)
    {
        string parth = Application.streamingAssetsPath + '/' + fileName;
        if (File.Exists(parth))
        {
            FileStream fs = new FileStream(parth, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string[] lines = sr.ReadToEnd().Split('\n');
            for (int i = 0; i < lines.Length - 1; ++i)
            {
                ObjectInfo objInfo = JsonMapper.ToObject<ObjectInfo>(lines[i]);
                _id_ObjectInfo_Dic.Add(objInfo.ID, objInfo);
                _name_ObjectInfo_Dic.Add(objInfo.EnglishName, objInfo);
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

    #region GetSomeValue

    public bool IsContains(int id)
    {
        if(_id_ObjectInfo_Dic.ContainsKey(id))
        {
            return true;
        }
        else
        {
            Debug.Log("The _id_ObjectInfo_Dic doesn't contain the given id of " + id);
            return false;
        }
    }

    public bool IsContains(string name)
    {
        if (_name_ObjectInfo_Dic.ContainsKey(name))
        {
            return true;
        }
        else
        {
            Debug.Log("The _name_ObjectInfo_Dic doesn't contain the given name of " + name);
            return false;
        }
    }

    public ObjectInfo GetObjectInfoByName(string name)
    {
        if(IsContains(name))
        {
            return _name_ObjectInfo_Dic[name];
        }
        else
        {
            return null;
        }
    }

    public ObjectInfo GetObjectInfoByID(int id)
    {
        if (IsContains(id))
        {
            return _id_ObjectInfo_Dic[id];
        }
        else
        {
            return null;
        }
    }

    public int GetObjectInfoIDByName(string name)
    {
        if(IsContains(name))
        {
            return _name_ObjectInfo_Dic[name].ID;
        }
        else
        {
            return -1;
        }
    }

    #endregion


}
