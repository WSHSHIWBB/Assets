using UnityEngine;
using System;

namespace VRFrameWork
{

      public  static class MethodExtend 
      {
            #region GetOrAddComponent
            /// <summary>
            /// Extend "GetOrAddComponet<T>()" Method for GameObject,Transform,Mono.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="go"></param>
            /// <param name="path"></param>
            /// <returns></returns>
            public static T GetOrAddComponent<T>(this GameObject go, string path = "") where T : Component
            {
                  Transform t;
                  if (string.IsNullOrEmpty(path))
                        t = go.transform;
                  else
                        t = go.transform.Find(path);
                  if (null == t)
                  {
                        Debug.LogError("GetOrAddComponent not Find GameObject at ");
                  }

                  T ret = t.GetComponent<T>();
                  if (null == ret)
                        ret = t.gameObject.AddComponent<T>();
                  return ret;
            }

            public static T GetOrAddComponent<T>(this Transform t, string path = "") where T : Component
            {
                  return t.gameObject.GetOrAddComponent<T>(path);
            }

            public static T GetOrAddComponent<T>(this MonoBehaviour mono, string path = "") where T : Component
            {
                  return mono.gameObject.GetOrAddComponent<T>(path);
            }
            #endregion

            #region GetComponentByPath
            /// <summary>
            /// Extend "GetComponentByPath<T>()" Method for GameObject,Transform,Mono.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="transform"></param>
            /// <param name="path"></param>
            /// <returns></returns>
            public static T GetComponentByPath<T>(this Transform transform,string path) where T : Component
            {
                  Transform t = transform.Find(path);
                  if (null == t)
                  {
                        Debug.LogError("GetComponentByPath not find GameObject at " + path);
                        return null;
                  }
                  T ret=t.GetComponent<T>();
                  if (null == ret)
                  {
                        Debug.LogError("GetComponentByPath not find [" + typeof(T).ToString() + "] at path " + path);
                  }
                  return ret;
            }

            public static T GetComponentByPath<T>(this GameObject go, string path) where T : Component
            {
                  return go.transform.GetComponentByPath<T>(path);
            }

            public static T GetComponentByPath<T>(this MonoBehaviour mono, string path) where T : Component
            {
                  return mono.transform.GetComponentByPath<T>(path);
            }
            #endregion
      }

}
