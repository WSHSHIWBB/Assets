using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace VRFrameWork
{

    public abstract class BaseUI : MonoBehaviour
    {
        #region ObjectStateChange
        private EnumObjectState _objectState = EnumObjectState.None;
        public event ObjectStateChangeEvent objectStateChangeHandler;

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
                if (objectStateChangeHandler != null)
                {
                    objectStateChangeHandler(this, _objectState, oldState);
                    OnObjectStateChanged(_objectState, oldState);
                }


            }
        }

        protected virtual void OnObjectStateChanged(EnumObjectState _objectState, EnumObjectState oldState)
        {

        }
        #endregion

        #region UIType

        /// <summary>
        /// A abstract function to return the UI type 
        /// </summary>
        /// <returns></returns>
        public abstract EnumUIType GetUIType();

        //Mono's Awake Function
        public void Awake()
        {
            this._objectState = EnumObjectState.Initial;
            UIManager.Instance.AddUI(GetUIType(), this.gameObject);
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            this._objectState = EnumObjectState.Loading;
            OnPlayOpenUIAudio();
        }

        //Mono's Start Functon
        public void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {

        }

        //Mono's OnEnable Functon
        public void OnEnable()
        {
            OnPlayOpenUIAnimaton();
            this._objectState = EnumObjectState.Ready;
            Enabled();
        }

        protected virtual void Enabled()
        {

        }

        //Mono's Update Function
        public void Update()
        {
            if (EnumObjectState.Ready == this._objectState)
                OnUpdate(Time.deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime)
        {

        }

        //Mono's OnDisable Function
        public void OnDisable()
        {
            OnPlayCloseUIAnimation();
            this._objectState = EnumObjectState.Disabled;
            Disable();
        }

        protected virtual void Disable()
        {

        }

        //Not a member of Mono,to be Called by UIManager
        public void Release()
        {
            OnPlayCloseUIAudio();
            OnRease();
        }

        /// <summary>
        /// The virtual function when Released,could destroy the gameobject or use a objpool
        /// </summary>
        protected virtual void OnRease()
        {
            GameObject.Destroy(this);
            //poolManager.Instance.Add(this);
        }

        /// <summary>
        /// The Animation to be played when open UI
        /// </summary>
        protected virtual void OnPlayOpenUIAnimaton()
        {

        }

        /// <summary>
        /// The Audio to be Played when open UI
        /// </summary>
        protected virtual void OnPlayOpenUIAudio()
        {

        }

        /// <summary>
        /// The Animation to be Played when close UI
        /// </summary>
        protected virtual void OnPlayCloseUIAnimation()
        {

        }

        /// <summary>
        /// The Audio to be played when close UI
        /// </summary>
        protected virtual void OnPlayCloseUIAudio()
        {

        }

        #endregion
    }

}
