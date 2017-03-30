using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace VRFrameWork
{

      public class EventControler
      {
            private Dictionary<string, Delegate> _str_Del_dic = new Dictionary<string, Delegate>();
            public Dictionary<string, Delegate> Str_Del_Dic
            {
                  get
                  {
                        return _str_Del_dic;
                  }
            }

            // A List to contain permanent eventType
            private List<string> perm_Del_List = new List<string>();

            public bool ContainsEvent(string eventType)
            {
                  return _str_Del_dic.ContainsKey(eventType);
            }
            
            /// <summary>
            /// Register a EventType(string) to a Permanent,It will not be remove when "CleanUp"
            /// </summary>
            /// <param name="eventType"></param>
            public void MakeEventPermanent(string eventType)
            {
                  if (!perm_Del_List.Contains(eventType))
                        perm_Del_List.Add(eventType);
            }

            /// <summary>
            /// Remove the event that is not permanent
            /// </summary>
            public  void CleanUp()
            {
                  List<string> typeToMove = new List<string>();
                  foreach (KeyValuePair<string, Delegate> pair in _str_Del_dic)
                  {
                        bool ispermanent = false;
                        foreach (string eventType in perm_Del_List)
                        {
                              if (eventType == pair.Key)
                              {
                                    ispermanent = true;
                                    break;
                              }
                        }
                        if (!ispermanent)
                        {
                              typeToMove.Add(pair.Key);
                        }
                  }

                  foreach (string eventType in typeToMove)
                  {
                        _str_Del_dic.Remove(eventType);
                  }
            }
            /// <summary>
            /// Check the type of delegate to  be added  is equal to that had been added 
            /// </summary>
            /// <param name="eventType"></param>
            /// <returns></returns>
            private void  BeforeAdding(string eventType,Delegate someDelegate)
            {
                  if (!_str_Del_dic.ContainsKey(eventType))
                  {
                        _str_Del_dic.Add(eventType, null);
                  }
                  Delegate d = _str_Del_dic[eventType];
                  if (null != d && d.GetType() != someDelegate.GetType())
                  {
                        throw new EventException(String.Format("Try to add a incorrect event{0},current event type is {1},adding is {2}"
                        ,eventType,d.GetType().Name, someDelegate.GetType().Name));
                  }
            }

            /// <summary>
            /// Check the type of delegate to be removed 
            /// </summary>
            /// <param name="eventType"></param>
            /// <param name="someDelegate"></param>
            /// <returns></returns>
            private bool BeforeRemoving(string eventType, Delegate someDelegate)
            {
                  if (!_str_Del_dic.ContainsKey(eventType))
                  {
                        return false;
                  }
                  Delegate d = _str_Del_dic[eventType];
                  if (null != d && d.GetType() != someDelegate.GetType())
                  {
                        throw new EventException(string.Format("Remove event {0} failed,current event type is {1},removing type is {2}"
                        , eventType, d.GetType().Name, someDelegate.GetType().Name));
                  }
                  else
                        return true;
            }

            /// <summary>
            /// Remove the key After Remove if the delegate is null
            /// </summary>
            /// <param name="eventType"></param>
            private void AfterRemoved(string eventType)
            {
                  if (_str_Del_dic.ContainsKey(eventType) && _str_Del_dic[eventType] == null)
                  {
                        _str_Del_dic.Remove(eventType);
                  }
            }

            #region Add a handler to a eventType
            //No arg
            public void AddEvent(string eventType, Action handler)
            {
                  BeforeAdding(eventType, handler);
                  _str_Del_dic[eventType] = (Action)_str_Del_dic[eventType] + handler;
            }

            //One arg
            public void AddEvent<T>(string eventType, Action<T> hanndler)
            {
                  BeforeAdding(eventType, hanndler);
                  _str_Del_dic[eventType] = (Action<T>)_str_Del_dic[eventType] + hanndler;
            }

            //Two args
            public void AddEvent<T, U>(string eventType, Action<T, U> handler)
            {
                  BeforeAdding(eventType, handler);
                  _str_Del_dic[eventType] = (Action<T, U>)_str_Del_dic[eventType] + handler;
            }

            //Three args
            public void AddEvent<T, U, V>(string eventType, Action<T, U, V> handler)
            {
                  BeforeAdding(eventType, handler);
                  _str_Del_dic[eventType] = (Action<T, U, V>)_str_Del_dic[eventType] + handler;
            }

            //Four args
            public void AddEvent<T, U, V, W>(string eventType, Action<T, U, V, W> handler)
            {
                  BeforeAdding(eventType, handler);
                  _str_Del_dic[eventType]=(Action<T,U,V,W >) _str_Del_dic[eventType] + handler;
            }
            #endregion

            #region Remove a handler to a eventType
            //No arg
            public void RemoveEvent(string eventType,Action handler)
            {
                  if (BeforeRemoving(eventType, handler))
                  {
                        _str_Del_dic[eventType] = (Action)_str_Del_dic[eventType] - handler;
                        AfterRemoved(eventType);
                  }
            }

           //One arg
            public void RemoveEvent<T>(string eventType, Action<T> handler)
            {
                  if (BeforeRemoving(eventType, handler))
                  {
                        _str_Del_dic[eventType] = (Action<T>)_str_Del_dic[eventType] - handler;
                        AfterRemoved(eventType);
                  }
            }

            //Two arg
            public void RemoveEvent<T,U>(string eventType, Action<T,U> handler)
            {
                  if (BeforeRemoving(eventType, handler))
                  {
                        _str_Del_dic[eventType] = (Action<T,U>)_str_Del_dic[eventType] - handler;
                        AfterRemoved(eventType);
                  }
            }
            
            //Three arg
            public void RemoveEvent<T,U,V>(string eventType, Action<T,U,V> handler)
            {
                  if (BeforeRemoving(eventType, handler))
                  {
                        _str_Del_dic[eventType] = (Action<T,U,V>)_str_Del_dic[eventType] - handler;
                        AfterRemoved(eventType);
                  }
            }

            //Four arg
            public void RemoveEvent<T,U,V,W>(string eventType, Action<T,U,V,W> handler)
            {
                  if (BeforeRemoving(eventType, handler))
                  {
                        _str_Del_dic[eventType] = (Action<T,U,V,W>)_str_Del_dic[eventType] - handler;
                        AfterRemoved(eventType);
                  }
            }
            #endregion

            #region Invoke all handler of a eventType
            //no arg
            public void TriggerEvent(string eventType)
            {
                  Delegate d;
                  if (!_str_Del_dic.TryGetValue(eventType, out d))
                  {
                        return;
                  }
                  var handlerList = d.GetInvocationList();
                  for (int i = 0; i < handlerList.Length; ++i)
                  {
                        Action handler = (Action)handlerList[i];
                        if (null == handler)
                        {
                              throw new EventException(String.Format("There is NUll reference in The handlerList of type{0}", eventType));
                        }

                        try
                        {
                              handler();
                        }
                        catch (Exception e)
                        {
                              throw (e);
                        }

                  }
            }

            //One arg
            public void TriggerEvent<T>(string eventType,T arg1)
            {
                  Delegate d;
                  if (!_str_Del_dic.TryGetValue(eventType, out d))
                  {
                        return;
                  }
                  var handlerList = d.GetInvocationList();
                  for (int i = 0; i < handlerList.Length; ++i)
                  {
                        Action<T> handler = (Action<T>)handlerList[i];
                        if (null == handler)
                        {
                              throw new EventException(String.Format("There is NUll reference in The handlerList of type{0}", eventType));
                        }

                        try
                        {
                              handler(arg1);
                        }
                        catch (Exception e)
                        {
                              throw (e);
                        }

                  }
            }

            //Two args
            public void TriggerEvent<T,U>(string eventType, T arg1,U arg2)
            {
                  Delegate d;
                  if (!_str_Del_dic.TryGetValue(eventType, out d))
                  {
                        return;
                  }
                  var handlerList = d.GetInvocationList();
                  for (int i = 0; i < handlerList.Length; ++i)
                  {
                        Action<T,U> handler = (Action<T,U>)handlerList[i];
                        if (null == handler)
                        {
                              throw new EventException(String.Format("There is NUll reference in The handlerList of type{0}", eventType));
                        }

                        try
                        {
                              handler(arg1,arg2);
                        }
                        catch (Exception e)
                        {
                              throw (e);
                        }

                  }
            }

            //Three args
            public void TriggerEvent<T,U,V>(string eventType, T arg1,U arg2,V arg3)
            {
                  Delegate d;
                  if (!_str_Del_dic.TryGetValue(eventType, out d))
                  {
                        return;
                  }
                  var handlerList = d.GetInvocationList();
                  for (int i = 0; i < handlerList.Length; ++i)
                  {
                        Action<T,U,V> handler = (Action<T,U,V>)handlerList[i];
                        if (null == handler)
                        {
                              throw new EventException(String.Format("There is NUll reference in The handlerList of type{0}", eventType));
                        }

                        try
                        {
                              handler(arg1,arg2,arg3);
                        }
                        catch (Exception e)
                        {
                              throw (e);
                        }

                  }
            }

            //Four args
            public void TriggerEvent<T,U,V,W> (string eventType, T arg1,U arg2,V arg3,W arg4)
            {
                  Delegate d;
                  if (!_str_Del_dic.TryGetValue(eventType, out d))
                  {
                        return;
                  }
                  var handlerList = d.GetInvocationList();
                  for (int i = 0; i < handlerList.Length; ++i)
                  {
                        Action<T,U,V,W> handler = (Action<T,U,V,W>)handlerList[i];
                        if (null == handler)
                        {
                              throw new EventException(String.Format("There is NUll reference in The handlerList of type{0}", eventType));
                        }

                        try
                        {
                              handler(arg1,arg2,arg3,arg4);
                        }
                        catch (Exception e)
                        {
                              throw (e);
                        }

                  }
            }
            #endregion

      }
}
