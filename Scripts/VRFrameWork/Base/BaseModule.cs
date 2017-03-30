using UnityEngine;
using System.Collections;
using System;

namespace VRFrameWork
{
      
      //BaseModule
      public class BaseModule
      {
            #region EnumObjectState
            private EnumObjectState _objectState = EnumObjectState.Initial;
            public event ObjectStateChangeEvent objectStateChangehandler;
            
            public EnumObjectState ObjectState
            {
                  get
                  {
                        return _objectState;
                  }
                  set
                  {
                        if (value == _objectState)
                              return;
                        EnumObjectState oldState = _objectState;
                        _objectState = value;
                        if (objectStateChangehandler != null)
                        {
                              objectStateChangehandler(this, _objectState, oldState);
                        }
                        OnObjectStateChanged(_objectState,oldState);
                  }
            }

            protected virtual  void OnObjectStateChanged(EnumObjectState _objectState, EnumObjectState oldState)
            {
                  
            }
            #endregion

            public enum EnumRegisterMode
            {
                  NotRegister,
                  HasRegistered,
            }

            private EnumRegisterMode _registerMode = EnumRegisterMode.NotRegister;
            public EnumRegisterMode RegisterMode
            {
                  get
                  {
                        return _registerMode;
                  }
                  set
                  {
                        _registerMode = value;
                  }
            }

            private bool _isAutoRegister = true;
            public bool IsAutoResiger
            {
                  get
                  {
                        return _isAutoRegister;
                  }
                  set
                  {
                        _isAutoRegister = value;
                  }
            }

            public void Load()
            {
                  if (_objectState != EnumObjectState.Initial)
                        return;
                  if (_isAutoRegister)
                  {
                        ModuleManager.Instance.Register(this);
                        _registerMode = EnumRegisterMode.HasRegistered;
                  }
                  OnLoad();
                  _objectState = EnumObjectState.Ready;
            }

            protected virtual void OnLoad()
            {
                  
            }

            public void Release()
            {
                  if (_objectState != EnumObjectState.Disabled)
                  {
                        _objectState = EnumObjectState.Disabled;
                        if (_registerMode == EnumRegisterMode.HasRegistered)
                        {
                              ModuleManager.Instance.UnRegister(this);
                              _registerMode = EnumRegisterMode.NotRegister;
                        }
                        OnRelease();
                  }
            }

            protected virtual  void OnRelease()
            {
                  
            }
      }

}