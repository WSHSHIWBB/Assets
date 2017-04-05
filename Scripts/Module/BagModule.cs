using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;
using System.IO;
using LitJson;

public class BagModule : BaseModule
{
    private class BagItem
    {
        public int Index;
        public int ID;
    }

    private const string BagConfigFile= "BagConfig.json";
    private Dictionary<int,int> _index_ID_Dic = new Dictionary<int, int>();

    protected override void OnLoad()
    {
        ReadJsonDataFromLocal(BagConfigFile);
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
                BagItem bagItem = JsonMapper.ToObject<BagItem>(lines[i]);
                _index_ID_Dic.Add(bagItem.Index,bagItem.ID);
            }
            fs.Close();
            fs.Dispose();
        }
        else
        {
            Debug.LogError("Can't Find " + fileName + ".json in StreamingAssets");
        }
    }

    private void InitialBag()
    {
        int size = GetBagSize();
        if(size==0)
        {
            Debug.LogError("Bag is Empty!!");
            return;
        }
    }

    #region Get Some Info

    public int GetBagSize()
    {
        return _index_ID_Dic.Count;
    }

    private bool IsIndexValid(int index)
    {
        if(index<0||index>GetBagSize()-1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int GetIDByIndex(int index)
    {
        if(!IsIndexValid(index))
        {
            return -1;
        }
        else
        {
            return _index_ID_Dic[index];
        }
    }

    public void AddItem(int index)
    {
        //to be 
    }

    public void RemoveItem(int index)
    {
        // to be 
    }
    

    #endregion


}
