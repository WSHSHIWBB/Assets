using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace VRFrameWork
{

      public class UIManager : Singleton<UIManager>
      {
            //The Dic of The gameobjects contains the ui scripts and this enumtype
            private Dictionary<EnumUIType, GameObject> _eUIType_UIObj_Dic; 
            //The list of The types were opening;  
            private List<EnumUIType> _openingEnumUITypes;
            public List<EnumUIType> OpenedEnumUITypes
            {
                  get
                  {
                        return _openingEnumUITypes;
                  }
                  set
                  {
                        _openingEnumUITypes = value;
                  }
            }

            public override void Init()
            {
                  _eUIType_UIObj_Dic = new Dictionary<EnumUIType, GameObject>();
                  _openingEnumUITypes = new List<EnumUIType>();
            }

            /// <summary>
            /// Add The type and the GameObject that contain the UI scripts to a dictionary
            /// </summary>
            /// <param name="type"></param>
            /// <param name="go"></param>
            public void AddUI(EnumUIType type, GameObject go)
            {
                  if (null == go)
                  {
                        Debug.LogError("The GaomeObject of type " + type.ToString() + " add to UIManager is NULL!");
                        return;
                  }
                  if (_openingEnumUITypes.Contains(type))
                  {
                        Debug.LogError("The UI Type " + type.ToString());
                        return;
                  }
                  else
                  {
                        _eUIType_UIObj_Dic.Add(type, go);
                        _openingEnumUITypes.Add(type);
                  }
            }

            /// <summary>
            /// Remove The UI type from The UIManager and call their Release fucntion
            /// </summary>
            /// <param name="type"></param>
            public void RemoveUI(EnumUIType type)
            {
                  if (!_openingEnumUITypes.Contains(type))
                  {
                        Debug.LogError("The UI of type " + type.ToString() + " you want to remove doesn't exit in the UIManager");
                  }
                  GetUI<BaseUI>(type).Release();
                  _openingEnumUITypes.Remove(type);
                  _eUIType_UIObj_Dic.Remove(type);
            }

            public GameObject GetUIObject(EnumUIType type)
            {
                  GameObject ret = null;
                  if (!_eUIType_UIObj_Dic.TryGetValue(type, out ret))
                  {
                        throw new Exception("_eUIType_UI_Dic try get " + type.ToString() + " type failed!");
                  }
                  return ret;
            }

            public T GetUI<T>(EnumUIType type) where T : BaseUI
            {
                  GameObject ret = GetUIObject(type);
                  if (null != ret)
                  {
                        return ret.GetComponent<T>();
                  }
                  else
                  {
                        return null;
                  }
            }

            public void SetActiveUI(EnumUIType type, bool isHideOthers)
            {
                  if (!_eUIType_UIObj_Dic.ContainsKey(type))
                  {
                        Debug.LogError("UIManager don't have type of " + type.ToString());
                        return;
                  }
                  if (isHideOthers)
                  {
                        for (int i = 0; i < _openingEnumUITypes.Count; ++i)
                        {
                              _eUIType_UIObj_Dic[_openingEnumUITypes[i]].SetActive(false);
                        }

                  }
                  _eUIType_UIObj_Dic[type].SetActive(true);
                  _openingEnumUITypes.Add(type);
            }

            public void SetInactiveUI(EnumUIType type)
            {
                  var ret = GetUIObject(type);
                  if (null == ret)
                  {
                        Debug.LogError("The type of " + type.ToString() + " have not been add to UIManager!");
                        return;
                  }
                  ret.SetActive(false);
                  _openingEnumUITypes.Remove(type);
            }

      }

}
