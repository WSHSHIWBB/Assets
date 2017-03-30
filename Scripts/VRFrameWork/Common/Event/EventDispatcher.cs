using System;
using System.Collections.Generic;

namespace VRFrameWork
{

      public class EventDispatcher
      {
            //Use static to make EventControler to be a singleton
            private static EventControler _eventControler = new EventControler();

            public static Dictionary<string, Delegate> Str_Del_Dic
            {
                  get
                  {
                        return _eventControler.Str_Del_Dic;
                  }
            }

            // Use static method to call singleton's method

            public static void MakeEventPermanent(string eventType)
            {
                  _eventControler.MakeEventPermanent(eventType);
            }

            public static void ClearUP()
            {
                  _eventControler.CleanUp();
            }

            #region Add a event handler to The singleton Eventcontroler
            public static void AddEvent(string eventType, Action handler, bool ispermanent)
            {
                  _eventControler.AddEvent(eventType, handler);
                  if (ispermanent)
                        _eventControler.MakeEventPermanent(eventType);
            }

            public static void AddEvent<T>(string eventType, Action<T> handler, bool ispermanent)
            {
                  _eventControler.AddEvent(eventType, handler);
                  if (ispermanent)
                        _eventControler.MakeEventPermanent(eventType);
            }

            public static void AddEvent<T,U>(string eventType, Action<T,U> handler, bool ispermanent)
            {
                  _eventControler.AddEvent(eventType, handler);
                  if (ispermanent)
                        _eventControler.MakeEventPermanent(eventType);
            }

            public static void AddEvent<T,U,V>(string eventType, Action<T,U,V> handler, bool ispermanent)
            {
                  _eventControler.AddEvent(eventType, handler);
                  if (ispermanent)
                        _eventControler.MakeEventPermanent(eventType);
            }

            public static void AddEvent<T,U,V,W>(string eventType, Action<T,U,V,W> handler, bool ispermanent)
            {
                  _eventControler.AddEvent(eventType, handler);
                  if (ispermanent)
                        _eventControler.MakeEventPermanent(eventType);
            }
            #endregion

            #region Remove a event handler to The Singleton EventControler
            public static void RemoveEvent(string eventType, Action handler)
            {
                  _eventControler.RemoveEvent(eventType, handler);
            }

            public static void RemoveEvent<T>(string eventType, Action<T> handler)
            {
                  _eventControler.RemoveEvent(eventType, handler);
            }

            public static void RemoveEvent<T,U>(string eventType, Action<T,U> handler)
            {
                  _eventControler.RemoveEvent(eventType, handler);
            }

            public static void RemoveEvent<T,U,V>(string eventType, Action<T,U,V> handler)
            {
                  _eventControler.RemoveEvent(eventType, handler);
            }

            public static void RemoveEvent<T,U,V,W>(string eventType, Action handler)
            {
                  _eventControler.RemoveEvent(eventType, handler);
            }
            #endregion

            #region Trigger a event handler to The singleton EventControler
            public static void TriggerEvent(string eventType)
            {
                  _eventControler.TriggerEvent(eventType);
            }

            public static void TriggerEvent<T>(string eventType, T arg1)
            {
                  _eventControler.TriggerEvent<T>(eventType, arg1);
            }

            public static void TriggerEvent<T, U>(string eventType, T arg1, U arg2)
            {
                  _eventControler.TriggerEvent<T, U>(eventType, arg1, arg2);
            }

            public static void TriggerEvent<T,U,V>(string eventType,T arg1,U arg2,V arg3)
            {
                  _eventControler.TriggerEvent<T,U,V>(eventType,arg1,arg2,arg3);
            }

            public static void TriggerEvent<T,U,V,W>(string eventType,T arg1,U arg2,V arg3,W arg4)
            {
                  _eventControler.TriggerEvent(eventType,arg1,arg2,arg3,arg4);
            }
            #endregion
      }

}
