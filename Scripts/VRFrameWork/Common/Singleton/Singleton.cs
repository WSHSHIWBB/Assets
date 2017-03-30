using UnityEngine;
using System;

namespace VRFrameWork
{

      public abstract class Singleton<T> where T : class, new()
      {
            protected static T _instance = null;

            public static T Instance
            {
                  get
                  {
                        if (null == _instance)
                        {
                              _instance = new T();
                        }
                        return _instance;
                  }
            }

            protected Singleton()
            {
                  if (null != _instance)
                  {
                        throw new SingletonException("The Singleton instance of " + typeof(T).ToString() + " is not Null");
                  }
                  Init();
            }

            public virtual void Init()
            {
            }
           
      }

}
