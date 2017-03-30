using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRFrameWork;

public class ObjPoolManager:Singleton<ObjPoolManager>
{
    private Dictionary<string, Stack<Object>> _name_Stack_Dic;

    public override void Init()
    {
        _name_Stack_Dic = new Dictionary<string, Stack<Object>>();
    }

    public  Object Get(string parth,bool isInstance=true)
    {
        int lenght = parth.LastIndexOf("/") + 1;
        string name = parth.Substring(lenght, parth.Length - lenght);
        if (isInstance)
        {
             name+= "(Clone)";
        }
        Object obj;
        if (_name_Stack_Dic.ContainsKey(name) &&_name_Stack_Dic[name].Count>0)
        {
            obj = _name_Stack_Dic[name].Pop();
            ((GameObject)obj).SetActive(true);
            //to be Reset;      
        }
        else
        {
            obj = Resources.Load(parth);
        }
        return obj;
    }

    public Object Set(GameObject o)
    {
        o.SetActive(false);
        if (_name_Stack_Dic.ContainsKey(o.name))
        {
            _name_Stack_Dic[o.name].Push(o);
        }
        else
        {
            Stack<Object> newStack = new Stack<Object>();
            newStack.Push(o);
            _name_Stack_Dic.Add(o.name, newStack);
        }
        return o;
    }

    public Object Set(Object o)
    {
        ((GameObject)o).SetActive(false);
        if (_name_Stack_Dic.ContainsKey(o.name))
        {
            _name_Stack_Dic[o.name].Push(o);
        }
        else
        {
            Stack<Object> newStack = new Stack<Object>();
            newStack.Push(o);
            _name_Stack_Dic.Add(o.name, newStack);
        }
        return o;
    }
}
