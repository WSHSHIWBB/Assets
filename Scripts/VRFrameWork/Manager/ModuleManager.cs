using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

namespace VRFrameWork
{

    public class ModuleManager : Singleton<ModuleManager>
    {
        private Dictionary<string, BaseModule> _str_Module_Dic;

        public override void Init()
        {
            _str_Module_Dic = new Dictionary<string, BaseModule>();
        }

        public BaseModule Get(string name)
        {
            if (_str_Module_Dic.ContainsKey(name))
            {
                return _str_Module_Dic[name];
            }
            return null;
        }

        public T Get<T>() where T : BaseModule
        {
            Type type = typeof(T);
            if (_str_Module_Dic.ContainsKey(type.ToString()))
            {
                return _str_Module_Dic[type.ToString()] as T;
            }
            return default(T);
        }

        //To be completed
        public void RegisterAllModules()
        {
            LoadModule(typeof(ObjectsConfigModule));
            LoadModule(typeof(AssemblyObjectsModule));
        }

        public void UnRegisterAllModule()
        {
            List<string> nameList = new List<string>(_str_Module_Dic.Keys);
            for (int i = 0; i < nameList.Count; ++i)
            {
                UnRegister(nameList[i]);
            }
            _str_Module_Dic.Clear();
        }

        //To be completed
        private void LoadModule(Type moduleType)
        {
            BaseModule bm = null;
            if (moduleType == typeof(ObjectsConfigModule))
            {
                bm = new ObjectsConfigModule();
            }
            else if (moduleType == typeof(AssemblyObjectsModule))
            {
                bm = new AssemblyObjectsModule();
            }
          

            if (bm != null)
            {
                bm.IsAutoResiger = true;
                bm.Load();
            }
        }

        public void Register(string name, BaseModule module)
        {
            if (!_str_Module_Dic.ContainsKey(name))
            {
                _str_Module_Dic.Add(name, module);
            }
        }

        public void Register(BaseModule module)
        {
            Register(module.GetType().ToString(), module);
        }

        public void UnRegister(string name)
        {
            if (_str_Module_Dic.ContainsKey(name))
            {
                BaseModule module = _str_Module_Dic[name];
                module.Release();
                _str_Module_Dic.Remove(name);
                module = null;
            }
        }

        public void UnRegister(BaseModule module)
        {
            UnRegister(module.GetType().ToString());
        }
    }

}
